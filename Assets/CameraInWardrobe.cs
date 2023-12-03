using Unity.VisualScripting;
using UnityEngine;

public class CameraInWardrobe : MonoBehaviour
{

    cameraController cameraController;
    Camera cam;
    GameObject player;
    PlayerMove pm;
    PlayerLook pl;
    private void Start()
    {
        player = GameObject.Find("Player");
        cam = Camera.main;
        pm = player.GetComponent<PlayerMove>();
        pl = GameObject.Find("Camera").GetComponent<PlayerLook>();
    }
    private void OnTriggerStay(Collider other)
    {
        // Check if the colliding object is the player
        if (other.CompareTag("Player"))
        {
            cameraController = GameObject.Find("Camera").GetComponent<cameraController>();
            if (cameraController != null)
            {
                cameraController.activate = true;
                //pm.enabled = !pm.enabled;
                //pl.enabled = !pl.enabled;
            }
            else
            {
                Debug.LogError("cameraController component not found on the Player GameObject.");
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        // Check if the colliding object is the player
        if (other.CompareTag("Player"))
        {
            // Log a message when the player exits the trigger
            Debug.Log("Player exited the trigger!");
        }
    }
}
