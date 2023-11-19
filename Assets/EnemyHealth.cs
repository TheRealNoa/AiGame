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

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.UpArrow))
        {
            Debug.Log("up pressed");
            enemyHP += 10;
            showingHP += 10;
        }else if(Input.GetKeyUp(KeyCode.DownArrow)) 
        { enemyHP -= 10;
            showingHP -= 10;
        }

    }
}
