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
    [SerializeField] BrickController brickController;
    [SerializeField] BotSetupState botSetupState;
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
        if(FrameManager.Ins.currentgameState == GameState.Result && isWin == false)
        {
            _currentState = BotState.Idle;
        }
        else
        {
            switch(_currentState)
            {
            case BotState.Idle:
                botSetupState.BotIdle();
                break;
            case BotState.Collect:
                botSetupState.BotCollect();
                break;
            case BotState.Build:
                botSetupState.BotBuild();
                break;
            case BotState.Win:
                botSetupState.BotWin();
                break;
            case BotState.Lose:
                break;
            }
        }
    }

    private void OnCollisionEnter(Collision hit) 
    {
        if(hit.collider.CompareTag(ConstValue.PLAYER_TAG))
        {
            BrickController brickController = hit.collider.GetComponent<BrickController>();

            if(brickController != null)
            {
                HitFall(brickController);
            }
        }
    }

    private void HitFall(BrickController _brickController)
    {
        int temp = brickController.brickCount - _brickController.brickCount;

        if(temp == 0)
        {
            return;
        }
        else if(temp < 0)
        {
            TriggerFall();
            brickController.DropBrick();
        }
    }

    protected virtual void TriggerFall()
    {
        StartCoroutine(Fall());
    }

    private IEnumerator Fall()
    {
        collider.enabled = false;

        animator.SetTrigger(ConstValue.ANIM_FALL_TRIGGER);

        yield return new WaitForSeconds(4.9f);

        collider.enabled = true;
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag(ConstValue.LASTSTAGE_TAG))
        {
            switchState(BotState.Win);
        }
        else if(other.CompareTag(ConstValue.FINISH_TAG))
        {
            brickController.DropAllBrick();
            animator.SetTrigger(ConstValue.WIN_TAG);
            FrameManager.Ins.ChangeState(GameState.Result);
            UIManager.Ins.OpenUI<CanvasVictory>(UICanvasID.Result).SetResult(false);
            isWin = true;
        }
    }

    public void switchState(BotState state)
    {
        _currentState = state;
    }
}
