using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool isGrounded;
    public float gravity = -9.8f;
    public float speed = 5f;
    public float jumpHeight = 1f;
    bool crouching = false;
    float crouchTimer = 1;
    bool lerpCrouch = false;
    bool sprinting = false;

    public float Gravity { get => gravity; set => gravity = value; }

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame

    void Update()
    {
        isGrounded = controller.isGrounded;
        if (lerpCrouch)
        {
            crouchTimer += Time.deltaTime;
            float p = crouchTimer / 1;
            p *= p;
            {

                if (crouching)
                    controller.height = Mathf.Lerp(controller.height, 1, p);
                else
                    controller.height = Mathf.Lerp(controller.height, 2, p);
                if (p > 1)
                {
                    lerpCrouch = false;
                    crouchTimer = 0f;
                }
            }
        }
    }

    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);
        playerVelocity.y += Gravity * Time.deltaTime;
        if (isGrounded && playerVelocity.y < 0)
            playerVelocity.y = -2f;
        controller.Move(playerVelocity * Time.deltaTime);
    }


    public void Jump()
    {
    }

    public void Crouch()
    {
        crouching = !crouching;
        if (crouching)
        {
            crouchTimer = 0;
            lerpCrouch = true;
            speed = 2f;
        }
        else {
            crouchTimer = 0;
            speed = 5;
            lerpCrouch = true;
        }

    }
    public void Sprint()
    {
        sprinting = !sprinting;
        if (sprinting)
            speed = 8;
        else
            speed = 5;
    }
}
