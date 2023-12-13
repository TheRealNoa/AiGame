using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class FlashlightToggle : MonoBehaviour
{
    public GameObject lightGO; //light gameObject to work with
    private bool isOn = false; //is flashlight on or off?
    public bool specialIsOn = false;
    GameObject lightObject;
    public BatteryControll batteryControll;
    Light lightComponent;

    // Use this for initialization
    void Start()
    {
        //set default off
        GameObject player = GameObject.Find("Player");
        GameObject lightObject = GameObject.Find("LightOuter");
        batteryControll = player.GetComponent<BatteryControll>();
        lightGO.SetActive(isOn);
    }

    // Update is called once per frame
    void Update()
    {
        if (isOn && !specialIsOn)
        {
            batteryControll.regular();
            GameObject lightObject = GameObject.Find("LightOuter");
            Light lightComponent = lightObject.GetComponent<Light>();
            Color newColor = Color.white;
            lightComponent.color = newColor;
        }
        else if (isOn && specialIsOn)
        {
            batteryControll.special();
        }

        float value = batteryControll._showingHP;
        if (value <= 0)
        {

            if (isOn)
            {
                isOn = !isOn;
                lightGO.SetActive(false);
                specialIsOn = false;
                GameObject lightObject = GameObject.Find("LightOuter");
                if(lightComponent  != null)
                {
                    lightComponent = lightObject.GetComponent<Light>();
                    Color newColor = Color.white;
                    lightComponent.color = newColor;
                }
            }
        }

        //toggle flashlight on key down
        //if (Input.GetMouseButtonDown(1)) FOR SPECIAL ATTACK
        if (Input.GetKeyDown(KeyCode.X))
        {
                if (value > 0)
                {
                    //toggle light
                    isOn = !isOn;
                    //turn light on
                    if (isOn)

                        lightGO.SetActive(true);

                    //turn light off
                    else
                    {
                        GameObject lightObject = GameObject.Find("LightOuter");
                        Light lightComponent = lightObject.GetComponent<Light>();
                        Color newColor = Color.white;
                        lightComponent.color = newColor;
                        lightGO.SetActive(false);
                        specialIsOn = false;

                    }
                }
                else
                {
                    lightGO.SetActive(false);
                    specialIsOn = false;

                }

            }
            else if ((Input.GetMouseButtonDown(1)))
            {
                if (isOn)
                {
                    if (!specialIsOn)
                    {
                        GameObject lightObject = GameObject.Find("LightOuter");
                        Light lightComponent = lightObject.GetComponent<Light>();
                        Color newColor = Color.red;
                        lightComponent.color = newColor;
                        specialIsOn = true;
                    }
                    else
                    {
                        GameObject lightObject = GameObject.Find("LightOuter");
                        Light lightComponent = lightObject.GetComponent<Light>();
                        Color newColor = Color.white;
                        lightComponent.color = newColor;
                        specialIsOn = false;
                    }
                }

                else if (!isOn)
                {
                    Debug.Log("Flashlight is off");
                    specialIsOn = false;
                }

            }

        }
}
