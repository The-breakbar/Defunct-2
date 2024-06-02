using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controls : MonoBehaviour
{

    public KeyCode upKey = KeyCode.W;
    public KeyCode downKey = KeyCode.S;
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;
    public KeyCode jumpKey = KeyCode.Space;

    private float speed = 15.0f;
    private float jumpPower = 10.0f;
    private float gravity = 18.0f;

    // a character controller is a small wrapper for a rigid body
    // it implements useful features like slope movement, stepping, and collision detection
    private CharacterController controller;
    private float verticalVelocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Horizontal movement
        Vector3 move = Vector3.zero;
        if (Input.GetKey(upKey)) { move.x = 1; }
        if (Input.GetKey(downKey)) { move.x = -1; }
        if (Input.GetKey(leftKey)) { move.z = 1; }
        if (Input.GetKey(rightKey)) { move.z = -1; }

        controller.Move(move.normalized * Time.deltaTime * speed);
        if (move != Vector3.zero)
        {
            // Change the direction the player is facing
            gameObject.transform.forward = move;
        }

        // Vertical movement
        if (controller.isGrounded)
        {
            verticalVelocity = Input.GetKeyDown(jumpKey) ? jumpPower : 0;
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }

        controller.Move(new Vector3(0, verticalVelocity, 0) * Time.deltaTime);
    }
}
