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
    [SerializeField] private BrickGenerator spawner1;
    [SerializeField] private BrickGenerator spawner2;
    [SerializeField] private BrickGenerator spawner3;
    public BridgeWay[] bridgeWays1;
    public BridgeWay[] bridgeWays2;
    public BridgeWay[] bridgeWays3;
    private BridgeWay[] currentBridgeWays;

    private int randomNum;

    private void Start() 
    {
        currentBridgeWays = bridgeWays1;
    }

    public Stage switchStage(string stage)
    {
        switch(stage)
        {
            case ConstValue.STAGE1_TAG:
                currentBridgeWays = bridgeWays1;
                return Stage.Stage1;
            case ConstValue.STAGE2_TAG:
                currentBridgeWays = bridgeWays2;
                return Stage.Stage2;
            case ConstValue.STAGE3_TAG:
                currentBridgeWays = bridgeWays3;
                return Stage.Stage3;
            default:
                currentBridgeWays = bridgeWays1;
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

    public BridgeWay ChooseBridge()
    {
        randomNum = Random.Range(0, currentBridgeWays.Length);
    
        return currentBridgeWays[randomNum];
    }
}
