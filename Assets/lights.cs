using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLightController : MonoBehaviour
{
    [SerializeField]
    private List<Light> lightsList = new List<Light>();
    [SerializeField]
    private BoxCollider targetCollider;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryToggleLights();
        }
    }

    void TryToggleLights()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 10f))
        {
            GameObject hitObject = hit.collider.gameObject;
            if (hit.collider == targetCollider)
            {
                foreach (Light lightObject in lightsList)
                {
                    LightToggle lightToggle = lightObject.GetComponent<LightToggle>();

                    if (lightToggle != null)
                    {
                        if (lightToggle.canToggle)
                        {
                            lightToggle.ToggleLight();
                            Debug.Log("Toggled light with LightToggle script on object: " + lightObject.gameObject.name);
                        }
                        else
                        {
                            Debug.Log("All lights have been toggled off and player can't turn them on");
                        }
                    }
                    else
                    {
                        Debug.Log("LightToggle script not found on object: " + lightObject.gameObject.name);
                    }
                }
            }
        }
    }
}
