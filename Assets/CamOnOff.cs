using UnityEngine;
using XEntity.InventoryItemSystem;

public class CamOnOff : MonoBehaviour
{
    public ItemContainer container;
    public PlayerLook pm;

    // Start is called before the first frame update
    void Start()
    {
        GameObject cont = GameObject.Find("PlayerInventory");
        container = cont.GetComponent<ItemContainer>();
        pm = GetComponent<PlayerLook>();
        // Ensure that the references are assigned in the Inspector or through code
        if (container == null)
        {
            Debug.LogError("ItemContainer reference is not assigned.");
        }

        if (pm == null)
        {
            Debug.LogError("PlayerMotor reference is not assigned.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if container is not null before accessing its properties
        if (container != null)
        {
            // Check the cameraMovement property
            if (!container.cameraMovement)
            {
                // Disable the PlayerMotor component
                if (pm != null)
                {
                    pm.enabled = false;
                    pm.mouseSensitivity = 0;
                }
                else
                {
                    Debug.LogError("PlayerMotor reference is null.");
                }
            }
            else
            {
                // Enable the PlayerMotor component
                if (pm != null)
                {
                    pm.enabled = true;
                    pm.mouseSensitivity = 30;
                }
                else
                {
                    Debug.LogError("PlayerMotor reference is null.");
                }
            }
        }
        else
        {
            Debug.LogError("ItemContainer reference is null.");
        }
    }
}
