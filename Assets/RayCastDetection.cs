using UnityEngine;

public class RayCastDetection : MonoBehaviour
{
    public float raycastDistance = 0.5f;
    public bool isLooking = false;
    public bool isSpecial;

    private FlashlightToggle ft;
    private SC_NPCFollow enemyFollowScript;
    private bool hasTriggeredPause = false;

    private void Start()
    {

        // Assign the SC_NPCFollow script
        enemyFollowScript = GetComponent<SC_NPCFollow>();
    }

    void Update()
    {
        GameObject Player = GameObject.Find("Flashlight");
        if(Player != null)
        {
            ft = Player.GetComponent<FlashlightToggle>();
        }
        // Cast a ray from the camera to check if it hits this object
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, raycastDistance))
        {
            // Check if the hit object is this GameObject
            if (hit.collider.gameObject == gameObject)
            {
                // Player is looking at this object
                isLooking = true;
                isSpecial = ft.specialIsOn;

                // Trigger the pause and flee behavior if the special flashlight mode is on for the first time
                if (isSpecial && !hasTriggeredPause)
                {
                    hasTriggeredPause = true;
                    if (enemyFollowScript != null)
                    {
                      enemyFollowScript.SpecialFlashlightHit();
                    }
                }
            }
        }
        else
        {
            isLooking = false;
            isSpecial = false;
            hasTriggeredPause = false;
        }
    }
}
