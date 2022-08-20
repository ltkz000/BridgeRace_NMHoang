using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerStage { Stage1, Stage2, Stage3 }

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] BrickGenerator spawner1;
    [SerializeField] BrickGenerator spawner2;
    [SerializeField] BrickGenerator spawner3;
    public void UpdatePlayerStage(PlayerStage playerStage)
    {
        switch(playerStage)
        {
            case PlayerStage.Stage1:
                break;
            case PlayerStage.Stage2:
                break;
            case PlayerStage.Stage3:
                break;
            default:
                break;
        }
    }
}
