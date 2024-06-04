using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Starting,
    Spawning,
    Racing,
    Winning
}

public class GameLoop : MonoBehaviour
{
    public Player player1;
    public Player player2;

    private readonly int pointsToWin = 2;
    private GameState state;
    private float currentDelay;
    private List<Checkpoint> checkpoints = new List<Checkpoint>();
    private Checkpoint lastCheckpoint;

    public void Start()
    {
        // Grab all checkpoints in the scene
        Checkpoint[] checkpointArray = FindObjectsOfType<Checkpoint>(true);
        foreach (Checkpoint checkpoint in checkpointArray)
        {
            checkpoints.Add(checkpoint);
        }
        if (checkpoints.Count < 2)
        {
            Debug.LogError("Not enough checkpoints found");
            return;
        }

        // Select random starting checkpoint
        CheckpointReached(checkpoints[Random.Range(0, checkpoints.Count)]);
        Respawn();
    }

    public void Update()
    {
        if (state == GameState.Spawning)
        {
            if (DelayFinished())
            {
                state = GameState.Racing;
                Debug.Log("Racing started");
            }
        }
        else if (state == GameState.Racing)
        {
            // Check if a player is outside the window
            Player deadPlayer = null;
            if (!player1.CheckVisibility()) deadPlayer = player1;
            else if (!player2.CheckVisibility()) deadPlayer = player2;

            if (deadPlayer != null)
            {
                Debug.Log("Player " + deadPlayer.GetId() + " died");
                Player notDeadPlayer = deadPlayer == player1 ? player2 : player1;
                notDeadPlayer.AddPoint();

                if (notDeadPlayer.GetPoints() >= pointsToWin)
                {
                    state = GameState.Winning;
                    Debug.Log("Player " + notDeadPlayer.GetId() + " won");
                }
                else
                {
                    Respawn();
                }
            }
        }
    }

    // Teleport players to the last checkpoint
    private void Respawn()
    {
        state = GameState.Spawning;
        currentDelay = 1.0f;
        TeleportPlayersTo(lastCheckpoint.transform);
    }

    public void CheckpointReached(Checkpoint checkpoint)
    {
        checkpoint.Deactivate();

        Checkpoint newCheckpoint = checkpoint;
        while (newCheckpoint == checkpoint)
        {
            newCheckpoint = checkpoints[Random.Range(0, checkpoints.Count)];
        }
        newCheckpoint.Activate();
        lastCheckpoint = checkpoint;
    }

    // Utility to wait for a delay
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

        // Clear the trails which are left behind from the teleportation
        player1.ClearTrail();
        player2.ClearTrail();
    }

    public GameState GetState()
    {
        return state;
    }
}
