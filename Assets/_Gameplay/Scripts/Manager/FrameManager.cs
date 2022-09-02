using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { MainMenu, GamePlay, Pause}

public class FrameManager : MonoBehaviour
{
    private GameState gameState;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        Input.multiTouchEnabled = true;

        //init data
    }

    public void ChangeState(GameState gameState)
    {
        this.gameState = gameState;
    }

    public bool IsState(GameState gameState)
    {
        return this.gameState == gameState;
    }
}
