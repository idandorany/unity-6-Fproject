using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, PlayerControls.IPlayerActions
{
    private PlayerControls controls;
    private CharacterController controller;
    private Vector2 moveInput;
    private Vector3 moveDirection;
    private Weapon weapon;
    private Animator animator;

    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    private float fixedYPosition;

    void Awake()
    {
        controls = new PlayerControls();
        controls.Player.SetCallbacks(this);
        controller = GetComponent<CharacterController>();
        weapon = GetComponentInChildren<Weapon>();
        animator = GetComponent<Animator>();
        fixedYPosition = transform.position.y;
    }

    void OnEnable() => controls.Enable();
    void OnDisable() => controls.Disable();

    void Update()
    {
        // Convert input to world movement direction
        moveDirection = new Vector3(moveInput.x, 0f, moveInput.y);

        // Move the player
        controller.Move(moveDirection * moveSpeed * Time.deltaTime);

        // Update animator "Speed" parameter
        animator.SetFloat("Speed", moveDirection.magnitude);

        // Rotate towards movement direction
        if (moveDirection.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            Quaternion flatRotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, flatRotation, rotationSpeed * Time.deltaTime * 100f);
        }

        // Lock Y rotation and position
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
        transform.position = new Vector3(transform.position.x, fixedYPosition, transform.position.z);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        // Prevent ghost input
        if (moveInput.magnitude < 0.01f)
            moveInput = Vector2.zero;
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed && weapon != null)
        {
            Debug.Log("Player is attacking!");
            Collider weaponCollider = weapon.GetComponent<Collider>();
            weaponCollider.enabled = true;
            Invoke(nameof(DisableWeaponCollider), 0.1f);
        }
    }

    void DisableWeaponCollider()
    {
        Collider weaponCollider = weapon.GetComponent<Collider>();
        if (weaponCollider != null)
            weaponCollider.enabled = false;
    }
}
