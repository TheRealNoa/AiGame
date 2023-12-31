using UnityEngine;
using XEntity.InventoryItemSystem;



public class DoorController : MonoBehaviour
{
    public float interactionRange = 2.0f;
    public string openAnimationName; 
    public string closeAnimationName;
    public bool LockedByDefault = false;
    public string requiredItemName;

    private Animator doorAnimator;
    private bool isOpen = false;
    private bool hasCycled = false;

    public GameObject firstdoor;
    DoorController firstDoorController;

    ItemContainer playerInventory;
    ItemManager manager;
    ItemRemovalController removal;

    private void Start()
    {
    GameObject playerObject = GameObject.FindWithTag("PlayerInv");
        playerInventory = playerObject.GetComponent<ItemContainer>();
        doorAnimator = GetComponent<Animator>();
        if (firstdoor != null)
        {
            firstDoorController = firstdoor.GetComponent<DoorController>();
        }

        ItemContainer[] containers = FindObjectsOfType<ItemContainer>();

        foreach (ItemContainer container in containers)
        {
            if (container != null)
            {
                playerInventory = container;
                break;
            }
        }

        removal = FindObjectOfType<ItemRemovalController>();

        if (playerInventory == null)
        {
            Debug.LogWarning("ItemContainer not found in the scene or doesn't have the required component.");
        }

        if (removal == null)
        {
            Debug.LogWarning("ItemRemovalController not found in the scene.");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteractWithDoor();
        }
    }
    public void TryOpenDoor()
    {
        if (!isOpen)
        {
            if(!this.LockedByDefault)
            {
                doorAnimator.Play(openAnimationName);
                isOpen = true;
                Debug.Log("Door opened by enemy");
            }
        }
    }

    public bool IsOpen
    {
        get { return isOpen; }
    }

    private void RemoveItem()
    {
        if (playerInventory != null)
        {
            if (playerInventory.ContainsItemName(requiredItemName))
            {
                ItemSlot slotToRemove = FindItemSlot(requiredItemName);
                if (slotToRemove != null)
                {
                    slotToRemove.Remove(1);
                    Debug.Log($"Removed {requiredItemName} from inventory.");                }
            }
            else
            {
                Debug.Log($"Inventory does not contain {requiredItemName}.");
            }
        }
        else
        {
            Debug.LogError("Player inventory is not assigned!");
        }
    }

    private ItemSlot FindItemSlot(string itemName)
    {
        var slots = playerInventory.GetSlots();

        foreach (ItemSlot slot in slots)
        {
            if (!slot.IsEmpty && slot.slotItem.name == itemName)
            {
                return slot;
            }
        }
        return null;
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
                        if (requiredItemName == "" || hasCycled || (!LockedByDefault))
                        {
                            doorAnimator.Play(openAnimationName);
                            isOpen = true;
                            print("door opened");
                        }
                        else
                        {
                            if (firstdoor != null)
                            {
                                print($"You need {requiredItemName} to open this door.");
                            }
                            else
                            {
                                Debug.Log("The door next is locked and needs" + firstDoorController.requiredItemName + " to open this one");
                            }
                        }
                    }
                }
                else if (LockedByDefault)
                {
                    if (playerInventory == null)
                    {
                        Debug.Log("Inventory is empty.");
                    }
                    else if (firstdoor != null)
                    {
                        if (playerInventory.ContainsItemName(requiredItemName) || firstDoorController.LockedByDefault == false)
                        {
                            doorAnimator.Play(openAnimationName);
                            isOpen = true;
                            LockedByDefault = false;
                            print("door opened");
                            RemoveItem();
                            hasCycled = true;
                        }
                        else
                        {
                            print("Door locked");
                            print($"You need to unlock the door next over to open this door.");
                        }
                    }
                    else if (playerInventory.ContainsItemName(requiredItemName))
                    {
                        doorAnimator.Play(openAnimationName);
                        isOpen = true;
                        LockedByDefault = false;
                        print("door opened");
                        RemoveItem();
                        hasCycled = true;
                    }
                    else if(requiredItemName != "")
                    {
                        Debug.Log($"Player needs {requiredItemName} to open this door.");
                    }
                    
                }
            }
        }
    }
}