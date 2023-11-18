/*
 *  Author: ariel oliveira [o.arielg@gmail.com]
 */

using UnityEngine;
using XEntity.InventoryItemSystem;

public class HealthBarHUDTester : MonoBehaviour
{

    public ItemManager itemManager;
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            Hurt(1);
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            Heal(1);
        }
        if (itemManager != null)
        {
            if (itemManager.healred)
            {
                Debug.Log(itemManager.healred);
                PlayerStats.Instance.Heal(1);
                Debug.Log("Player healed red.");
                itemManager.healred = false;

            }else if (itemManager.healgreen) 
            {
                PlayerStats.Instance.Heal(3);
                Debug.Log("Player healed green.");
                itemManager.healgreen = false;
            }else if(itemManager.healwhite) 
            {
                PlayerStats.Instance.Heal(2);
                Debug.Log("Player healed white.");
                itemManager.healwhite = false;
            }
        }
    }
    void Start()
    {
        // Find the ItemManager script in the scene
        itemManager = FindObjectOfType<ItemManager>();
    }
    public void AddHealth()
    {
        PlayerStats.Instance.AddHealth();
    }

    public void Heal(float health)
    {
        PlayerStats.Instance.Heal(health);
    }

    public void Hurt(float dmg)
    {
        PlayerStats.Instance.TakeDamage(dmg);
    }
}
