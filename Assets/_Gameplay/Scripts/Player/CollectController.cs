using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectController : MonoBehaviour
{
    [SerializeField] private PlayerTestBrick playerBrickController; 
    private Stage currentStage;
    private LevelManager ins;

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

        if(hit.collider.CompareTag("PlacedBrick"))
        {
            if(playerBrickController.brickCount > 0)
            {
                hit.collider.isTrigger = true;
            }
        }    
        else if(hit.collider.CompareTag("normal"))
        {
            playerBrickController.PickedUp(hit.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Stage1"))
        {
            playerBrickController.brickGenerator.GeneratedRemovedBrick();
            currentStage = ins.switchStage(1);
            playerBrickController.brickGenerator = ins.ChooseSpawner(currentStage);
            playerBrickController.spawnerPooling = playerBrickController.brickGenerator.GetComponent<Pooling>();
        }
        else if(other.CompareTag("Stage2"))
        {
            playerBrickController.brickGenerator.GeneratedRemovedBrick();
            currentStage = ins.switchStage(2);
            playerBrickController.brickGenerator = ins.ChooseSpawner(currentStage);
            playerBrickController.spawnerPooling = playerBrickController.brickGenerator.GetComponent<Pooling>();
        }
        else if(other.CompareTag("Stage3"))
        {
            playerBrickController.brickGenerator.GeneratedRemovedBrick();
            currentStage = ins.switchStage(3);
            playerBrickController.brickGenerator = ins.ChooseSpawner(currentStage);
            playerBrickController.spawnerPooling = playerBrickController.brickGenerator.GetComponent<Pooling>();
        }
        else if(other.CompareTag("PlacedBrick"))
        {
            playerBrickController.PlaceBrick(other.gameObject);
        }
        else if(other.CompareTag("normal"))
        {
            playerBrickController.PickedUp(other.gameObject);
        }
        else{
            playerBrickController.ReplaceBrick(other.gameObject);
        }
    }
}
