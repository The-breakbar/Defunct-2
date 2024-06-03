using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform target1;
    public Transform target2;
    public float interpolation = 0.5f;
    public Vector3 offset;

    void Start()
    {

    }

    void Update()
    {
        Vector3 targetPosition = target1.position * (1 - interpolation) + target2.position * interpolation;
        Camera.main.transform.position = targetPosition + offset;
    }
}
