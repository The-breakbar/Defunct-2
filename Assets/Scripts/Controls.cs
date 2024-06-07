using System;
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
    public KeyCode boostKey = KeyCode.LeftShift;
    public Transform modelTransform;

    [Range(0.0f, 100.0f)]
    public float speed = 15.0f;
    [Range(0.0f, 1.0f)]
    public float speedIncreaseAfterSlowdown = 0.5f;

    [Range(0.0f, 50.0f)]
    public float maxSpeed = 15.0f;
    public float boost = 3.0f;

    [Range(0.0f, 5.0f)]
    public int maxBoost = 3;

    [Range(0.0f, 100.0f)]
    public float maxBoostSpeed = 30.0f;
    [Range(0.0f, 3.0f)]
    public float boostSpeedChange = 1.0f;

    [Range(0.0f, 50.0f)]
    public float jumpPower = 10.0f;

    [Range(0.0f, 50.0f)]
    public float gravity = 18.0f;

    // a character controller is a small wrapper for a rigid body
    // it implements useful features like slope movement, stepping, and collision detection
    private CharacterController controller;
    private Vector3 velocity;

    public void DoSlowDown(float speedAfterSlowdown, float speedIncreaseAfterSlowdown)
    {
        speed = speedAfterSlowdown;
        this.speedIncreaseAfterSlowdown = speedIncreaseAfterSlowdown;
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        // Acceleration after slowdown
        if (speed < maxSpeed) speed += speedIncreaseAfterSlowdown;

        // Boosting
        if (Input.GetKey(boostKey) && boost > 0)
        {
            speed += boostSpeedChange;
            boost -= Time.deltaTime;
            boost = Mathf.Max(boost, 0);
        }
        else if (speed > maxSpeed) speed -= boostSpeedChange;

        // Cap speed
        speed = Input.GetKey(boostKey) ? Mathf.Min(speed, maxBoostSpeed) : Mathf.Min(speed, maxSpeed);
    }

    public void Update()
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
            modelTransform.forward = move;
        }

        // Change the angle the player is facing
        if (controller.isGrounded)
        {
            // reset vertical rotation
            modelTransform.eulerAngles = new Vector3(0, modelTransform.eulerAngles.y, 0);
        }
        else
        {
            // rotate player 45 degrees upwards
            modelTransform.eulerAngles = new Vector3(velocity.y < 0 ? 45 : -35, modelTransform.eulerAngles.y, 0);
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

    public float GetBoostPercentage()
    {
        return boost / maxBoost;
    }

    public bool IsBoosting()
    {
        return Input.GetKey(boostKey) && boost > 0;
    }

    public void AddBoost(float amount)
    {
        boost += amount;
        boost = Mathf.Min(boost, maxBoost);
    }
}
