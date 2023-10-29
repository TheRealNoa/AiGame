using UnityEngine;
using UnityEngine.AI;

public class SC_NPCFollow : MonoBehaviour
{
    // Transform that NPC has to follow
    public Transform transformToFollow;
    // NavMesh Agent variable
    NavMeshAgent agent;
    // Distance to maintain from the player
    public float stoppingDistance = 0.001f;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        // Set stopping distance for the agent
        agent.stoppingDistance = stoppingDistance;
    }

    // Update is called once per frame
    void Update()
    {
        stoppingDistance = 0.001f;
        // Calculate the direction from NPC to player
        Vector3 directionToPlayer = transformToFollow.position - transform.position;
        // Calculate the desired destination point with stopping distance
        Vector3 destinationPoint = transformToFollow.position - directionToPlayer.normalized * stoppingDistance;

        // Set the agent's destination to the calculated point
        agent.SetDestination(destinationPoint);
    }
}
