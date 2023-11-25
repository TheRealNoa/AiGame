using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class SC_NPCFollow : MonoBehaviour
{
    public Transform transformToFollow;
    public Animations scriptInstance;

    public NavMeshAgent agent;
    private EnemyHealth enemyHealthScript;
   
    // State Parameters
    public enum State { NotSpawned, Patrol, Chase, Flee, EndNotSpawned, EndPatrol, EndChase, EndFlee }
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
    bool IsFleePointSet = false;

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
    public float endFleeSpeed = 8.0f;
    public float endChaseSpeed = 4.0f;
    public float endPatrolSpeed = 3.0f;

    private bool isNotSpawnedCoroutineRunning = false;
    


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        enemyHealthScript = GetComponent<EnemyHealth>();
        ToggleVisibilityAndInteractivity(false);
        NotSpawned();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) 
        {
            currentState = State.NotSpawned;
            StartCoroutine(NotSpawned());
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) 
        {
            currentState = State.Patrol;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3)) 
        {
            currentState = State.Chase;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4)) 
        {
            currentState = State.Flee;
        }

        if (enemyHealthScript.enemyHealth < 50)
        {
            switch (currentState)
            {
                case State.EndNotSpawned:
                    StartCoroutine(EndNotSpawned());
                    break;
                case State.EndPatrol:
                    agent.speed = endPatrolSpeed;
                    EndPatrol();
                    UpdateStationaryTimer();
                    break;
                case State.EndChase:
                    agent.speed = endChaseSpeed;
                    EndChase();
                    break;
                case State.EndFlee:
                    agent.speed = endFleeSpeed;
                    EndFlee();
                    UpdateStationaryTimer();
                    break;
            }
        }
        else {
            switch (currentState)
            {
                case State.NotSpawned:
                    if (currentState == State.NotSpawned && !isNotSpawnedCoroutineRunning)
                    {
                        StartCoroutine(NotSpawned());
                        isNotSpawnedCoroutineRunning = true;
                    }
                    break;
                case State.Patrol:
                    agent.speed = patrolSpeed;
                    Patrol();
                    UpdateStationaryTimer();
                    break;
                case State.Chase:
                    agent.speed = chaseSpeed;
                    Chase();
                    UpdateStationaryTimer();
                    break;
                case State.Flee:
                    agent.speed = fleeSpeed;
                    Flee();
                    UpdateStationaryTimer();
                    break;
            }
        }
    }


    // Start States
    IEnumerator NotSpawned()
    {
        Debug.Log("Not Spawned");
        ToggleVisibilityAndInteractivity(false);

        yield return new WaitForSeconds(10);

        Debug.Log("Spawning");
        ToggleVisibilityAndInteractivity(true);
        Vector3 randomPoint = PointOutOfPlayerView();
        transform.position = randomPoint;
        currentState = State.Patrol;
        isNotSpawnedCoroutineRunning = false;
    }

    void Patrol()
    {
        float distance = Vector3.Distance(transform.position, transformToFollow.position);
        if (distance <= attackDistance)
        {
            currentState = Random.Range(0, 2) == 0 ? State.Chase : State.Flee;
        }
    }

    void Chase()
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
            Vector3 fleePoint = PointOutOfPlayerView();
            agent.SetDestination(fleePoint);
            IsFleePointSet = true;
        }
        else if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            currentState = Random.Range(0, 2) == 0 ? State.Patrol : State.NotSpawned;
            IsFleePointSet = false;
        }
    }

    // End States
    IEnumerator EndNotSpawned()
    {
        Debug.Log("Not Spawned");
        ToggleVisibilityAndInteractivity(false);

        yield return new WaitForSeconds(10);

        Debug.Log("Spawning");
        ToggleVisibilityAndInteractivity(true);
        Vector3 randomPoint = PointOutOfPlayerView();
        transform.position = randomPoint;
        currentState = State.EndPatrol;
    }

    void EndPatrol()
    {
        float distance = Vector3.Distance(transform.position, transformToFollow.position);
        if (distance <= attackDistance)
        {
            currentState = Random.Range(0, 100) < 80 ? State.Chase : State.Flee;
        }
    }

    void EndChase()
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

    void EndFlee()
    {
        Debug.Log("Fleeing");
        if (!IsFleePointSet)
        {
            Vector3 fleePoint = PointOutOfPlayerView();
            agent.SetDestination(fleePoint);
            IsFleePointSet = true;
        }
        else if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            currentState = Random.Range(0, 2) == 0 ? State.Patrol : State.NotSpawned;
            IsFleePointSet = false;
        }
    }

    // Misc methods
    void ToggleVisibilityAndInteractivity(bool isActive)
    {

        Transform zombie1 = transform.Find("Zombie1");
        Transform healthBar = transform.Find("HealthBar");
        if (zombie1 != null)
        {
            zombie1.gameObject.SetActive(isActive);
            healthBar.gameObject.SetActive(isActive);
        }

        if (!isActive)
        {
            zombie1.gameObject.SetActive(isActive);
            healthBar.gameObject.SetActive(isActive);
        }
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

    Vector3 PointOutOfPlayerView()
    {
        const int maxAttempts = 10;
        for (int i = 0; i < maxAttempts; i++)
        {
            float fleeDistance = Random.Range(20f, 60f);
            Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * fleeDistance;
            randomDirection += transform.position;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, fleeDistance, NavMesh.AllAreas))
            {
                Vector3 potentialFleePoint = hit.position;
                Vector3 originalPosition = transform.position;
                transform.position = potentialFleePoint;
                if (!IsInPlayerView())
                {
                    transform.position = originalPosition;
                    return potentialFleePoint;
                }
                transform.position = originalPosition;
            }
        }
        return GetRandomPointOnNavMesh(40f);
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

        agent.isStopped = true;
        yield return new WaitForSeconds(3f);

        agent.isStopped = false;
        currentState = State.Flee;
        isFleeingDueToFlashlight = false;
    }
}