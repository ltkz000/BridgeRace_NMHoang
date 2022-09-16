using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotSetupState : MonoBehaviour
{
    public NavMeshAgent agent;
    public BrickGenerator brickGenerator;
    
    [SerializeField] BrickController brickController;
    [SerializeField] public AIController aIController;
    [SerializeField] private Transform finishLine;
    [SerializeField] private BridgeWay bridgeWay;
    [SerializeField] private Animator animator;

    private int selectedColorCount;
    [SerializeField] private string selectedColorName;

    private void Start()
    {
        selectedColorCount = brickGenerator.GetSelectedColor(selectedColorName);
    }

    public void BotCollect()
    {
        Debug.Log("Collect");

        for(int i = brickGenerator.spawnedBricks.Length - 1; i >= 0; i--)
        {
            if(brickGenerator.spawnedBricks[i].isRemoved != true && brickGenerator.spawnedBricks[i].colorName == selectedColorName)
            {
                agent.SetDestination(brickGenerator.spawnedBricks[i].position);
                animator.SetBool(ConstValue.ANIM_RUNNING_BOOL, agent.velocity.magnitude > 0.01f);
            }
        }
        
        if(brickController.brickCount >= brickGenerator.GetSelectedColor(selectedColorName))
        {
            // StartCoroutine(waitBuild());   
            bridgeWay = LevelManager.Ins.ChooseBridge(); 
            aIController.switchState(BotState.Build);
        }
    }

    private IEnumerator waitBuild()
    {
        agent.SetDestination(bridgeWay.startPoint.position);
        yield return new WaitForSeconds(5f);
    }

    public void BotIdle()
    {
        Debug.Log("Idle");
        animator.SetTrigger(ConstValue.ANIM_IDLE_TRIGGER);   
        if(FrameManager.Ins.currentgameState == GameState.GamePlay)
        {
            aIController.switchState(BotState.Collect);
        }
    }

    public void BotBuild()
    {
        Debug.Log("Build");
        agent.SetDestination(bridgeWay.finishPoint.position);

        if(brickController.brickCount == 0)
        {
            // StartCoroutine(waitBuild());
            aIController.switchState(BotState.Collect);
        }
    }

    public void BotWin()
    {
        agent.SetDestination(finishLine.position);
    }
}
