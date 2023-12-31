using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDoorControl : MonoBehaviour
{
    [SerializeField] private Animator myDoor = null;

    [SerializeField] private bool openTrigger = false;
    [SerializeField] private bool closeTrigger = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (openTrigger)
            {
                myDoor.Play("Door1Animation", 0, 0.0f);
                gameObject.SetActive(false);
            }
            else if (closeTrigger)
            {
                myDoor.Play("Door1Close", 0, 0.0f);
                gameObject.SetActive(false);
            }
        }
    }
}
