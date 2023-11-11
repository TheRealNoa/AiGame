using UnityEngine;

public class NeonLampController : MonoBehaviour
{
    // Reference to the light component
    private Light lampLight;

    // Reference to the material with emission property
    public Material lampMaterial;

    // Boolean to track the light state
    private bool isLightOn = false;

    void Start()
    {
        // Get the Light component from the child
        lampLight = GetComponentInChildren<Light>();

        // Ensure the light component is found
        if (lampLight == null)
        {
            Debug.LogError("Light component not found on NeonLamp: " + gameObject.name);
            enabled = false; // Disable the script to prevent errors
            return;
        }

        // Ensure the material is assigned
        if (lampMaterial == null)
        {
            Debug.LogError("Material is not assigned to NeonLampController on: " + gameObject.name);
            enabled = false; // Disable the script to prevent errors
            return;
        }

    }

    void Update()
    {
        // Check for input to toggle the light state
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ToggleLight();
        }
    }

    void ToggleLight()
    {
        // Toggle the light state
        isLightOn = !isLightOn;

    }

}
