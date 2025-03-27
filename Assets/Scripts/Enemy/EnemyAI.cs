using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public Transform[] patrolPoints;
    public float chaseRange = 10f;
    public float attackRange = 2f;
    public int damageAmount = 10;
    public float damageCooldown = 1f;

    private NavMeshAgent agent;
    private int currentPatrolIndex = 0;
    private bool canDamage = true;

    private enum State { Patrolling, Chasing, Attacking }
    private State currentState;

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

                if (distance <= chaseRange)
                    currentState = State.Chasing;
                break;

            case State.Chasing:
                agent.SetDestination(player.position);

                if (distance <= attackRange)
                {
                    currentState = State.Attacking;
                    agent.ResetPath();
                }
                else if (distance > chaseRange)
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
}
