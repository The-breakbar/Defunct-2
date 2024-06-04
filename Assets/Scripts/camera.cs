using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraState
{
    Tracking,
    Lerping
}

public class PlayerCamera : MonoBehaviour
{
    public Transform player1;
    public Transform player2;
    public float interpolation = 0.5f;
    public Vector3 offset;

    private Vector3 currentGoal;
    private bool player1WasCloser = false;
    private CameraState state = CameraState.Tracking;
    public float camChangeSpeed = 1.0f;
    private float remainingTime = 0.0f;
    private Vector3 lastChangePosition;

    void Start()
    {
        currentGoal = transform.position;
    }

    void Update()
    {
        bool player1Closer = Vector3.Distance(currentGoal, player1.position) < Vector3.Distance(currentGoal, player2.position);
        Vector3 interpLow = player1Closer ? player2.position : player1.position;
        Vector3 interpHigh = player1Closer ? player1.position : player2.position;
        Vector3 targetPosition = Vector3.Lerp(interpLow, interpHigh, interpolation);
        targetPosition += offset;

        if (player1Closer != player1WasCloser)
        {
            Debug.Log("Player " + (player1Closer ? "1" : "2") + " is closer to the goal");
            player1WasCloser = player1Closer;

            state = CameraState.Lerping;
            remainingTime = camChangeSpeed;
            lastChangePosition = transform.position;
        }

        if (state == CameraState.Lerping)
        {
            remainingTime -= Time.deltaTime;
            if (remainingTime <= 0.0f)
            {
                state = CameraState.Tracking;
            }
            else
            {
                float t = 1.0f - (remainingTime / camChangeSpeed);
                transform.position = Vector3.Lerp(lastChangePosition, targetPosition, t);
            }
        }
        else
        {
            transform.position = targetPosition;
        }
    }

    public void SetGoal(Vector3 goal)
    {
        currentGoal = goal;
    }
}
