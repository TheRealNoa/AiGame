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
    public float stoppingDistance = 0.001f;
    float distance;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        // Set stopping distance for the agent
        agent.stoppingDistance = stoppingDistance;
        stoppingDistance = 0.001f;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = agent.remainingDistance;
        //stoppingDistance = 0.001f;
        // Calculate the direction from NPC to player
        Vector3 directionToPlayer = transformToFollow.position - transform.position;
        // Calculate the desired destination point with stopping distance
        Vector3 destinationPoint = transformToFollow.position - directionToPlayer.normalized * stoppingDistance;
        // Set the agent's destination to the calculated point
        agent.SetDestination(destinationPoint);
        if (distance<=2)
        {
            scriptInstance.attack_state = true;

        }
        else scriptInstance.attack_state = false;
    }

    public void Hurt(float dmg)
    {
        PlayerStats.Instance.TakeDamage(dmg);
    }
}
