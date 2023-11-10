using UnityEngine;
using XEntity.InventoryItemSystem;

public class DoorController : MonoBehaviour
{
    public float interactionRange = 2.0f; // Adjust this to your desired interaction range
    public string openAnimationName; // Name of the door's open animation
    public string closeAnimationName; // Name of the door's close animation
    public bool LockedByDefault = false;
    public string requiredItemName; // Name of the item required to open the door

    private Animator doorAnimator;
    private bool isOpen = false;

    ItemContainer playerInventory;
    ItemManager manager;

    private void Start()
    {
        doorAnimator = GetComponent<Animator>();
        playerInventory = FindObjectOfType<ItemContainer>();// Assuming the player's inventory is a single instance in the scene
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteractWithDoor();
        }
    }

    private void TryInteractWithDoor()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, interactionRange))
        {
            if (hit.collider.gameObject == gameObject)
            {
                if (!LockedByDefault)
                {
                    if (isOpen)
                    {
                        doorAnimator.Play(closeAnimationName);
                        isOpen = false;
                        print("door closed");
                    }
                    else
                    {
                        // Check if the required item is in the player's inventory
                        if (playerInventory.ContainsItemName(requiredItemName))
                        {
                            doorAnimator.Play(openAnimationName);
                            isOpen = true;
                            print("door opened");
                        }
                        else
                        {
                            print($"You need {requiredItemName} to open this door.");
                        }
                    }
                }
                else if (LockedByDefault)
                {
                    if(playerInventory == null)
                    {
                        Debug.Log("lol, inv is empty");
                    }
                    if (playerInventory.ContainsItemName(requiredItemName))
                    {
                        playerInventory.UseKey
                        doorAnimator.Play(openAnimationName);
                        isOpen = true;
                        LockedByDefault = false;
                        print("door opened");
                    }
                        print("door locked aaaaaaaaaaa");
                    print($"You need {requiredItemName} to open this door.");
                }
            }
        }
    }
}
