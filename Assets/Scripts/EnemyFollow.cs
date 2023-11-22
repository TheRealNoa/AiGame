using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class SC_NPCFollow : MonoBehaviour
{
    public Transform transformToFollow;
    public Animations scriptInstance;

    public NavMeshAgent agent;

    // State Parameters
    public enum State { NotSpawned, Patrol, Chase, Flee }
    public State currentState = State.NotSpawned;

    // Patrol State Parameters
    public float patrolWaitTime = 2f;
    private float stationaryTimer = 0f;

    // Chase State Parameters
    public float chaseDuration = 30f;
    private float chaseTimer = 0f;

    // Fleeing paramaters
    private bool isFleeingDueToFlashlight = false;
    private bool isPausedByFlashlight = false;

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
        ToggleVisibilityAndInteractivity(false); // Make invisible and non-interactive
        StartCoroutine(NotSpawned());
    }

    IEnumerator NotSpawned()
    {
        Debug.Log("Not Spawned");
        ToggleVisibilityAndInteractivity(false);

        // Wait for 10 seconds
        yield return new WaitForSeconds(10);

        // Spawn at a random NavMesh point and switch to Patrol state
        Debug.Log("Spawning");
        ToggleVisibilityAndInteractivity(true);
        Vector3 randomPoint = GetRandomPointOnNavMesh(20);
        transform.position = randomPoint; // Teleport to the random point
        currentState = State.Patrol;
    }

    void ToggleVisibilityAndInteractivity(bool isActive)
    {
        // Find and toggle the child GameObject - "Zombie1"
        Transform zombie1Child = transform.Find("Zombie1");
        if (zombie1Child != null)
        {
            zombie1Child.gameObject.SetActive(isActive);
        }

        // If isActive is false, disable the agent and collider to make the parent non-interactive
        if (!isActive)
        {
          zombie1Child.gameObject.SetActive(isActive);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) // Press '1' for NotSpawned
        {
            currentState = State.NotSpawned;
            StartCoroutine(NotSpawned());
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) // Press '2' for Patrol
        {
            currentState = State.Patrol;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3)) // Press '3' for Chase
        {
            currentState = State.Chase;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4)) // Press '4' for Flee
        {
            currentState = State.Flee;
        }

            switch (currentState)
        {
            case State.NotSpawned:
                NotSpawned();
                break;
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
        Debug.Log("Fleeing");
        if (!IsFleePointSet)
        {
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
                Debug.Log("Patrol or despawn triggered");
                currentState = Random.Range(0, 2) == 0 ? State.Patrol : State.NotSpawned;
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

    public State GetCurrentState()
    {
        return currentState;
    }

    public void SpecialFlashlightHit()
    {
        if (!isFleeingDueToFlashlight)
        {
            StartCoroutine(FreezeAndFlee());
        }
    }

    private IEnumerator FreezeAndFlee()
    {
        isFleeingDueToFlashlight = true;
       
         // Freeze the enemy for 3 seconds
        agent.isStopped = true;
        yield return new WaitForSeconds(3f);

        // After 3 seconds, resume movement and switch to flee state
        agent.isStopped = false;
        currentState = State.Flee;
        isFleeingDueToFlashlight = false; // Reset the flag after starting to flee
    }




}