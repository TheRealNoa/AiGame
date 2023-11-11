using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    public bool alllights;
    public bool thislight;
    public string tag = "light";
    GameObject[] taggedObjects;
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(tag);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeBoolForTaggedObjects()
    {
        // Loop through each object and change the boolean variable
        foreach (GameObject obj in taggedObjects)
        {
            // Check if the script is attached to the object
            LightController script = obj.GetComponent<LightController>();
            if (script != null)
            {
                // Change the boolean variable
                script.alllights = true;  // Or any other value you want
            }
        }
    }
}
