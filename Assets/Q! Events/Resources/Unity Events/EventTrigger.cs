using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventTrigger : MonoBehaviour
{
    public bool listening;
    public string targetTag;
    [Space] public UnityEvent events, onEnter, onExit;

    public void Trigger(string tag)
    {
        if (listening && Tag(tag)) events?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (listening && Tag(other.tag)) onEnter?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (listening && Tag(other.tag)) onExit?.Invoke();
    }

    public bool Tag(string tag)
    {
        if (string.IsNullOrEmpty(targetTag)) return true;
        else if (tag == targetTag) return true;
        else return false;
    }
}
