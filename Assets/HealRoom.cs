using System.Collections;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    EnemyHealth enemyHealth;
    GameObject enemy;

    public float healMultiplier = 1; // maybe if we have time, we can make the healing faster when player has advanced far in the game...

    private void Start()
    {
        enemy = GameObject.Find("Enemy");
        enemyHealth = enemy.GetComponent<EnemyHealth>();
    }

    private void OnTriggerEnter(Collider trigger1)
    {
        if (trigger1.CompareTag("Enemy"))
        {
            if (enemyHealth.getHP() >= 98)
            {
                Debug.Log("Enemy not healed");
            }
            else
            {
                StartCoroutine(HealEnemyRepeatedly());
            }
        }
    }

    void Update()
    {
        enemyHealth = enemy.GetComponent<EnemyHealth>();
    }

    IEnumerator HealEnemyRepeatedly()
    {
        while (enemyHealth.getHP() < 98)
        {
            Debug.Log("Enemy healing");
            enemyHealth.HealEnemy(2 * healMultiplier);
            yield return new WaitForSeconds(0.5f);
        }

        Debug.Log("Enemy fully healed");
    }
}
