using System.Collections.Generic;
using UnityEngine;
namespace XEntity.InventoryItemSystem
{
    //This script is attached to any item that is picked up by the interactor on a single click such as small rocks and sticks.
    //NOTE: The item is only added if the interactor is within the interaction range.
    public class InstantHarvest : MonoBehaviour, IInteractable
    {


        private List<string> keynamesList = new List<string>();


        public bool isHarvested = false;

        //The item that will be harvested on click.
        public Item harvestItem;

        public bool pickedup = false;
        
        public bool keyPicked = false;

        //The item is instantly added to the inventory of the interactor on interact.

        public void OnClickInteract(Interactor interactor)
        {
            //Attempt to harvest if not harvested already
            AttemptHarvest(interactor);
            checkPickup(interactor);

        }

        public void AttemptHarvest(Interactor harvestor) 
        {
            if (!isHarvested)
            {
                if (harvestor.AddToInventory(harvestItem, gameObject))
                {
                    Debug.Log("Item:" + harvestItem.name);
                    pickedup = true;
                    isHarvested = true;
                }
             }
         }

        public Item GetItem()
        {
            return harvestItem;
        }

        public string GetName()
        {
            return harvestItem.itemName;
        }

        public ItemType GetItemType()
        {
            return harvestItem.getItemType();
        }

        public void checkPickup(Interactor harvestor)
        {
            

            string name = harvestItem.name;
            foreach (string str in keynamesList)
            {
                if (str == name)
                {
                    Debug.Log("Player used a key");
                    keyPicked = true;
                }
            }
        }

    }
}
