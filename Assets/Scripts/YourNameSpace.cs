using UnityEngine;
using XEntity.InventoryItemSystem;

namespace YourNamespace // Replace with your actual namespace
{
    public class CheckItemInInventory : MonoBehaviour
    {
        public string itemNameToCheck = "YourItemName"; // Replace with the name of the item you want to check

        private void Start()
        {
            // Assuming the script is attached to a GameObject with an ItemContainer component
            ItemContainer itemContainer = GetComponent<ItemContainer>();

            if (itemContainer != null)
            {
                bool hasItem = itemContainer.ContainsItemName(itemNameToCheck);

                if (hasItem)
                {
                    Debug.Log($"Inventory contains {itemNameToCheck}!");
                }
                else
                {
                    Debug.Log($"Inventory does not contain {itemNameToCheck}.");
                }
            }
            else
            {
                Debug.LogError("ItemContainer component not found on the GameObject!");
            }
        }
    }
}

