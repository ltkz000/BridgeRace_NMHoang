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
    [SerializeField] private Animator animator;
    private int selectedColorCount;
    private Vector3 finalPos;
    int rdNumber;


    private void Start() 
    {
        brickGenerator = FindObjectOfType<BrickGenerator>();

        StartCoroutine(FirstCollect());
        finalPos = finalPoint.position;
    }

    private IEnumerator FirstCollect()
    {
        yield return new WaitForSeconds(1f);

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
                    // animator.SetBool("isRunning", navMeshAgent.velocity.magnitude > 0.01f);
                    StopCoroutine(FirstCollect());
                }
            }
        }
    }

    public void CollectBrick()
    {
        BrickGenerator.SpawnedBricks[] spawnedBricks = brickGenerator.spawnedBricks;

        // rdNumber = Random.Range(0, selectedColorCount);

        if(brickController.brickCount < selectedColorCount)
        {
            for(int i = 0; i < spawnedBricks.Length; i++)
            {
                if(spawnedBricks[i].isRemoved != true && spawnedBricks[i].colorName == brickController.selectedColorName)
                {
                    navMeshAgent.SetDestination(spawnedBricks[i].position);
                    // animator.SetBool("isRunning", navMeshAgent.velocity.magnitude > 0.01f);
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
            // animator.SetBool("isRunning", navMeshAgent.velocity.magnitude > 0.01f);
        }
    }

    public void UpdateMesh()
    {
        UnityEditor.AI.NavMeshBuilder.BuildNavMesh();
    }
}
