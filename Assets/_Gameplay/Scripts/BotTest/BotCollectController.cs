using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotCollectController : CollideController
{
    [SerializeField] private BotSetupState botSetupState;

    private void OnCollisionEnter(Collision hit) 
    {
        if(hit.collider.CompareTag(ConstValue.PLACEDBRICK_TAG))
        {
            if(brickController.brickCount > 0)
            {
                hit.collider.isTrigger = true;
            }
        }       
        else if(hit.collider.CompareTag(ConstValue.NORMALBRICK_TAG))
        {
            brickController.PickedUp(hit.gameObject);
        }
    }

    protected override void UpdateStage(string stage)
    {
        base.UpdateStage(stage);
        botSetupState.brickGenerator = ins.ChooseSpawner(currentStage);
        botSetupState.aIController.switchState(BotState.Collect);
    }
}
