using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.Apple;

public class SC_NPCFollow : MonoBehaviour
{
    [Header("Movement Settings")]
    public Transform transformToFollow;
    public Animations scriptInstance;

    public NavMeshAgent agent;

    private EnemyHealth enemyHealthScript;

    public enum State { NotSpawned, Patrol, Chase, Flee, goHeal, EndNotSpawned, EndPatrol, EndChase, EndFlee, FirstHide }
    [Header("ENEMY STATE CONTROL:")]
    public State currentState = State.NotSpawned;

    [Space(15)]

    [Header("Patrol State Parameters:")]
    public float patrolWaitTime = 2f;
    [Header("Chase State Parameters:")]
    public float chaseDuration = 30f;
    private float chaseTimer = 0f;
    [Header("Fleeing paramaters:")]
    private bool isFleeingDueToFlashlight = false;
    bool IsFleePointSet = false;
    [Header("Attack Parameters:")]
    [Range(0f, 1f)]
    public float stoppingDistance;
    [Range(0f, 10f)]
    public float hurtInterval;
    [Range(0f, 10f)]
    public float attackDistance = 3f;
    [Range(0f, 5f)]
    public float hurtAmount = 1f;
    
    private bool canAttack = true;
    [Range(0f, 5f)]
    public float patrolSpeed = 1.5f;
    [Range(0f, 7f)]
    public float chaseSpeed = 2.5f;
    [Range(0f, 7f)]
    public float fleeSpeed = 5.0f;
    [Range(0f, 7f)]
    public float endFleeSpeed = 8.0f;
    [Range(0f, 7f)]
    public float endChaseSpeed = 4.0f;
    [Range(0f, 7f)]
    public float endPatrolSpeed = 3.0f;

    [Space(15)]

    [Header("Enemy node route 1 Parameters:")]
    public Vector3 StartPosition;
    public Vector3 EndPosition;
    private bool isGoingOnPath;
    private bool playerNoticed;
    public bool checkedForPlayer;

    private bool isNotSpawnedCoroutineRunning = false;

    GameObject player;
    playerController pc;

    [SerializeField] Transform[] PathPoints;
    private int pointIndex;
    bool firstPoint;
    SC_NPCFollow enemyFollow;
    float enemySpeed;


    public bool pathActivated;

    //Improved stationarytimer method
    private Vector3 lastPosition;
    private float timer;
    private float timer1;
    private int stuckTimes = 0;
    bool once = false;

    //enemy teleport
    public float maxDistance;
    public int maxAttempts = 10;

    void Start()
    {
        player = GameObject.Find("Player");
        pc = player.GetComponent<playerController>();
        lastPosition = transform.position;
        agent = GetComponent<NavMeshAgent>();
        enemyHealthScript = GetComponent<EnemyHealth>();
        ToggleVisibilityAndInteractivity(false);
        NotSpawned();
        once = false;
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
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            currentState = State.FirstHide;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            currentState = State.goHeal;
        }
       

