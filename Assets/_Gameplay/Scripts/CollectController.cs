using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectController : MonoBehaviour
{
    [SerializeField] private PlayerBrickController playerBrickController; 
    [SerializeField] private Pooling spawnerPooling;
    [SerializeField] private BrickGenerator brickGenerator;
    Stage currentStage;
    LevelManager ins;
    public string playerColorName;

    private void Start() 
    {
        currentStage = Stage.Stage1;
        ins = LevelManager.Ins;    
    }

    private void Update() 
    {
        Debug.Log(currentStage.ToString());
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Stage1"))
        {
            currentStage = ins.switchStage(1);
            brickGenerator = ins.ChooseSpawner(currentStage);
            // brickGenerator.GeneratedRemovedBrick();
        }
        else if(other.CompareTag("Stage2"))
        {
            currentStage = ins.switchStage(2);
            brickGenerator = ins.ChooseSpawner(currentStage);
            // brickGenerator.GeneratedRemovedBrick();
        }
        else if(other.CompareTag("Stage3"))
        {
            currentStage = ins.switchStage(3);
            brickGenerator = ins.ChooseSpawner(currentStage);
            // brickGenerator.GeneratedRemovedBrick();
        }
        else
        {
            Brick brick = other.transform.GetComponent<Brick>();
        
            if(brick.colorName == playerColorName)
            {
                brickGenerator.RemovePickedBrick(brick.brickNumber);
                spawnerPooling.ReturnObject(other.gameObject);
                playerBrickController.UpdateBrickHolder();
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        
    }
}
