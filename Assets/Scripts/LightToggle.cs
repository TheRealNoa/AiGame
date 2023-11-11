using UnityEngine;

public class LightToggle : MonoBehaviour
{
    private Light myLight;
    public bool allLightsOff;
    public bool isOn;

    void Start()
    {
        // Get the Light component attached to the same GameObject
        myLight = GetComponent<Light>();

        // Ensure the light component is found
        if (myLight == null)
        {
            Debug.LogError("Light component not found on: " + gameObject.name);
            enabled = false; // Disable the script to prevent errors
        }
        myLight.enabled = false;
    }

    void Update()
    {
        // Check for input to toggle the light state
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ToggleLight(); // This should detect raycast to a switch
        }
        else {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                ToggleAll();
            }
        }
        
    }

    void ToggleLight()
    {
        if (!allLightsOff)
        {
            if (!isOn)
            {
                isOn = true;
                myLight.enabled = true;
            }
            else if (isOn)
            {
                isOn = false;
                myLight.enabled = false;
            }
            else if (allLightsOff)
            {
                if (!isOn)
                {
                    isOn = true;
                    myLight.enabled = true;
                }
                else if (isOn)
                {
                    isOn = false;
                    myLight.enabled = false;
                }
                // Log the state change
                Debug.Log("Light is " + (myLight.enabled ? "ON" : "OFF"));
            }
        }
    }
    public void ToggleAll()
    {
        if (!allLightsOff)
        {
            if (isOn)
            {
                myLight.enabled = false;
                isOn = true;
                ToggleLight();
                allLightsOff = true;
            }
            
        }
        else if (allLightsOff)// for Start of game only
        {
            myLight.enabled = false;
            isOn = false;
            allLightsOff = false;
            ToggleLight();
        }
        else Debug.Log("Lol nah cuh");
    }
}
