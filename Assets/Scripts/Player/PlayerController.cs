using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour, PlayerControls.IPlayerActions
{
    private PlayerControls controls;
    private NavMeshAgent agent;
    private Vector2 moveInput;
    private Weapon weapon;
    public Animator animator;

    public float rotationSpeed = 10f;

    void Awake()
    {
        controls = new PlayerControls();
        controls.Player.SetCallbacks(this);
        agent = GetComponent<NavMeshAgent>();
        weapon = GetComponentInChildren<Weapon>();
        animator = GetComponent<Animator>();


        // Optional: Make sure the agent doesn't auto-rotate
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void OnEnable() => controls.Enable();
    void OnDisable() => controls.Disable();

    void Update()
    {
        Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);

        // Set animation Speed based on movement magnitude
        animator.SetFloat("Speed", moveDirection.magnitude);

        // Move using NavMeshAgent
        if (moveDirection.sqrMagnitude > 0.001f)
        {
            Vector3 targetPosition = transform.position + moveDirection.normalized;
            agent.SetDestination(targetPosition);

            // Rotate toward movement
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            Quaternion smoothRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime * 100f);
            transform.rotation = smoothRotation;
        }
        else
        {
            agent.ResetPath();
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        // Filter out ghost input
        if (moveInput.magnitude < 0.01f)
            moveInput = Vector2.zero;
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed && weapon != null)
        {
            Debug.Log("Player is attacking!");
            animator.SetTrigger("Attack");

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
