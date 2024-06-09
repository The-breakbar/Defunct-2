using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Range(0.0f, 3.0f)]
    public float boostBonus;
    private bool active = false;
    private GameLoop gameLoop;
    private PlayerCamera playerCamera;

    public void Start()
    {
        gameLoop = FindObjectOfType<GameLoop>();
        playerCamera = FindObjectOfType<PlayerCamera>();
        Deactivate();
    }

    public void Activate()
    {
        if (playerCamera == null) playerCamera = FindObjectOfType<PlayerCamera>();

        active = true;
        gameObject.SetActive(true);
        playerCamera.SetGoal(transform.position);
    }

    public void Deactivate()
    {
        active = false;
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (active && other.gameObject.CompareTag("Player"))
        {
            Controls player = other.gameObject.GetComponent<Controls>();
            player.AddBoost(1.0f);
            gameLoop.CheckpointReached(this);
        }
    }

    public bool IsActive()
    {
        return active;
    }
}
