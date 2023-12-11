using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XEntity.InventoryItemSystem;

public class PickupChecker : MonoBehaviour
{

    private List<InstantHarvest> selectableScripts = new List<InstantHarvest>();


    private List<string> names = new List<string>();

    [SerializeField]
    private List<string> specialNames = new List<string>
    {
    "UpstairsDoubleDoor",
    };


    [SerializeField] GameObject enemy;

    EnemyFollow enemyMain;

    private void Start()
    {
        enemyMain = enemy.GetComponent<EnemyFollow>();
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

    private void Update()
    {

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
                enemyMain.attackDistance *= Random.Range(1f,1.1f);

                Debug.Log("Chase duration increased");
                enemyMain.chaseDuration *= Random.Range(1f, 1.3f);

                Debug.Log("Enemy hurt interval increased");
                enemyMain.hurtInterval *= Random.Range(1f, 1.15f);
       }
        else
        {
            Debug.Log("Default enemy bonus added.");
        }
    }

}

