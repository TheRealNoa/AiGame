using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microlight.MicroBar;
using System.Security;

public class EnemyHealth : MonoBehaviour
{
    const float MAX_HP = 100f;
    float _enemyHP;
    public float enemyHealth;
    public float FlashlightDamage;

    RayCastDetection rcd;

    float enemyHP {
        get => _enemyHP; set {
            _enemyHP = value;
            enemyHealth = _enemyHP;
            _enemyBar.UpdateHealthBar(_enemyHP);
        }
        
            }

    float _showingHP;
    float showingHP { get => _showingHP; set
        {
            _showingHP = value;
            _showingHPBar.UpdateHealthBar(_showingHP);
        } }


    [SerializeField] MicroBar _enemyBar;
    [SerializeField] MicroBar _showingHPBar;

    // Start is called before the first frame update
    void Start()
    {
        _enemyBar.Initialize(MAX_HP);
        _showingHPBar.Initialize(MAX_HP);
        showingHP = MAX_HP;
        enemyHP = MAX_HP;
        GameObject rd = GameObject.Find("Enemy");
        rcd = rd.GetComponent<RayCastDetection>();
        StartCoroutine(HurtEnemyRepeatedly());


    }

    IEnumerator HurtEnemyRepeatedly()
    {
        while (true)
        {
            // Check condition (rcd.isLooking && rcd.isSpecial)
            if (rcd.isLooking && rcd.isSpecial)
            {
                // Hurt the enemy
                HurtEnemy(FlashlightDamage);
                Debug.Log("Enemy was hurt");
            }

            // Wait for 0.2 seconds before the next check
            yield return new WaitForSeconds(0.5f);
        }
    }

    void HurtEnemy(float damage)
    {
        // Your logic to hurt the enemy
        enemyHP -= damage;
    }
    // Update is called once per frame

}
