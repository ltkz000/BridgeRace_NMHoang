using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotController : MonoBehaviour
{
    private BrickGenerator brickGenerator;
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private PlayerBrickController brickController;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform finalPoint;
    private int selectedColorCount;
    private Vector3 finalPos;


    private void Start() 
    {
        brickGenerator = FindObjectOfType<BrickGenerator>();

        StartCoroutine(FirstCollect());
        finalPos = finalPoint.position;
    }

    private IEnumerator FirstCollect()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("FirstCollect");

        BrickGenerator.SpawnedBricks[] spawnedBricks = brickGenerator.spawnedBricks;

        for(int i = 0; i < spawnedBricks.Length; i++)
        {
            if(spawnedBricks[i].colorName == brickController.selectedColorName)
            {
                selectedColorCount++;
            }
        }

        if(brickController.brickCount < selectedColorCount)
        {
            for(int i = 0; i < spawnedBricks.Length; i++)
            {
                if(spawnedBricks[i].isRemoved != true && spawnedBricks[i].colorName == brickController.selectedColorName)
                {
                    navMeshAgent.SetDestination(spawnedBricks[i].position);
                    StopCoroutine(FirstCollect());
                }
            }
        }
    }

    public void CollectBrick()
    {
        BrickGenerator.SpawnedBricks[] spawnedBricks = brickGenerator.spawnedBricks;

        Debug.Log("SelectColorCount: " + selectedColorCount);

        int rdNumber = Random.Range(0, selectedColorCount);

        if(brickController.brickCount < selectedColorCount - rdNumber)
        {
            for(int i = 0; i < spawnedBricks.Length; i++)
            {
                if(spawnedBricks[i].isRemoved != true && spawnedBricks[i].colorName == brickController.selectedColorName)
                {
                    navMeshAgent.SetDestination(spawnedBricks[i].position);
                    return;
                }
            }
        }
        else
        {
            Place_CollectBrick();
        }
    }

    public void Place_CollectBrick()
    {
        if(brickController.brickCount == 0)
        {
            CollectBrick();
        }
        else
        {
            navMeshAgent.SetDestination(finalPos);
        }
    }

    public void UpdateMesh()
    {
        UnityEditor.AI.NavMeshBuilder.BuildNavMesh();
    }
}
