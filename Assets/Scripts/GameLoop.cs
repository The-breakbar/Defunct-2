using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Racing
}

public class GameLoop : MonoBehaviour
{
    public Player player1;
    public Player player2;

    private GameState state = GameState.Racing;
    private List<Checkpoint> checkpoints = new List<Checkpoint>();

    public void Start()
    {
        Checkpoint[] checkpointArray = FindObjectsOfType<Checkpoint>(true);
        foreach (Checkpoint checkpoint in checkpointArray)
        {
            checkpoints.Add(checkpoint);
        }

        Debug.Log("Initialized " + checkpoints.Count + " checkpoints");
    }

    public void CheckpointReached(Checkpoint checkpoint, Player player)
    {
        Debug.Log("Player " + player.GetId() + " reached checkpoint");
        player.AddPoint();
        checkpoint.Deactivate();

        // Select new random checkpoint (that can't be the same as the current one)
        if (checkpoints.Count < 2)
        {
            Debug.Log("Not enough checkpoints to select a new one");
            return;
        }

        Checkpoint newCheckpoint = checkpoint;
        while (newCheckpoint == checkpoint)
        {
            newCheckpoint = checkpoints[Random.Range(0, checkpoints.Count)];
        }
        newCheckpoint.Activate();

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
    }

    public GameState GetState()
    {
        return state;
    }
}
