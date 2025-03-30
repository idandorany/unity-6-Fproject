using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour, PlayerControls.IPlayerActions
{
    private PlayerControls controls;
    private NavMeshAgent agent;
    private Vector2 moveInput;
    private Weapon weapon;
    private Animator animator;

    public float rotationSpeed = 10f;

    void Awake()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        if (players.Length > 1)
        {
            Debug.LogWarning("Duplicate player found. Destroying this one.");
            Destroy(gameObject);
            return;
        }

        controls = new PlayerControls();
        controls.Player.SetCallbacks(this);

        agent = GetComponent<NavMeshAgent>();
        weapon = GetComponentInChildren<Weapon>();
        animator = GetComponent<Animator>();

        if (agent != null)
        {
            agent.updateRotation = false;
            agent.updateUpAxis = false;
        }
    }

    void OnEnable()
    {
        if (controls != null)
            controls.Enable();
    }

    void OnDisable()
    {
        if (controls != null)
            controls.Disable();
    }

    void Update()
    {
        if (agent == null || animator == null) return;

        Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);

        // Update animation parameter
        animator.SetFloat("Speed", moveDirection.magnitude);

        if (moveDirection.sqrMagnitude > 0.001f)
        {
            // Move
            Vector3 targetPosition = transform.position + moveDirection.normalized;
            agent.SetDestination(targetPosition);

            // Rotate
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

        // Ignore tiny "ghost" input
        if (moveInput.magnitude < 0.01f)
            moveInput = Vector2.zero;
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed && weapon != null && animator != null)
        {
            Debug.Log("Player is attacking!");
            animator.SetTrigger("Attack");

            Collider weaponCollider = weapon.GetComponent<Collider>();
            if (weaponCollider != null)
            {
                weaponCollider.enabled = true;
                Invoke(nameof(DisableWeaponCollider), 0.1f);
            }
        }
    }

    void DisableWeaponCollider()
    {
        if (weapon != null)
        {
            Collider weaponCollider = weapon.GetComponent<Collider>();
            if (weaponCollider != null)
            {
                weaponCollider.enabled = false;
            }
        }
    }
}
