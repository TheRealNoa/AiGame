using UnityEngine;

public class LightModelController : MonoBehaviour
{
    // Reference to the child light object with the Lights script
    public GameObject lightObject;

    // Reference to the Lights script on the child light object
    private Lights lightsScript;

    // Reference to the material with emission property on the light prefab
    public Material lightPrefabMaterial;

    void Start()
    {
        // Ensure the lightObject is assigned
        if (lightObject == null)
        {
            Debug.LogError("Light object is not assigned to LightParentController on: " + gameObject.name);
            enabled = false; // Disable the script to prevent errors
            return;
        }

        // Get the Lights script from the child light object
        lightsScript = lightObject.GetComponent<Lights>();

        // Ensure the Lights script is found
        if (lightsScript == null)
        {
            Debug.LogError("Lights script not found on the child light object of: " + gameObject.name);
            enabled = false; // Disable the script to prevent errors
            return;
        }

        // Ensure the lightPrefabMaterial is assigned
        if (lightPrefabMaterial == null)
        {
            Debug.LogError("Light prefab material is not assigned to LightParentController on: " + gameObject.name);
            enabled = false; // Disable the script to prevent errors
            return;
        }

        // Set the initial emission state based on the Lights script's state
        UpdateEmissionState();
    }

    void Update()
    {
        // Check for changes in the Lights script's state
        if (lightsScript.HasStateChanged)
        {
            // Update the emission state when the Lights script's state changes
            UpdateEmissionState();
        }
    }

    // Update the emission state based on the Lights script's state
    void UpdateEmissionState()
    {
        // Set the emission based on the Lights script's state
        if (lightsScript.isLightOn)
        {
            // Turn on the emission
            lightPrefabMaterial.EnableKeyword("_EMISSION");
            lightPrefabMaterial.SetColor("_EmissionColor", Color.white);
        }
        else
        {
            // Turn off the emission
            lightPrefabMaterial.DisableKeyword("_EMISSION");
        }

        // Ensure changes are applied
        lightPrefabMaterial.globalIlluminationFlags &= ~MaterialGlobalIlluminationFlags.EmissiveIsBlack;
    }
}
