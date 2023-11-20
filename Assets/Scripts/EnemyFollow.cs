using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SC_NPCFollow : MonoBehaviour
{
    // Transform that NPC has to follow
    public Transform transformToFollow;
    public Animations scriptInstance;

    // NavMesh Agent variable
    public NavMeshAgent agent;
    // Distance to maintain from the player
    public float stoppingDistance;
    public float hurtInterval = 3f;
    public float attackDistance = 3f;
    public float hurtAmmount = 1f;
    public float enemySpeed = 3f; // Add this line for speed control
    bool canAttack = true;
    float distance;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("CheckDistanceAndHurt", 0f, hurtInterval);
        agent = GetComponent<NavMeshAgent>();
        // Set stopping distance for the agent
        agent.stoppingDistance = stoppingDistance;
        // Set the initial speed of the agent
        agent.speed = enemySpeed;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 directionToPlayer = transformToFollow.position - transform.position;
        // Calculate the desired destination point with stopping distance
        Vector3 destinationPoint = transformToFollow.position - directionToPlayer.normalized * stoppingDistance;

        agent.SetDestination(destinationPoint);
        // Set the agent's destination to the calculated point
        float distance = Vector3.Distance(transform.position, transformToFollow.position);
        if (distance <= attackDistance)
        {
            scriptInstance.attack_state = true;
        }
        else scriptInstance.attack_state = false;
    }

    private void CheckDistanceAndHurt()
    {
        // Calculate the distance between the enemy and the player
        distance = Vector3.Distance(transform.position, transformToFollow.position);
        // Check if the player is within the attack distance
        if (distance <= attackDistance)
        {
            //Debug.Log(distance);
            // Execute Hurt function
            Hurt(hurtAmmount);
        }
    }

    public void Hurt(float dmg)
    {
        PlayerStats.Instance.TakeDamage(dmg);
    }
}
