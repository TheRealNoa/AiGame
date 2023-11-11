using UnityEngine;
using XEntity.InventoryItemSystem;

public class GameObjectInteraction : MonoBehaviour
{
    public string requiredItemName;
    private bool hasItem = false;
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
            ToggleLightsOff(true);
            hasItem = true;
            Debug.Log($"Used {requiredItemName} to toggle lights off.");
        }
        else if (hasItem)
        {
            ToggleLightsOff(false);
            hasItem = false;
            Debug.Log("Toggling lights on/off.");
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
}
