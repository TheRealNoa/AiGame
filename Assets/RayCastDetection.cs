using UnityEngine;

public class RayCastDetection : MonoBehaviour
{
    public float raycastDistance = 0.5f;
    public bool isLooking = false;
    public bool isSpecial;

    FlashlightToggle ft;
    private void Start()
    {
        GameObject Player = GameObject.Find("Flashlight");
        ft = Player.GetComponent<FlashlightToggle>();
    }
    void Update()
    {
        // Cast a ray from the camera to check if it hits this object
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, raycastDistance))
        {
            // Check if the hit object is this GameObject
            if (hit.collider.gameObject == gameObject)
            {
                // Player is looking at this object
                isLooking = true;
                if (ft.specialIsOn) { isSpecial = true; }
                else isSpecial = false;
                //Debug.Log("Player is looking at " + gameObject.name);
            }
        }
        else
        {
            isLooking = false;
            isSpecial = false;
        }

    }
}
