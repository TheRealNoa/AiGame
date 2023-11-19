using System.Collections;
using System.Collections.Generic;
using UnityEditor.AssetImporters;
using UnityEngine;
using XEntity.InventoryItemSystem;

public class Usage : MonoBehaviour
{
    ItemManager im;
    BatteryControll bc;
    // Start is called before the first frame update
    void Start()
    {
        GameObject finder = GameObject.Find("Item Manager");
        im = finder.GetComponent<ItemManager>();
        GameObject player = GameObject.Find("Player");
        bc = player.GetComponent<BatteryControll>();
    }

    // Update is called once per frame
    void Update()
    {
        if (im.addBattery)
        {
            if ((bc._showingHP + 20) < 100)
            {
                bc._showingHP += 20;
                Debug.Log("Added 20");
                im.addBattery = false;
                bc.used = true;
            }
            else { bc._showingHP = 100;
                im.addBattery = false;
            }
        }
        if (im.addTwoBatteries)
        {
            if ((bc._showingHP + 40) < 100)
            {
                bc._showingHP += 40;
                Debug.Log("Added 40");
                im.addTwoBatteries = false;
                bc.used = true;
            }
            else { bc._showingHP = 100;
                im.addTwoBatteries = false;
            }
        }
    }
}
