using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideController : MonoBehaviour
{
    [SerializeField] protected BrickController brickController; 
    protected Stage currentStage;
    protected LevelManager ins;

    private void Start() 
    {
        currentStage = Stage.Stage1;
        ins = LevelManager.Ins;    
    }

    private void Update() 
    {
        Debug.Log(currentStage.ToString());
    }

    private void OnControllerColliderHit(ControllerColliderHit hit) 
    {
        Brick brick = hit.collider.GetComponent<Brick>();

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

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag(ConstValue.STAGE1_TAG) || other.CompareTag(ConstValue.STAGE2_TAG) || other.CompareTag(ConstValue.STAGE3_TAG))
        {
            UpdateStage(other.tag);
        }
        else if(other.CompareTag(ConstValue.PLACEDBRICK_TAG))
        {
            brickController.PlaceBrick(other.gameObject);
        }
        else if(other.CompareTag(ConstValue.NORMALBRICK_TAG))
        {
            brickController.PickedUp(other.gameObject);
        }
    }

    protected virtual void UpdateStage(string stage)
    {
        brickController.brickGenerator.GeneratedRemovedBrick();
        currentStage = ins.switchStage(stage);
        brickController.brickGenerator = ins.ChooseSpawner(currentStage);
        brickController.spawnerPooling = brickController.brickGenerator.GetComponent<Pooling>();
    }
}
