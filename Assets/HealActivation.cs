using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealActivation : MonoBehaviour
{
    public bool heal = false;
    // Start is called before the first frame update
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        if (heal)
        {
            PlayerStats.Instance.Heal(1);
        }
    }
}
