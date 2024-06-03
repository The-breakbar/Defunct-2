using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
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
    private Vector3 velocity;
    private GameLoop gameLoop;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        gameLoop = GameObject.Find("gameLoop").GetComponent<GameLoop>();
    }

    void Update()
    {
        velocity = new Vector3(0, velocity.y, 0);

        // Horizontal movement
        Vector3 move = Vector3.zero;
        if (Input.GetKey(upKey)) { move.x = 1; }
        if (Input.GetKey(downKey)) { move.x = -1; }
        if (Input.GetKey(leftKey)) { move.z = 1; }
        if (Input.GetKey(rightKey)) { move.z = -1; }
        velocity += move.normalized * speed;

        // Change the direction the player is facing
        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        // Vertical movement
        if (controller.isGrounded)
        {
            velocity.y = Input.GetKeyDown(jumpKey) ? jumpPower : 0;
        }

        // Apply gravity (should be done every frame for consistent isGrounded behavior)
        velocity.y -= gravity * Time.deltaTime;

        // Move the player
        controller.Move(velocity * Time.deltaTime);
    }
}
