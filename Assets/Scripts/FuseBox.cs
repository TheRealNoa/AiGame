using UnityEngine;
using XEntity.InventoryItemSystem;

public class GameObjectInteraction : MonoBehaviour
{
    public string requiredItemName;
    private bool hasItem = false;
    private bool on;
    ItemContainer playerInventory;
    void Start()
    {
        GameObject playerObject = GameObject.FindWithTag("PlayerInv");
        playerInventory = playerObject.GetComponent<ItemContainer>();

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteractWithObject();
        }
    }

    void TryInteractWithObject()
    {
        if (playerInventory.ContainsItemName(requiredItemName) && !hasItem)
        {
            ItemSlot slotToRemove = FindItemSlot(requiredItemName);
            ToggleLightsOff(true);
            hasItem = true;
            on = true;
            Debug.Log($"Used {requiredItemName} to toggle lights off.");
            slotToRemove.Remove(1);
            Debug.Log($"Removed {requiredItemName} from inventory.");
        }
        else if (hasItem)
        {
            if (on) 
            {
                ToggleLightsOff(false);
                Debug.Log("All lights have been toggled");
                on = false;
            }
            else if (!on)
            {
                ToggleLightsOff(true);
                Debug.Log("All lights have been toggled");
                on = true;
            }
            
        }
        else
        {
            Debug.Log($"You need {requiredItemName} to interact with this object.");
        }
    }

    void ToggleLightsOff(bool value)
    {
        LightToggle[] lightToggleScripts = FindObjectsOfType<LightToggle>();

        foreach (LightToggle lightToggle in lightToggleScripts)
        {
            lightToggle.SetToggleState(value);
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
