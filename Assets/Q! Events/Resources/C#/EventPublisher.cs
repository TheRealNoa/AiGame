using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventPublisher : MonoBehaviour
{
    public void Trigger(string id) { EventTriggered?.Invoke(id); }
    public delegate void OnTrigger(string id);
    public event OnTrigger EventTriggered;
}
