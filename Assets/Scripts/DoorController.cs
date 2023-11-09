using UnityEngine;

public class DoorController : MonoBehaviour
{
    public float interactionRange = 2.0f; // Adjust this to your desired interaction range
    public string openAnimationName; // Name of the door's open animation
    public string closeAnimationName; // Name of the door's close animation

    private Animator doorAnimator;
    private bool isOpen = false;

    private void Start()
    {
        doorAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteractWithDoor();
        }
    }

    private void TryInteractWithDoor()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, interactionRange))
        {
            if (hit.collider.gameObject == gameObject)
            {
                if (isOpen)
                {
                    doorAnimator.Play(closeAnimationName);
                    isOpen = false;
                }
                else
                {
                    doorAnimator.Play(openAnimationName);
                    isOpen = true;
                }
            }
        }
    }
}
