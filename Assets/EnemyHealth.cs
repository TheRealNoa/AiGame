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
       

    }

    // Update is called once per frame
    void Update()
    {
       

        if (rcd.isLooking && rcd.isSpecial)
        {
            enemyHP -= 0.1f;
        }
        //else
            //Debug.Log("Nothing is happening");


    }

}
