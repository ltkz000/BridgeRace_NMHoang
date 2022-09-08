using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotBrickControll : MonoBehaviour
{
    public GameObject brideWay;
    public Pooling spawnerPooling;
    public BrickGenerator brickGenerator;
    public NavMeshAgent agent;
    
    [SerializeField] private AIController aIController;
    [SerializeField] private Transform playerModel;
    [SerializeField] private Transform dropedBrickHolder;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform carryPoint;
    [SerializeField] private Transform placePoint;
    [SerializeField] private Transform placedBrickPrefab;
    [SerializeField] private Transform finishLine;
    [SerializeField] private BridgeWay bridgeWay;
    [SerializeField] private Color selectedColor;
    [SerializeField] private string selectedColorName;


    private Pooling poolingDropedBrick;
    private Pooling poolingCarryPoint;
    private PlacedBrick placedBrickScript;
    private int check;
    private float placedBrickSizey, placedBrickSizez;
    private int selectedColorCount;
    public int brickCount = 0;
    private float force = 50f;
    private float subForce = 10f;

    private void Start()
    {
        poolingDropedBrick = dropedBrickHolder.GetComponent<Pooling>();
        poolingCarryPoint = carryPoint.GetComponent<Pooling>();

        placedBrickSizey = placedBrickPrefab.GetComponent<Renderer>().bounds.size.y;
        placedBrickSizez = placedBrickPrefab.GetComponent<Renderer>().bounds.size.z;
        // selectedColorCount = Random.Range(4,8);

        selectedColorCount = brickGenerator.GetSelectedColor(selectedColorName);
    }

    public void BotCollect()
    {
        Debug.Log("Collect");

        // selectedColorCount = brickGenerator.GetSelectedColor(selectedColorName);
        for(int i = brickGenerator.spawnedBricks.Length - 1; i >= 0; i--)
        {
            if(brickGenerator.spawnedBricks[i].isRemoved != true && brickGenerator.spawnedBricks[i].colorName == selectedColorName)
            {
                check++;
                agent.SetDestination(brickGenerator.spawnedBricks[i].position);
                animator.SetBool("isRunning", agent.velocity.magnitude > 0.01f);
            }
        }
        
        if(brickCount >= brickGenerator.GetSelectedColor(selectedColorName))
        {
            StartCoroutine(waitBuild());   
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
        // animator.SetTrigger("Idle");

        if(FrameManager.Ins.currentgameState == GameState.GamePlay)
        {
            aIController.switchState(BotState.Collect);
        }
    }

    public void BotBuild()
    {
        agent.SetDestination(bridgeWay.finishPoint.position);

        if(brickCount == 0)
        {
            StartCoroutine(waitBuild());
            aIController.switchState(BotState.Collect);
        }
    }

    public void BotFall()
    {

    }

    public void BotWin()
    {
        agent.SetDestination(finishLine.position);
    }

    public void PlaceBrick(GameObject placedBrick)
    {
        placedBrickScript = placedBrick.GetComponent<PlacedBrick>();

        if(brickCount > 0 && placedBrickScript.colorName != selectedColorName)
        {
            placedBrickScript.colorName = selectedColorName;
            placedBrickScript.meshRenderer.enabled = true;
            placedBrickScript.meshRenderer.material.SetColor("_Color", selectedColor);
            placedBrickScript.colorName = selectedColorName;

            RemovedTopBrick();

            brickGenerator.GeneratedRemovedBrick();
        }
        else if(placedBrickScript.colorName == selectedColorName)
        {

        }
        else
        {
            placedBrickScript.boxCollider.isTrigger = false;
        }
    }

    public void ReplaceBrick(GameObject other)
    {
        placedBrickScript = other.GetComponent<PlacedBrick>();

        if(placedBrickScript.colorName != selectedColorName)
            {
                if(brickCount > 0)
                {
                    placedBrickScript.colorName = selectedColorName;
                    placedBrickScript.meshRenderer.material.SetColor("_Color", selectedColor);
                    placedBrickScript.colorName = selectedColorName;

                    RemovedTopBrick();

                    brickGenerator.GeneratedRemovedBrick();
                }
                else
                {
                    placedBrickScript.boxCollider.isTrigger = false;
                }
            }
    }

    public void PickedUp(GameObject other)
    {
        Brick brick = other.GetComponent<Brick>();

        if(brick.colorName == selectedColorName)
        {
            brickGenerator.RemovePickedBrick(brick.brickNumber);
            spawnerPooling.ReturnObject(other.gameObject);
            UpdateBrickHolder();
        }
        else if(brick.colorName == "gray")
        {
            poolingDropedBrick.ReturnObject(other.gameObject);
            brickGenerator.GeneratedRemovedBrick();
            UpdateBrickHolder();
        }
    }

    public void UpdateBrickHolder()
    {
        Vector3 newPositon = new Vector3(carryPoint.position.x, carryPoint.position.y + brickCount*0.05f, carryPoint.position.z);
        GameObject brick = poolingCarryPoint.GetObject();
        brick.transform.position = newPositon;
        brick.transform.rotation = playerModel.rotation;
        brick.GetComponent<Renderer>().material.SetColor("_Color", selectedColor);
        brick.SetActive(true);
        brickCount++;
    }

    public void DropBrick()
    {
        while(brickCount > 0)
        {
            GameObject lastChild = carryPoint.GetChild(carryPoint.childCount - brickCount).gameObject;
            GameObject dropedBrick = poolingDropedBrick.GetObject();
            dropedBrick.transform.position = lastChild.transform.position;
            dropedBrick.transform.rotation = dropedBrick.transform.rotation;
            dropedBrick.SetActive(true);
            dropedBrick.GetComponent<Rigidbody>().AddForce(playerModel.transform.forward * (force + (brickCount * subForce)));

            poolingCarryPoint.ReturnObject(lastChild);
            brickCount--;
        }
    }

    public void RemovedTopBrick()
    {
        GameObject lastChild = carryPoint.GetChild(carryPoint.childCount - brickCount).gameObject;
        poolingCarryPoint.ReturnObject(lastChild);
        brickCount--;
    }

    public void DropAllBrick()
    {
        while(brickCount != 0)
        {
            GameObject lastChild = carryPoint.GetChild(carryPoint.childCount - brickCount).gameObject;
            poolingCarryPoint.ReturnObject(lastChild);
            brickCount--;
        }
    }
}
