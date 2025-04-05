using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour, PlayerControls.IPlayerActions
{
    private PlayerControls controls;

    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Weapon weapon;
    [SerializeField] private Animator animator;

    private Vector2 moveInput;

    public float rotationSpeed = 10f;
    public float attackCooldown = 0.8f;

    private bool isAttacking = false;
    private bool canAttack = true;

    void Awake()
    {
        controls = new PlayerControls();
        controls.Player.SetCallbacks(this);

        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void OnEnable() => controls.Enable();
    void OnDisable() => controls.Disable();

    void Update()
    {
        if (isAttacking)
        {
            agent.ResetPath();
            return;
        }

        Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
        animator.SetFloat("Speed", moveDirection.magnitude);

        if (moveDirection.sqrMagnitude > 0.001f)
        {
            Vector3 targetPosition = transform.position + moveDirection.normalized;
            agent.SetDestination(targetPosition);

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
        if (isAttacking) return;

        moveInput = context.ReadValue<Vector2>();

        if (moveInput.magnitude < 0.01f)
            moveInput = Vector2.zero;
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed && weapon != null && !isAttacking && canAttack)
        {
            Debug.Log("Player is attacking!");
            isAttacking = true;
            canAttack = false;

            animator.SetTrigger("Attack");

            Collider weaponCollider = weapon.GetComponent<Collider>();
            if (weaponCollider != null)
                weaponCollider.enabled = true;

            Invoke(nameof(DisableWeaponCollider), 0.1f);
            Invoke(nameof(FinishAttack), attackCooldown);
            Invoke(nameof(ResetAttackCooldown), attackCooldown);
        }
    }

    void DisableWeaponCollider()
    {
        if (weapon != null)
        {
            Collider weaponCollider = weapon.GetComponent<Collider>();
            if (weaponCollider != null)
                weaponCollider.enabled = false;
        }
    }

    void FinishAttack() => isAttacking = false;
    void ResetAttackCooldown() => canAttack = true;
}
