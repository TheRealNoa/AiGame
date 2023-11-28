using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventActor : MonoBehaviour
{
    [Header("Key Binding")]
    public string inputKey;
    public KeyCode key;
    [Header("Root Objects")]
    public Transform root;
    public Transform rootTag;
    [Header("Raycast Layer")]
    public LayerMask layer;
    private string targetTag;

    private void Start()
    {
        if (root == null) root = Camera.main.transform;
        if (rootTag == null) rootTag = root.parent;
        targetTag = !rootTag.CompareTag("Untagged") ? rootTag.tag : "";
    }

    private void Update()
    {
        
        if (((key != 0 && Input.GetKeyDown(key)) || (!string.IsNullOrEmpty(inputKey) && Input.GetButtonDown(inputKey)))
        && Physics.Raycast(root.position, root.forward, out RaycastHit hit, 3f, layer) && hit.collider.GetComponent<EventTrigger>() != null)
            hit.collider.GetComponent<EventTrigger>().Trigger(targetTag);
    }
}
