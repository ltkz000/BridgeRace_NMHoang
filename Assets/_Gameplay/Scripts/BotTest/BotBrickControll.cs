using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotBrickControll : MonoBehaviour
{
    public GameObject brideWay;
    public Pooling spawnerPooling;
    public BrickGenerator brickGenerator;
    

    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private AIController aIController;
    [SerializeField] private Transform playerModel;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform carryPoint;
    [SerializeField] private Transform placePoint;
    [SerializeField] private Transform placedBrickPrefab;
    [SerializeField] Transform startPoint;
    [SerializeField] Transform finishPoint;
    [SerializeField] private Color selectedColor;
    [SerializeField] private string selectedColorName;


    private int check;
    private int brickCount = 0;
    private float placedBrickSizey, placedBrickSizez;
    private Pooling poolingCarryPoint;
    private int selectedColorCount;


    private void Start()
    {
        poolingCarryPoint = carryPoint.GetComponent<Pooling>();

        placedBrickSizey = placedBrickPrefab.GetComponent<Renderer>().bounds.size.y;
        placedBrickSizez = placedBrickPrefab.GetComponent<Renderer>().bounds.size.z;
        // selectedColorCount = Random.Range(4,8);

        selectedColorCount = brickGenerator.GetSelectedColor(selectedColorName);
    }

    private void Update() {
        Debug.Log("brickCount: " + brickCount);
        CheckBridge();
    }

    public void BotCollect()
    {
        Debug.Log("Collect");

        for(int i = brickGenerator.spawnedBricks.Length - 1; i >= 0; i--)
        {
            if(brickGenerator.spawnedBricks[i].isRemoved != true && brickGenerator.spawnedBricks[i].colorName == selectedColorName)
            {
                check++;
                agent.SetDestination(brickGenerator.spawnedBricks[i].position);
                animator.SetBool("isRunning", agent.velocity.magnitude > 0.01f);
            }
        }
        
        if(brickCount == selectedColorCount)
        {
            aIController.switchState(BotState.Build);
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
    }

    public void BotBuild()
    {
        agent.SetDestination(startPoint.position);

        if(brickCount == 0)
        {
            aIController.switchState(BotState.Collect);
            selectedColorCount = Random.Range(4,8);
        }
    }

     private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag(selectedColorName))
        {
            agent.SetDestination(finishPoint.position);
        }
    }


    private void CheckBridge()
    {
        RaycastHit hit;
        if(Physics.Raycast(placePoint.position, playerModel.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
        {
            Debug.DrawRay(placePoint.position, playerModel.TransformDirection(Vector3.down) * hit.distance, Color.magenta);

            if(hit.collider.CompareTag("PlacedBrick"))
            {
                Debug.Log("ground");
                if(brideWay != null)
                {
                    brideWay.GetComponent<BridgeWay>().TurnOffStartWall();
                } 
            }
        }
        else
        {
            Debug.DrawRay(placePoint.position, placePoint.TransformDirection(Vector3.down) * 1000, Color.yellow);
        }
    }

    public void PlaceBrick(GameObject placedBrick)
    {
        if(brickCount > 0)
        {
            placedBrick.tag = selectedColorName;
            placedBrick.GetComponent<MeshRenderer>().enabled = true;
            placedBrick.GetComponent<MeshRenderer>().material.SetColor("_Color", selectedColor);
            placedBrick.GetComponent<PlacedBrick>().colorName = selectedColorName;

            RemovedTopBrick();

            brickGenerator.GeneratedRemovedBrick();

            agent.SetDestination (finishPoint.position);
        }
        else if(brickCount == 0)
        {
            brideWay.GetComponent<BridgeWay>().MoveStartWall(placedBrick.transform.position + new Vector3(.0f, .0f, 0.5f));
        }
    }

    public void ReplaceBrick(GameObject other)
    {
        if(other.tag != selectedColorName)
            {
                if(brickCount > 0)
                {
                    other.tag = selectedColorName;
                    other.GetComponent<MeshRenderer>().material.SetColor("_Color", selectedColor);
                    other.GetComponent<PlacedBrick>().colorName = selectedColorName;

                    RemovedTopBrick();

                    brickGenerator.GeneratedRemovedBrick();
                }
                else
                {
                    brideWay.GetComponent<BridgeWay>().MoveStartWall(other.transform.position + new Vector3(.0f, .0f, 0.5f));
                }
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

    public void RemovedTopBrick()
    {
        GameObject lastChild = carryPoint.GetChild(carryPoint.childCount - brickCount).gameObject;
        poolingCarryPoint.ReturnObject(lastChild);
        brickCount--;
    }

    public void Hit(GameObject player)
    {

    }
}
