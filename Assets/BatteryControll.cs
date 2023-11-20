using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microlight.MicroBar;
using System.Security;

public class BatteryControll : MonoBehaviour
{
    public const float MAX_HP = 100f;
    public const float MIN_XP = 0f;

    public float regularSpeed = 0.01f;
    public float specialSpeed = 0.2f;

    public bool used;
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
        if(_showingHP < 0) 
        {
            _showingHP = MIN_XP;
        }
        if (used)
        {
            showingHP = showingHP;
            used = false;
        }
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
    public void regular()
    {
        if (showingHP > MIN_XP)
        { showingHP -= regularSpeed; }
        else showingHP = MIN_XP;
    }
    public void special()
    {
        if (showingHP > MIN_XP)
        {
            showingHP -= specialSpeed;
        }else showingHP = MIN_XP;
    }
}
