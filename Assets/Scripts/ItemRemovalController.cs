using UnityEngine;
using XEntity.InventoryItemSystem;

public class ItemRemovalController : MonoBehaviour
{
    // Reference to the player's inventory
    public ItemContainer playerInventory;
    DoorController controller;
    // The name of the item you want to remove
    public string itemToRemoveName = "";
    public bool is_activated;

    private void Start()
    {
        controller = GetComponent<DoorController>();
        itemToRemoveName = controller.requiredItemName;
        GameObject playerObject = GameObject.FindWithTag("PlayerInv");
        playerInventory = playerObject.GetComponent<ItemContainer>();
    }

    private void Update()
    {
        // Check for input, for example, when the 'A' key is pressed
        if (is_activated)
        {
            RemoveItem();
        }
    }

    private void RemoveItem()
    {
        // Check if the player's inventory is assigned
        if (playerInventory != null)
        {
            // Check if the inventory contains the item
            if (playerInventory.ContainsItemName(itemToRemoveName))
            {
                // Find the slot with the item and remove it
                ItemSlot slotToRemove = FindItemSlot(itemToRemoveName);
                if (slotToRemove != null)
                {
                    slotToRemove.Remove(1);
                    Debug.Log($"Removed {itemToRemoveName} from inventory.");
                    is_activated = false;
                }
            }
            else
            {
                Debug.Log($"Inventory does not contain {itemToRemoveName}.");
            }
        }
        else
        {
            Debug.LogError("Player inventory is not assigned!");
        }
    }

    private ItemSlot FindItemSlot(string itemName)
    {
        // Get the list of slots from the ItemContainer
        var slots = playerInventory.GetSlots();

        // Iterate through the inventory slots and find the slot with the specified item
        foreach (ItemSlot slot in slots)
        {
            if (!slot.IsEmpty && slot.slotItem.name == itemName)
            {
                return slot;
            }
        }
        return null;
    }
}
