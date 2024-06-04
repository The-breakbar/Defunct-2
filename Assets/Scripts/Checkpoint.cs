using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    public bool startCheckpoint = false;

    private bool active = false;
    private GameLoop gameLoop;
    private PlayerCamera playerCamera;

    public void Start()
    {
        gameLoop = FindObjectOfType<GameLoop>();
        playerCamera = FindObjectOfType<PlayerCamera>();

        if (startCheckpoint)
        {
            Activate();
        }
        else
        {
            Deactivate();
        }
    }

    public void Activate()
    {
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
            gameLoop.CheckpointReached(this, other.gameObject.GetComponent<Player>());
        }
    }
}
