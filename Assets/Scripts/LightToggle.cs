using UnityEngine;

public class LightToggle : MonoBehaviour
{
    public Light targetLight;
    public bool canToggle = false;
    public bool toggle = false;


    void Start()
    {
        targetLight = GetComponent<Light>();
        ToggleLight();
        canToggle = false;

        if (targetLight == null)
        {
            Debug.LogError("Light component not found on the same GameObject. Please attach a Light component.");
        }
    }

    private void Update()
    {
        if (toggle)
        {
            ToggleLight();
            toggle = false;
            Debug.Log("Light has been TOGGLED");
        }
    }

    public void ToggleLight()
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
