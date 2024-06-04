using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Starting,
    Spawning,
    Racing
}

public class GameLoop : MonoBehaviour
{
    public Player player1;
    public Player player2;

    private GameState state;
    private float currentDelay;
    private List<Checkpoint> checkpoints = new List<Checkpoint>();
    private Checkpoint lastCheckpoint;

    public void Start()
    {
        state = GameState.Starting;

        Checkpoint[] checkpointArray = FindObjectsOfType<Checkpoint>(true);
        foreach (Checkpoint checkpoint in checkpointArray)
        {
            checkpoints.Add(checkpoint);
        }
        if (checkpoints.Count < 2)
        {
            Debug.LogError("Not enough checkpoints found");
        }
    }

    public void Update()
    {
        if (state == GameState.Starting)
        {
            while (lastCheckpoint == null || lastCheckpoint.IsActive())
            {
                lastCheckpoint = checkpoints[Random.Range(0, checkpoints.Count)];
            }

            state = GameState.Spawning;
            currentDelay = 1.0f;
            Debug.Log("Found start, spawning started");
        }
        else if (state == GameState.Spawning)
        {

            if (DelayFinished())
            {
                state = GameState.Racing;
                Debug.Log("Racing started");
            }
        }
        else if (state == GameState.Racing)
        {
            Player deadPlayer = null;
            if (!player1.CheckVisibility()) deadPlayer = player1;
            else if (!player2.CheckVisibility()) deadPlayer = player2;

            if (deadPlayer != null)
            {
                Debug.Log("Player " + deadPlayer.GetId() + " died");
                Respawn();
            }
        }
    }

    private void Respawn()
    {
        TeleportPlayersTo(lastCheckpoint.transform);
    }

    public void CheckpointReached(Checkpoint checkpoint, Player player)
    {
        Debug.Log("Player " + player.GetId() + " reached checkpoint");
        checkpoint.Deactivate();

        Checkpoint newCheckpoint = checkpoint;
        while (newCheckpoint == checkpoint)
        {
            newCheckpoint = checkpoints[Random.Range(0, checkpoints.Count)];
        }
        newCheckpoint.Activate();
        lastCheckpoint = checkpoint;
    }

    private bool DelayFinished()
    {
        currentDelay -= Time.deltaTime;
        if (currentDelay <= 0.0f)
        {
            currentDelay = 0.0f;
        }
        return currentDelay == 0.0f;
    }

    private void TeleportPlayersTo(Transform target)
    {
        // Offset the players slightly so they don't overlap
        Vector3 offset = new Vector3(0, 2, 0);
        Vector3 playerOffset = new Vector3(0, 0, 2);

        player1.transform.position = target.position + offset + playerOffset;
        player2.transform.position = target.position + offset - playerOffset;

        // Needed as otherwise the transform is not recognized due to an override by the character controller components
        Physics.SyncTransforms();

        player1.ClearTrail();
        player2.ClearTrail();
    }

    public GameState GetState()
    {
        return state;
    }
}
