using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Stage
{
    Stage1,
    Stage2,
    Stage3
}

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] BrickGenerator spawner1;
    [SerializeField] BrickGenerator spawner2;
    [SerializeField] BrickGenerator spawner3;

    public Stage switchStage(int stage)
    {
        switch(stage)
        {
            case 1:
                return Stage.Stage1;
            case 2:
                return Stage.Stage2;
            case 3:
                return Stage.Stage3;
            default:
                return Stage.Stage1;      
        }
    }

    public BrickGenerator ChooseSpawner(Stage currentStage)
    {
        if(currentStage == Stage.Stage1)
        {
            return spawner1;
        }
        else if(currentStage == Stage.Stage2)
        {
            return spawner2;
        }
        else
        {
            return spawner3;
        }
    }
}
