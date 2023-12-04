using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.Apple;

public class SC_NPCFollow : MonoBehaviour
{
    public Transform transformToFollow;
    public Animations scriptInstance;

    public NavMeshAgent agent;
    private EnemyHealth enemyHealthScript;

    // State Parameters
    public enum State { NotSpawned, Patrol, Chase, Flee, EndNotSpawned, EndPatrol, EndChase, EndFlee, FirstHide }
    public State currentState = State.NotSpawned;

    // Patrol State Parameters
    public float patrolWaitTime = 2f;
    private float stationaryTimer = 0f;

    // Chase State Parameters
    public float chaseDuration = 30f;
    private float chaseTimer = 0f;

    // Fleeing paramaters
    private bool isFleeingDueToFlashlight = false;
    bool IsFleePointSet = false;

    // Attack Parameters
    public float stoppingDistance = 0.001f;
    public float hurtInterval = 3f;
    public float attackDistance = 3f;
    public float hurtAmount = 1f;
    private bool canAttack = true;

    public float patrolSpeed = 1.5f;
    public float chaseSpeed = 2.5f;
    public float fleeSpeed = 5.0f;
    public float endFleeSpeed = 8.0f;
    public float endChaseSpeed = 4.0f;
    public float endPatrolSpeed = 3.0f;

    private bool isNotSpawnedCoroutineRunning = false;

    GameObject player;
    playerController pc;

    [SerializeField] Transform[] PathPoints;
    private int pointIndex;
    bool firstPoint;
    SC_NPCFollow enemyFollow;
    float enemySpeed;

    float forwardSpeed;

    public bool pathActivated;

    void Start()
    {
        player = GameObject.Find("Player");
        pc = player.GetComponent<playerController>();

        agent = GetComponent<NavMeshAgent>();
        enemyHealthScript = GetComponent<EnemyHealth>();
        ToggleVisibilityAndInteractivity(false);
        NotSpawned();
    }

    void Update()
    {
        float forwardSpeed = Vector3.Dot(agent.velocity.normalized, transform.forward);
        Debug.Log(forwardSpeed);

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
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            currentState = State.FirstHide;
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
                case State.FirstHide:
                    if (!checkedForPlayer)
                    {
                        StartCoroutine(CheckForPlayer());
                        checkedForPlayer = true;
                    }
                    FirstHide();
                    break;
            }
        }
    }

    public Vector3 StartPosition;
    public Vector3 EndPosition;
    private bool isGoingOnPath;
    private bool playerNoticed;
    public bool checkedForPlayer;
    void FirstHide()
    {
        if(!playerNoticed)
        {
            float distanceToDestination = Vector3.Distance(transform.position, agent.destination);
            distanceToDestination = distanceToDestination - 1;
            if (!pathActivated)
            {
                pathActivated = true;
                transform.position = (PathPoints[0].position);
                distanceToDestination = Vector3.Distance(transform.position, agent.destination);
                if(distanceToDestination > 0.2)
                {
                    pointIndex++;
                    isGoingOnPath = true;
                }
                else
                {
                    transform.position = (PathPoints[0].position);
                }
            }
            if (isGoingOnPath)
            {
                {
                    agent.SetDestination(PathPoints[pointIndex].position);
                    distanceToDestination = Vector3.Distance(transform.position, agent.destination);
                    distanceToDestination -= 1;
                    if (distanceToDestination < 0.05f)
                    {
                        if (pointIndex < PathPoints.Length - 1)
                        {
                            pointIndex++;
                        }
                        else
                        {
                            currentState = State.NotSpawned;
                        }
                    }
                }
            }
        }else if(playerNoticed)
        {
            currentState = State.Chase;
        }
    }
    IEnumerator CheckForPlayer()
    {
        yield return new WaitForSeconds(1);
        Debug.Log("Checked for player");
        checkedForPlayer = false;
        if (!pc.playerHidden)
        {
            float distancetoPlayer = Vector3.Distance(transform.position, transformToFollow.position);
            if (distancetoPlayer < attackDistance)
            {
                playerNoticed = true;
                Debug.Log("Player noticed");
            }
            else
            {
                playerNoticed = false;
            }
        }
        else
        {
            playerNoticed = false;
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
        if (forwardSpeed<0.5)
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
        if (currentState == State.Flee)
        {
            return;
        }
        else if (!isFleeingDueToFlashlight)
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