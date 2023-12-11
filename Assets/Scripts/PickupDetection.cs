using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using XEntity.InventoryItemSystem;

public class PickupDetection : MonoBehaviour 
{

    // USAGE DETECTION
    ItemManager im;
    public Item[] items;
    public Item[] playerPickedItems;
    private GameObject enemy;
    bool pickedUp;
    EnemyFollow enemyScript;


    //PICKUP DETECTION

    private List<InstantHarvest> selectableScripts = new List<InstantHarvest>();

    private List<string> names = new List<string>();

    [SerializeField]
    private List<string> specialNames = new List<string>
    {
    "UpstairsDoubleDoor",
    };// we can add more in the editor


    public void Start()
    {
        //GENERAL
        GameObject finder = GameObject.Find("Item Manager");
        im = finder.GetComponent<ItemManager>();
        enemy = GameObject.Find("Enemy");
        enemyScript = enemy.GetComponent<EnemyFollow>();

        //PICKUP DETECTION
        GameObject[] keys = GameObject.FindGameObjectsWithTag("Selectable");
        foreach (GameObject obj in keys)
        {
            InstantHarvest instantHarvestScript = obj.GetComponent<InstantHarvest>();
            if (instantHarvestScript != null)
            {
                selectableScripts.Add(instantHarvestScript);
                names.Add(instantHarvestScript.GetName());
            }
        }
    }

    void Update()
    {
        // USAGE DETECTION
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
        //PICKUP DETECTION
        for (int i = selectableScripts.Count - 1; i >= 0; i--)
        {
            if (selectableScripts[i] == null)
            {
                selectableScripts.RemoveAt(i);
                string comppared = names[i];
                typeChecker(comppared);
                names.RemoveAt(i);

            }
        }
    }
    void typeChecker(string s)
    {
        if (specialNames.Contains(s))
        {
            Debug.Log("Attack distance increased");
            enemyScript.attackDistance *= Random.Range(1f, 1.1f);

            Debug.Log("Chase duration increased");
            enemyScript.chaseDuration *= Random.Range(1f, 1.3f);

            Debug.Log("Enemy hurt interval increased");
            enemyScript.hurtInterval *= Random.Range(1f, 1.15f);
        }
        else
        {
            Debug.Log("Player pricked up an item but enemy stats didn't change.");
        }
    }



}