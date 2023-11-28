using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using XEntity.InventoryItemSystem;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private string mouseXInputName, mouseYInputName;
    [SerializeField] public float mouseSensitivity;

    [SerializeField] private Transform playerBody;

    public bool lolwtf;

    private float xAxisClamp;

    ItemContainer itemContainer;
    GameObject playerInventory;

 

    private void Awake()
    {
        playerInventory = GameObject.Find("PlayerInventory");
        itemContainer = playerInventory.GetComponent<ItemContainer>();
        if (itemContainer.isOpen )
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else if ( !itemContainer.isOpen )
        {
            LockCursor();
            xAxisClamp = 0.0f;
        }
    }


    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if(itemContainer.isOpen)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            CameraRotation();
            itemContainer = playerInventory.GetComponent<ItemContainer>();
        } 
    }

    public void CameraRotation()
    {
         float mouseX = Input.GetAxis(mouseXInputName) * mouseSensitivity * Time.deltaTime;
         float mouseY = Input.GetAxis(mouseYInputName) * mouseSensitivity * Time.deltaTime;

        xAxisClamp += mouseY;

        if(xAxisClamp > 90.0f)
        {
            xAxisClamp = 90.0f;
            mouseY = 0.0f;
            ClampXAxisRotationToValue(270.0f);
        }
        else if (xAxisClamp < -90.0f)
        {
            xAxisClamp = -90.0f;
            mouseY = 0.0f;
            ClampXAxisRotationToValue(90.0f);
        }

        transform.Rotate(Vector3.left * mouseY);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    private void ClampXAxisRotationToValue(float value)
    {
        Vector3 eulerRotation = transform.eulerAngles;
        eulerRotation.x = value;
        transform.eulerAngles = eulerRotation;
    }
}