        if (enemyHealthScript.enemyHealth < 50)
        {
            currentState = State.goHeal;
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
                case State.goHeal:
                    agent.speed = fleeSpeed;
                    goHeal();
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
                    if (!checkedLastPos)
                    {
                        StartCoroutine(changeLastPos());
                        checkedLastPos = true;

                    }
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
                case State.goHeal:
                    agent.speed = fleeSpeed;
                    goHeal();
                    break;
            }
        }
    }
    public float positionThreshold = 0;
    bool IsInSameArea()
    {
        float distance = Vector3.Distance(transform.position, lastPosition);
        return distance <= positionThreshold;
    }

    private bool destinationSet;
    private bool inHealRoom;
    private Vector3 destination;
    private Vector3 healRoomCoordinates = new Vector3(-16.91f, -23.813f, 10.811f);
    bool goh;
    bool teleported1;
    void goHeal()
    {
        if (!goh)
        {
        if (!destinationSet)
        {
            destination = PointOutOfPlayerView();
            destinationSet = true;
            inHealRoom = false;
        }

        if (!inHealRoom)
        {
            agent.SetDestination(destination);
        }

            if (destination != null)
            {
                double distance = Vector3.Distance(transform.position, destination) - 1;
                //Debug.Log("Distance to destination: " + distance);

                if (distance < 0.001)
                {

                    TeleportAgent(healRoomCoordinates);
                    teleported1 = true;
                    inHealRoom = true;
                }
            }
        }
        if (teleported1)
        {
            if (enemyHealthScript.getHP() >= 98)
            {
                currentState = State.Patrol;
            }
        }
    }

    void TeleportAgent(Vector3 targetPosition)
    {
        agent.enabled = false;
        transform.position = healRoomCoordinates;
        agent.enabled = true;
        agent.SetDestination(transform.position);
    }



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
    void spawned()
    {
        Debug.Log("Spawning");
        ToggleVisibilityAndInteractivity(true);
        Vector3 randomPoint = PointOutOfPlayerView();
        transform.position = randomPoint;
        currentState = State.Patrol;
        isNotSpawnedCoroutineRunning = false;
    }
    void despawdned()
    {
        ToggleVisibilityAndInteractivity(false);
    }
    IEnumerator NotSpawned()
    {
        Debug.Log("Not Spawned");
        ToggleVisibilityAndInteractivity(false);

        yield return new WaitForSeconds(10);

        Debug.Log("Spawning");
        ToggleVisibilityAndInteractivity(true);
        Vector3 randomPoint = PointOutOfPlayerView();
            transform.position = randomPoint;
        randomPoint = GetRandomPointOnNavMesh(5);
        agent.SetDestination(randomPoint);
        currentState = State.Patrol;
            isNotSpawnedCoroutineRunning = false;
    }

    bool checkedLastPos;
    IEnumerator changeLastPos()
    {
        yield return new WaitForSeconds(1);
        lastPosition = transform.position;
        checkedLastPos = false;
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
    Vector3 randomPoint;
    float currentDistance;
    float distance;
    bool foundDistance1;
    private void UpdateStationaryTimer()
    {
        if (!once)
        {
            randomPoint = GetRandomPointOnNavMesh(14);
            once = true;
        }
        currentDistance = Vector3.Distance(transform.position, randomPoint);
        //Debug.Log(transform.position + " " + randomPoint);
        //Debug.Log("Current distance to point:" + currentDistance);
        timer += Time.deltaTime;
        timer1 += Time.deltaTime;
        if (foundDistance1)
        {
            randomPoint = GetRandomPointOnNavMesh(14);
            foundDistance1 = false;
            agent.SetDestination(randomPoint);
        }
        if (timer1 > 0.5)
        {
            distance = Vector3.Distance(transform.position, randomPoint);
            timer1 = -0.8f;
        }
        if (timer > 1f)
        {
            float distance2 = Vector3.Distance(transform.position, randomPoint);
            //Debug.Log("Distance 1:" + distance);
            // Debug.Log("Distance 2:" + distance2);
            if ((distance2) < (distance - 0.2))
            {
                //Debug.Log("Enemy is following the same path");
                timer = 0f;
            }
        }
        if (currentDistance <2)
        {
            foundDistance1 = true;
            Debug.Log("Chose a new patrol point.");
        }
        if (IsInSameArea())
        {
            //Debug.Log("Enemy is stuck");
            timer1 = 0f;
            timer = 0f;
            randomPoint = PointOutOfPlayerView();
            MoveToRandomPointOnNavMesh();
            Debug.LogWarning("Teleported, New destination: " + randomPoint);
            agent.SetDestination(randomPoint);
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
            float fleeDistance = Random.Range(10f, 14f);
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
        return GetRandomPointOnNavMesh(14f);
    }

    void CheckAndAttackPlayer()
    {
        float distance = Vector3.Distance(transform.position, transformToFollow.position);
        if (distance <= attackDistance && canAttack)
        {
            Hurt(hurtAmount);
            canAttack = false;
            StartCoroutine(ResetAttack(hurtInterval));
        }
    }


    IEnumerator ResetAttack(float interval)
    {
        yield return new WaitForSeconds(interval);
        canAttack = true;
        Debug.LogWarning("Reset the attack.");
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
        //Debug.Log("Random NavMesh Point: " + finalPosition);
        return finalPosition;
    }

    public void MoveToRandomPointOnNavMesh()
    {
        Vector3 currentPosition = transform.position;

        Vector3 randomDirection = Vector3.zero;
        int attempts = 0;

        while (attempts < maxAttempts)
        {
            randomDirection = Random.onUnitSphere * maxDistance;
            randomDirection += currentPosition;

            // I had some { Infinity, Infinity, Infinity } or {0,0,0} errors so eh... check, check, checkk
            if (!float.IsInfinity(randomDirection.x) && !float.IsInfinity(randomDirection.y) && !float.IsInfinity(randomDirection.z)
                && randomDirection != Vector3.zero)
            {
                break;
            }

            attempts++;
        }

        if (attempts == maxAttempts)
        {
            //can't find a valid direction lol
            return;
        }

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, maxDistance, NavMesh.AllAreas))
        {
            transform.position = (hit.position);
            attempts = 0;
        }
        else
        {
            Debug.Log("Unable to find a valid position :(");
        }
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