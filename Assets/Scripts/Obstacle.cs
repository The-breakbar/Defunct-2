using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float speedAfterSlowdown = 0.0f;
    public float speedIncreaseAfterSlowdown = 0.05f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Controls>().DoSlowDown(speedAfterSlowdown, speedIncreaseAfterSlowdown);
            // TODO: Add a sound effect
            // TODO: Add a particle effect
            gameObject.SetActive(false);
        }
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }
}