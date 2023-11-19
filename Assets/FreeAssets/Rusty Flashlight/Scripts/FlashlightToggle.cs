using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class FlashlightToggle : MonoBehaviour
{
    public GameObject lightGO; //light gameObject to work with
    private bool isOn = false; //is flashlight on or off?
    private bool specialIsOn = false;

    // Use this for initialization
    void Start()
    {
        //set default off
        GameObject lightObject = GameObject.Find("LightOuter");
        lightGO.SetActive(isOn);
    }

    // Update is called once per frame
    void Update()
    {
        //toggle flashlight on key down
        //if (Input.GetMouseButtonDown(1)) FOR SPECIAL ATTACK
        if (Input.GetKeyDown(KeyCode.X))
        {
            //toggle light
            isOn = !isOn;
            //turn light on
            if (isOn)
            {
                lightGO.SetActive(true);
            }
            //turn light off
            else
            {
                lightGO.SetActive(false);

            }
        }
        else if ((Input.GetMouseButtonDown(1))) 
        {
            specialIsOn = !specialIsOn;
            if (specialIsOn)
            {
                GameObject lightObject = GameObject.Find("LightOuter");
                Light lightComponent = lightObject.GetComponent<Light>();
                Color newColor = Color.red;
                lightComponent.color = newColor;
            }
            else
            {
                GameObject lightObject = GameObject.Find("LightOuter");
                Light lightComponent = lightObject.GetComponent<Light>();
                Color newColor = Color.white;
                lightComponent.color = newColor;
            }
        }

    }
}
