using UnityEngine;

namespace XEntity.EnemySystem
{

    // This class is responsible for detecting item pickups and notifying the EnemyFollow class.
    public class ItemPickupDetector : MonoBehaviour
    {
        public EnemyFollow enemyFollow;
        // Reference to the main game viewing camera.
        [SerializeField] private Camera mainCamera;

        // The range within which an item can be picked up.
        [SerializeField] private float pickupRange = 5f;

        // Update is called once per frame.
        private void Update()
        {
            DetectItemPickup();
        }

        // This method handles the detection of item pickups.
        private void DetectItemPickup()
        {
            if (Input.GetMouseButtonDown(0)) // Assuming left mouse click is used for picking items.
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit) && InPickupRange(hit.transform.position))
                {
                    IItem itemComponent = hit.transform.GetComponent<IItem>();
                    if (itemComponent != null)
                    {
                        // Item picked up, notify EnemyFollow.
                        enemyFollow.ItemPickedUp();
                         // Assuming the item should be destroyed after pickup.
                        
                    }
                }
            }
        }

        // Check if the target position is within the pickup range.
        private bool InPickupRange(Vector3 targetPosition)
        {
            return Vector3.Distance(targetPosition, transform.position) <= pickupRange;
        }

        // Interface for items that can be picked up.
        public interface IItem
        {
            // Define methods or properties necessary for an item here.
        }
    }
}
