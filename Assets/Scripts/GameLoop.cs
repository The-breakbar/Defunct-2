using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// define the possible game states
public enum GameState
{
    Racing
}

public class GameLoop : MonoBehaviour
{

    private GameState state = GameState.Racing;

    void Update()
    {

    }

    public GameState GetState()
    {
        return state;
    }
}
