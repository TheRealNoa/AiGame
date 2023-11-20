using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class SC_NPCFollow : MonoBehaviour
{
    public Transform transformToFollow;
    public Animations scriptInstance;

    public NavMeshAgent agent;

    // State Parameters
    private enum State { Patrol, Chase, Flee }
    private State currentState = State.Patrol;

    // Patrol State Parameters
    public float patrolWaitTime = 2f;
    private float stationaryTimer = 0f;

    // Chase State Parameters
    public float chaseDuration = 30f;
    private float chaseTimer = 0f;

    // Attack Parameters
    public float stoppingDistance = 0.001f;
    public float hurtInterval = 3f;
    public float attackDistance = 3f;
    public float hurtAmount = 1f;
    private bool canAttack = true;
    private float attackTimer = 0f;

    public float patrolSpeed = 1.5f;
    public float chaseSpeed = 2.5f;
    public float fleeSpeed = 5.0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = stoppingDistance;
        StartCoroutine(InitializePatrol());
    }

    IEnumerator InitializePatrol()
    {
        Debug.Log("Initializing Patrol");
        yield return new WaitForSeconds(10);
        currentState = State.Patrol;
        Debug.Log("Patrol Initialized");
        Patrol();
    }

    void Update()
    {

        switch (currentState)
        {
            case State.Patrol:
                agent.speed = patrolSpeed;
                CheckForPlayer();
                UpdateStationaryTimer();
                break;
            case State.Chase:
                agent.speed = chaseSpeed;
                ChasePlayer();
                break;
            case State.Flee:
                agent.speed = fleeSpeed;
                Flee();
                break;
        }
    }

    void Patrol()
    {
        Debug.Log("Patrolling");
        if (currentState != State.Patrol) return;

        Debug.Log("Calling GetRandomPointOnNavMesh");
        Vector3 randomPoint = GetRandomPointOnNavMesh(20);
        agent.SetDestination(randomPoint);
    }

    private void UpdateStationaryTimer()
    {
        if (agent.velocity.magnitude < 0.1f)
        {
            stationaryTimer += Time.deltaTime;

            if (stationaryTimer > 2f)
            {
                stationaryTimer = 0f;
                Vector3 randomPoint = GetRandomPointOnNavMesh(20);
                agent.SetDestination(randomPoint);
                Debug.Log("Choosing new patrol point: " + randomPoint);
            }
        }
        else
        {
            stationaryTimer = 0f;
        }
    }

    IEnumerator PauseAtPoint()
    {
        yield return new WaitForSeconds(patrolWaitTime);
        Patrol();
    }

    void ChasePlayer()
    {
        Debug.Log("Chasing");
        agent.SetDestination(transformToFollow.position);
        chaseTimer += Time.deltaTime;

        if (chaseTimer >= chaseDuration)
        {
            chaseTimer = 0f;
            currentState = State.Flee;
        }

        CheckAndAttackPlayer();
    }

    void Flee()
    {
        if (!IsFleePointSet)
        {
            Debug.Log("Fleeing");
            Vector3 fleeDirection = (transform.position - transformToFollow.position).normalized;
            Vector3 fleePoint = transform.position + fleeDirection * 40;


            NavMeshHit hit;
            if (NavMesh.SamplePosition(fleePoint, out hit, 40, NavMesh.AllAreas))
            {
                fleePoint = hit.position;
                agent.SetDestination(fleePoint);
                IsFleePointSet = true;
            }
        }
        else if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!IsInPlayerView())
            {
                currentState = State.Patrol;
                IsFleePointSet = false;
            }
        }
    }

    bool IsInPlayerView()
    {
        Vector3 directionToEnemy = transform.position - transformToFollow.position;
        float angle = Vector3.Angle(transformToFollow.forward, directionToEnemy);

        if (angle < 110f * 0.5f)
        {
            RaycastHit hit;
            if (Physics.Raycast(transformToFollow.position, directionToEnemy.normalized, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    return true;
                }
            }
        }
        return false;
    }

    bool IsFleePointSet = false;

    void CheckForPlayer()
    {
        float distance = Vector3.Distance(transform.position, transformToFollow.position);
        if (distance <= attackDistance)
        {
            currentState = Random.Range(0, 2) == 0 ? State.Chase : State.Flee;
        }
    }

    void CheckAndAttackPlayer()
    {
        float distance = Vector3.Distance(transform.position, transformToFollow.position);
        if (distance <= attackDistance && canAttack)
        {
            Hurt(hurtAmount);
            canAttack = false;
            Invoke("ResetAttack", hurtInterval);
        }
    }

    void ResetAttack()
    {
        canAttack = true;
    }

    public void Hurt(float damage)
    {
        PlayerStats.Instance.TakeDamage(damage);
        scriptInstance.attack_state = true;
    }

    Vector3 GetRandomPointOnNavMesh(float range)
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * range;
        randomDirection += transform.position;

        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, range, NavMesh.AllAreas))
        {
            finalPosition = hit.position;
        }
        Debug.Log("Random NavMesh Point: " + finalPosition);
        return finalPosition;
    }

    Vector3 GetRandomPointNotInViewOfPlayer()
    {
        const int maxAttempts = 10;
        const float playerFieldOfView = 110f; // Player's field of view angle

        for (int i = 0; i < maxAttempts; i++)
        {
            Vector3 randomPoint = GetRandomPointOnNavMesh(40); // Adjust range as needed
            Vector3 directionToPlayer = transformToFollow.position - randomPoint;

            // Check if the point is behind the player or outside their field of view
            float angle = Vector3.Angle(transformToFollow.forward, directionToPlayer);
            if (angle > playerFieldOfView / 2)
            {
                // Check if there are any obstacles blocking the line of sight to this point
                if (!Physics.Linecast(transformToFollow.position, randomPoint))
                {
                    return randomPoint; // Point is likely not in the player's view
                }
            }
        }

        return transform.position; // Fallback to current position if no point found
    }
}