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
        if (itemManager != null && itemManager.heal)
        {
            PlayerStats.Instance.Heal(1);
            Debug.Log("Player healed.");
            itemManager.heal = false;
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
