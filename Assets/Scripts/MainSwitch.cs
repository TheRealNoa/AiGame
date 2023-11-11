using UnityEngine;

public class MainSwitch : MonoBehaviour
{
    void Update()
    {
        // Check for input to toggle all lights
        if (Input.GetKeyDown(KeyCode.T))
        {
            ToggleAllLights();
        }
    }

    void ToggleAllLights()
    {
        // Find all game objects with the LightToggle script
        LightToggle[] lightToggleScripts = FindObjectsOfType<LightToggle>();

        // Toggle the allLightsOff boolean for each script
        foreach (LightToggle lightToggle in lightToggleScripts)
        {
            lightToggle.ToggleAll();
        }

        // Log the state change
        Debug.Log("All lights are now " + (lightToggleScripts[0].allLightsOff ? "OFF" : "ON"));
    }
}
