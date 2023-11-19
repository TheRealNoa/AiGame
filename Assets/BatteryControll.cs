using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microlight.MicroBar;
using System.Security;

public class BatteryControll : MonoBehaviour
{
    const float MAX_HP = 100f;
   

    public float _showingHP;
    float showingHP
    {
        get => _showingHP; set
        {
            _showingHP = value;
            _showingHPBar.UpdateHealthBar(_showingHP);
        }
    }


    [SerializeField] MicroBar _showingHPBar;

    // Start is called before the first frame update
    void Start()
    {
        _showingHPBar.Initialize(MAX_HP);
        showingHP = 50f;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.O))
        {
            Debug.Log("up pressed");
            showingHP += 10;
        }
        else if (Input.GetKeyUp(KeyCode.P))
        {
            showingHP -= 10;
        }

    }
}
