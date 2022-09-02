using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BotState{
    Idle,
    Collect,
    Build,
    Fall, 
    Win
}

public class AIController : MonoBehaviour
{   
    [SerializeField] private BrickGenerator brickGenerator;
    [SerializeField] BotBrickControll botBrickControll;

    
    private Stage currentStage;
    private BotState _currentState;
    LevelManager ins;

    private void Start() 
    {
        _currentState = BotState.Collect;
        ins = LevelManager.Ins;
    }
    // Update is called once per frame
    void Update()
    {
        switch(_currentState)
        {
            case BotState.Idle:
                break;
            case BotState.Collect:
                botBrickControll.BotCollect();
                break;
            case BotState.Build:
                Debug.Log("Build bridge");
                botBrickControll.BotBuild();
                break;
            case BotState.Fall:
                break;
            case BotState.Win:
                break;
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Stage1"))
        {
            currentStage = ins.switchStage(1);
            brickGenerator = ins.ChooseSpawner(currentStage);
            botBrickControll.spawnerPooling = botBrickControll.brickGenerator.GetComponent<Pooling>();
        }
        else if(other.CompareTag("Stage2"))
        {
            currentStage = ins.switchStage(2);
            brickGenerator = ins.ChooseSpawner(currentStage);
            botBrickControll.spawnerPooling = botBrickControll.brickGenerator.GetComponent<Pooling>();
        }
        else if(other.CompareTag("Stage3"))
        {
            currentStage = ins.switchStage(3);
            brickGenerator = ins.ChooseSpawner(currentStage);
            botBrickControll.spawnerPooling = botBrickControll.brickGenerator.GetComponent<Pooling>();
        }
        else if(other.CompareTag("PlacedBrick"))
        {
            botBrickControll.PlaceBrick(other.gameObject);
        }
        else if(other.CompareTag("normal"))
        {
            botBrickControll.PickedUp(other.gameObject);
        }
        else
        {
            botBrickControll.ReplaceBrick(other.gameObject);
        }
    }

    public void switchState(BotState state)
    {
        _currentState = state;
    }
}
