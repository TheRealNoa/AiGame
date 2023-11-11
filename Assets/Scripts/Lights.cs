using UnityEngine;

public class Lights : MonoBehaviour
{
    public bool isLightOn = false;
    private bool previousLightState = false;

    // Reference to the Light component
    private Light myLight;

    // Add a property to check if the light state has changed
    public bool HasStateChanged => previousLightState != isLightOn;

    void Start()
    {
        // Get the Light component attached to the same GameObject
        myLight = GetComponent<Light>();

        // Ensure the light component is found
        if (myLight == null)
        {
            Debug.LogError("Light component not found on: " + gameObject.name);
            enabled = false; // Disable the script to prevent errors
            return;
        }

        // Initial setup
        UpdateLightState();
    }

    void Update()
    {
        // Example: Check for input to toggle the light state
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ToggleLight();
        }

        // Check if the light state has changed and update accordingly
        if (HasStateChanged)
        {
            UpdateLightState();
        }
    }

    void ToggleLight()
    {
        // Toggle the light state
        isLightOn = !isLightOn;
    }

    void UpdateLightState()
    {
        // Update the light's actual state (turn on or off)
        myLight.enabled = isLightOn;

        // Update the previous light state
        previousLightState = isLightOn;

        // Log the state change
        Debug.Log("Light is " + (isLightOn ? "ON" : "OFF"));
    }
}

