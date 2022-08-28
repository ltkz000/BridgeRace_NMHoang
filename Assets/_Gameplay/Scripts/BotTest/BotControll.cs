using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotControll : MonoBehaviour
{
    [SerializeField] private BrickGenerator brickGenerator;
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private BotBrickControll botBrickControll;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform finalPoint;
    [SerializeField] private Animator animator;
    BrickGenerator.SpawnedBricks[] spawnedBricks;
    private int selectedColorCount;
    private Vector3 finalPos;
    int rdNumber;

    void Start()
    {
        spawnedBricks = brickGenerator.spawnedBricks;

        StartCoroutine(FirstCollect());
        finalPos = finalPoint.position;
    }

    private IEnumerator FirstCollect()
    {
        yield return new WaitForSeconds(1f);

        for(int i = 0; i < spawnedBricks.Length; i++)
        {
            if(spawnedBricks[i].colorName == botBrickControll.selectedColorName)
            {
                selectedColorCount++;
            }
        }

        if(botBrickControll.brickCount < selectedColorCount)
        {
            for(int i = 0; i < spawnedBricks.Length; i++)
            {
                if(spawnedBricks[i].isRemoved != true && spawnedBricks[i].colorName == botBrickControll.selectedColorName)
                {
                    navMeshAgent.SetDestination(spawnedBricks[i].position);
                    animator.SetBool("isRunning", navMeshAgent.velocity.magnitude > 0.01f);
                    StopCoroutine(FirstCollect());
                }
            }
        }
    }

    private void Update() {
        spawnedBricks = brickGenerator.spawnedBricks;
    }

    public void CollectBrick()
    {
        selectedColorCount = 0;

        for(int i = 0; i < spawnedBricks.Length; i++)
        {
            if(spawnedBricks[i].colorName == botBrickControll.selectedColorName)
            {
                selectedColorCount++;
            }
        }

        if(botBrickControll.brickCount < selectedColorCount - 3)
        {
            for(int i = 0; i < spawnedBricks.Length; i++)
            {
                if(spawnedBricks[i].isRemoved != true && spawnedBricks[i].colorName == botBrickControll.selectedColorName)
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
        if(botBrickControll.brickCount == 0)
        {
            CollectBrick();
        }
        else
        {
            navMeshAgent.SetDestination(finalPos);
        }
    }

    // public void UpdateMesh()
    // {
    //     UnityEditor.AI.NavMeshBuilder.BuildNavMesh();
    // }
}
