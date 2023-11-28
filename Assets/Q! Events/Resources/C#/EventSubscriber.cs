using System;
using UnityEngine;
using UnityEngine.Events;

public class EventSubscriber : MonoBehaviour
{
    [Tooltip("What event is this subscriber listening to?")] public string eventID;
    [Space] public UnityEvent onTrigger; public Action onAction;

    private void Start()
    {
        FindObjectOfType<EventPublisher>().EventTriggered += Trigger;
    }
    
    /// <summary>
    /// Initializes the EventSubscriber component.
    /// </summary>
    /// <param name="eventID">Event ID to listen to.</param>
    /// <param name="onAction">Coded actions to be fired when triggered.</param>
    /// <returns>This event subscriber.</returns>
    public EventSubscriber Initialize(string eventID, Action onAction)
    {
        this.eventID = eventID;
        this.onAction = onAction;
        return this;
    }

    public void Trigger(string id)
    {
        if (this.eventID != id) return;
        onTrigger?.Invoke();
        onAction?.Invoke();
    }
}
