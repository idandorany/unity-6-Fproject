using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public Transform[] patrolPoints;
    public float attackRange = 2f;
    public int damageAmount = 10;
    public float damageCooldown = 1f;

    private NavMeshAgent agent;
    private int currentPatrolIndex = 0;
    private bool canDamage = true;

    private enum State { Patrolling, Chasing, Attacking }
    private State currentState;

    [Header("Vision Settings")]
    public float viewAngle = 90f;
    public float viewDistance = 10f;
    public LayerMask playerMask;
    public LayerMask obstacleMask;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentState = State.Patrolling;

        if (patrolPoints.Length > 0)
            agent.SetDestination(patrolPoints[0].position);
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        switch (currentState)
        {
            case State.Patrolling:
                PatrolBehavior();

                if (CanSeePlayer())
                    currentState = State.Chasing;
                break;

            case State.Chasing:
                agent.SetDestination(player.position);

                if (distance <= attackRange)
                {
                    currentState = State.Attacking;
                    agent.ResetPath();
                }
                else if (!CanSeePlayer())
                {
                    currentState = State.Patrolling;
                    agent.SetDestination(patrolPoints[currentPatrolIndex].position);
                }
                break;

            case State.Attacking:
                AttackBehavior();

                if (distance > attackRange)
                    currentState = State.Chasing;
                break;
        }
    }

    void PatrolBehavior()
    {
        if (patrolPoints.Length == 0) return;

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }
    }

    void AttackBehavior()
    {
        transform.LookAt(player);

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= attackRange && canDamage)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
                canDamage = false;
                Invoke(nameof(ResetDamage), damageCooldown);
            }
        }
    }

    void ResetDamage()
    {
        canDamage = true;
    }

    bool CanSeePlayer()
    {
        Vector3 dirToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2f)
        {
            if (distanceToPlayer <= viewDistance)
            {
                if (!Physics.Raycast(transform.position, dirToPlayer, distanceToPlayer, obstacleMask))
                {
                    return true;
                }
            }
        }

        return false;
    }
}
