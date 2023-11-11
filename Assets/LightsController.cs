using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsController : MonoBehaviour
{
    public float interactionRange = 2.0f;
    public bool EnoughLightsOn = false;
    public bool fuse = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            {
                TryTurnLightsOn();
            }
        }
    }
    public void TryTurnLightsOn()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, interactionRange))
        {
            if (hit.collider.gameObject == gameObject)
                if (fuse)
                {
                    if (EnoughLightsOn)
                    {
                        Debug.Log("All lights have turned off.");
                    }
                }
        }
    }
}
