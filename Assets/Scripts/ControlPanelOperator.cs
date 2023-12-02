using UnityEngine;
using XEntity.InventoryItemSystem;

public class ControlPanelOperator : MonoBehaviour
{
    public float interactionRange = 2.0f; // Adjust this to your desired interaction range
    public string liftObjectName = "lift"; // Name of the lift object
    public string liftAnimationName; // Name of the lift's animation
    public string requiredItemName; // Name of the item required to open the door

    private Animator liftAnimator;
    ItemContainer playerInventory;



    private void Start()
    {
        GameObject playerObject = GameObject.FindWithTag("PlayerInv");
        playerInventory = playerObject.GetComponent<ItemContainer>();
        Transform liftTransform = transform.Find(liftObjectName);

        if (liftTransform != null)
        {
            liftAnimator = liftTransform.GetComponent<Animator>();

            if (liftAnimator == null)
            {
                Debug.LogError("Animator component not found on the lift object.");
            }
        }
        else
        {
            Debug.LogError("Lift object not found. Make sure the lift object is a child of the control panel or adjust the liftObjectName.");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteractWithControlPanel();
        }
    }

    private void TryInteractWithControlPanel()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, interactionRange))
        {
            if (hit.collider.gameObject == gameObject)
            {
                if (playerInventory.ContainsItemName(requiredItemName))
                {
                    // Check if liftAnimator is not null before trying to play the animation
                    if (liftAnimator != null)
                    {
                        // Play the lift animation
                        liftAnimator.Play(liftAnimationName);
                        print("Lift interaction successful");
                    }
                }
            }
        }
    }
}
