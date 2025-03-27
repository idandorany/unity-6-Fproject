using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, PlayerControls.IPlayerActions
{
    private PlayerControls controls;
    private CharacterController controller;
    private Vector2 moveInput;
    private Vector3 moveDirection;
    private Weapon weapon;

    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    private float fixedYPosition;

    void Awake()
    {
        controls = new PlayerControls();
        controls.Player.SetCallbacks(this);
        controller = GetComponent<CharacterController>();
        weapon = GetComponentInChildren<Weapon>();
        fixedYPosition = transform.position.y;
    }

    void OnEnable() => controls.Enable();
    void OnDisable() => controls.Disable();

    void Update()
    {
        // Convert input to world space movement
        moveDirection = new Vector3(moveInput.x, 0, moveInput.y);

        // Move the player
        if (moveDirection.sqrMagnitude > 0.0001f)
        {
            controller.Move(moveDirection * moveSpeed * Time.deltaTime);

            // Rotate only when input is valid
            Vector3 lookDirection = new Vector3(moveDirection.x, 0f, moveDirection.z);
            if (lookDirection.sqrMagnitude > 0.0001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
                Quaternion flatRotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, flatRotation, rotationSpeed * Time.deltaTime * 100f);
            }
        }

        // Always lock rotation to Y only
        Vector3 currentEuler = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0, currentEuler.y, 0);

        // Lock Y position to fixed value
        Vector3 position = transform.position;
        position.y = fixedYPosition;
        transform.position = position;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        // Ignore ghost inputs
        if (moveInput.magnitude < 0.01f)
            moveInput = Vector2.zero;
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        Debug.Log("Attack input received.");

        if (context.performed && weapon != null)
        {
            Debug.Log("Player is attacking!");

            // Enable collider briefly to detect hit
            Collider weaponCollider = weapon.GetComponent<Collider>();
            weaponCollider.enabled = true;

            Invoke(nameof(DisableWeaponCollider), 0.1f);
        }
    }

    void DisableWeaponCollider()
    {
        Collider weaponCollider = weapon.GetComponent<Collider>();
        if (weaponCollider != null)
        {
            weaponCollider.enabled = false;
        }
    }
}
