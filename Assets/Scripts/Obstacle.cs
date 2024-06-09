using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public GameObject obstacleModel;
    public float speedAfterSlowdown = 0.0f;
    public float speedIncreaseAfterSlowdown = 0.05f;

    [Range(0.0f, 60.0f)] public float respawnTime = 10.0f;
    private bool destroyed = false;
    private float timeLeft = 0.0f;

    public ParticleSystem destructionParticles;

    private void Start()
    {
        destructionParticles.Pause(true);
    }

    private void FixedUpdate()
    {
        if (destroyed)
        {
            timeLeft -= Time.fixedDeltaTime;
            if (timeLeft <= 0.0f)
            {
                obstacleModel.SetActive(true);
                destroyed = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!destroyed && other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Controls>().DoSlowDown(speedAfterSlowdown, speedIncreaseAfterSlowdown);

            // TODO: Add a sound effect
            // Add a particle effect
            destructionParticles.Play(true);

            obstacleModel.SetActive(false);
            destroyed = true;
            timeLeft = respawnTime;
        }
    }
}