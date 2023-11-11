using UnityEngine;

public class LightToggle : MonoBehaviour
{
    private Light targetLight;
    private bool canToggle = false;

    void Start()
    {
        targetLight = GetComponent<Light>();

        if (targetLight == null)
        {
            Debug.LogError("Light component not found on the same GameObject. Please attach a Light component.");
        }
    }

    void Update()
    {
        if (canToggle && Input.GetKeyDown(KeyCode.Space))
        {
            ToggleLight();
        }
        else if (!canToggle && Input.GetKeyDown(KeyCode.Space))
        {
            targetLight.enabled = false;
            Debug.Log("You can't because All lights have been switched off");
        }
    }

    void ToggleLight()
    {
        if (targetLight != null)
        {
            if(targetLight.enabled)
            {
                targetLight.enabled = false;
            }
            else
            {
                targetLight.enabled=true;
            }
        }
        else
        {
            Debug.LogError("Light component not found. Please attach a Light component.");
        }
    }

    public void SetToggleState(bool state)
    {
        canToggle = state;
        if (!canToggle)
        {
            targetLight.enabled = false; // Automatically turn off the light when canToggle is false
        }
    }
}
