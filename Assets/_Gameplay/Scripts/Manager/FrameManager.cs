using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { MainMenu, GamePlay, Pause, Result}

public class FrameManager : Singleton<FrameManager>
{
    public GameState currentgameState = GameState.MainMenu;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        Input.multiTouchEnabled = true;

        currentgameState = GameState.MainMenu;
    }

    public void ChangeState(GameState gameState)
    {
        currentgameState = gameState;
    }

    public bool IsState(GameState gameState)
    {
        return this.currentgameState == gameState;
    }
}
