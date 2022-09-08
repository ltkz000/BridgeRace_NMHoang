using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BotState{
    Idle,
    Collect,
    Build,
    Win,
    Lose
}

public class AIController : MonoBehaviour
{   
    [SerializeField] private BrickGenerator brickGenerator;
    [SerializeField] BotBrickControll botBrickControll;
    [SerializeField] private CapsuleCollider collider;
    [SerializeField] private Animator animator;

    private bool isWin;
    private Stage currentStage;
    private BotState _currentState;
    LevelManager ins;

    private void Start() 
    {
        isWin = false;

        _currentState = BotState.Idle;
        ins = LevelManager.Ins;
    }
    // Update is called once per frame
    void Update()
    {
        switch(_currentState)
        {
            case BotState.Idle:
                botBrickControll.BotIdle();
                break;
            case BotState.Collect:
                botBrickControll.BotCollect();
                break;
            case BotState.Build:
                botBrickControll.BotBuild();
                break;
            case BotState.Win:
                botBrickControll.BotWin();
                break;
            case BotState.Lose:
                break;
        }

        if(FrameManager.Ins.currentgameState == GameState.Result && isWin == false)
        {
            _currentState = BotState.Idle;
        }
    }

    private void OnCollisionEnter(Collision hit) 
    {
        Brick brick = hit.collider.GetComponent<Brick>();

        if(hit.collider.CompareTag("PlacedBrick"))
        {
            if(botBrickControll.brickCount > 0)
            {
                hit.collider.isTrigger = true;
            }
        }    
        else if(hit.collider.CompareTag("normal"))
        {
            botBrickControll.PickedUp(hit.gameObject);
        }
        else if(hit.collider.CompareTag("Player"))
        {
            BotBrickControll _botBrickController = hit.collider.GetComponent<BotBrickControll>();
            PlayerTestBrick _playerBrickController = hit.collider.GetComponent<PlayerTestBrick>();

            if(_botBrickController != null)
            {
                HitFall(_botBrickController);
            }
            else if(_playerBrickController != null)
            {
                HitFall(_playerBrickController);
            }
        }
    }

    private void HitFall(BotBrickControll _botBrickController)
    {
        int temp = botBrickControll.brickCount - _botBrickController.brickCount;

        Debug.Log("Temp: " + temp);

        if(temp == 0)
        {
            return;
        }
        else if(temp < 0)
        {
            TriggerFall();
            botBrickControll.DropBrick();
        }
    }

    private void HitFall(PlayerTestBrick _playerBrickController)
    {
        int temp = botBrickControll.brickCount - _playerBrickController.brickCount;

        Debug.Log("Temp: " + temp);

        if(temp == 0)
        {
            return;
        }
        else if(temp < 0)
        {
            TriggerFall();
            botBrickControll.DropBrick();
        }
    }

    protected virtual void TriggerFall()
    {
        StartCoroutine(Fall());
    }

    private IEnumerator Fall()
    {
        collider.enabled = false;
        // botBrickControll.agent.enabled = false;

        animator.SetTrigger("Fall");

        yield return new WaitForSeconds(4.9f);

        collider.enabled = true;
        // botBrickControll.agent.enabled = true;
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Stage1"))
        {
            currentStage = ins.switchStage(1);
            brickGenerator = ins.ChooseSpawner(currentStage);
            botBrickControll.brickGenerator = brickGenerator;
            botBrickControll.spawnerPooling = botBrickControll.brickGenerator.GetComponent<Pooling>();
            switchState(BotState.Collect);
        }
        else if(other.CompareTag("Stage2"))
        {
            currentStage = ins.switchStage(2);
            brickGenerator = ins.ChooseSpawner(currentStage);
            botBrickControll.brickGenerator = brickGenerator;
            botBrickControll.spawnerPooling = botBrickControll.brickGenerator.GetComponent<Pooling>();
            switchState(BotState.Collect);
        }
        else if(other.CompareTag("Stage3"))
        {
            currentStage = ins.switchStage(3);
            brickGenerator = ins.ChooseSpawner(currentStage);
            botBrickControll.brickGenerator = brickGenerator;
            botBrickControll.spawnerPooling = botBrickControll.brickGenerator.GetComponent<Pooling>();
            switchState(BotState.Collect);
        }
        else if(other.CompareTag("LastStage"))
        {
            switchState(BotState.Win);
        }
        else if(other.CompareTag("Finish"))
        {
            botBrickControll.DropAllBrick();
            animator.SetTrigger("Win");
            FrameManager.Ins.ChangeState(GameState.Result);
            UIManager.Ins.OpenUI<CanvasVictory>(UICanvasID.Result).SetResult(false);
            isWin = true;
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
