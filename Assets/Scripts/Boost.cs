using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boost : MonoBehaviour
{
    [Range(0.0f, 2.0f)]
    public float boostAmount = 0.5f;
    public Transform model;

    [Range(0.0f, 60.0f)]
    public float respawnTime = 10.0f;

    [Range(0.0f, 2.0f)]
    public float baseSize = 1.0f;

    [Range(0.0f, 10.0f)]
    public float pulseSpeed = 2.0f;

    [Range(0.0f, 1.0f)]
    public float pulseAmount = 0.1f;

    private bool pickedUp = false;
    private float timeLeft = 0.0f;


    private void FixedUpdate()
    {
        if (pickedUp)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0.0f)
            {
                model.gameObject.SetActive(true);
                pickedUp = false;
            }
        }
        else
        {
            model.localScale = Vector3.one * (baseSize + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!pickedUp && other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Controls>().AddBoost(boostAmount);

            model.gameObject.SetActive(false);
            pickedUp = true;
            timeLeft = respawnTime;
        }
    }
}
