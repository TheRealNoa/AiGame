using UnityEngine;
using UnityEngine.AI;

public class EnemyDoorOpener : MonoBehaviour
{
    public float doorInteractionRange = 0.1f; // Range within which doors will be opened

    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        OpenNearbyDoors();
    }

    private void OpenNearbyDoors()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, doorInteractionRange);
        foreach (var hitCollider in hitColliders)
        {
            DoorController door = hitCollider.GetComponent<DoorController>();
            if (door != null && !door.LockedByDefault)
            {
                // Only attempt to open the door if it's not locked
                if (!door.IsOpen)
                {
                    door.TryOpenDoor(); // Ensure this method exists in DoorController
                }
            }
        }
    }
}
