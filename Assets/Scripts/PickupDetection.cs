using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using XEntity.InventoryItemSystem;

public class PickupDetection : MonoBehaviour 
{
    ItemManager im;
    public Item[] items;
    public Item[] playerPickedItems;
    private GameObject enemy;
    bool pickedUp;
    EnemyFollow enemyScript;

    public void Start()
    {
        GameObject finder = GameObject.Find("Item Manager");
        im = finder.GetComponent<ItemManager>();
        enemy = GameObject.Find("Enemy");
        enemyScript = enemy.GetComponent<EnemyFollow>();
    }

    void Update()
    {
        items = im.getItems().ToArray();
        if (im.addBattery)
        {
            Debug.Log("Player has used a battery.");    // for the single battery
            enemyScript.hurtAmount += 0.1f;
        }
        else if (im.addTwoBatteries)
        {
            Debug.Log("Player has used a double battery."); // for the double battery
            enemyScript.hurtAmount += 0.2f;
        }
        else if (im.healred)
        {
            enemyScript.hurtAmount += 0.1f;
            Debug.Log("Player has used a red medkit."); // for the +10 medkit
        }
        else if (im.healwhite)
        {
            enemyScript.hurtAmount += 0.2f;
            Debug.Log("Player has used a white medkit."); // for the +20 medkit
        }
        else if (im.healgreen)
        {
            enemyScript.hurtAmount += 0.3f;
            Debug.Log("Player has used a green medkit."); // for the +30 medkit
        }

    }
}