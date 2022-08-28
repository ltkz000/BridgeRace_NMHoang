using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBrickController : MonoBehaviour
{
    [SerializeField] private GameObject brideWay;
    [SerializeField] private Transform playerModel;
    [SerializeField] private Transform carryPoint;
    private Pooling poolingCarryPoint;
    [SerializeField] private Transform placePoint;
    [SerializeField] private Transform placeBrickHolder;
    private Pooling poolingPlacedBrick;
    [SerializeField] private Transform placedBrickPrefab;
    private Stage currentStage;
    private BrickGenerator brickGenerator;
    LevelManager ins;


    private float placedBrickSizey, placedBrickSizez;
    public Color selectedColor;
    public string selectedColorName;
    public int brickCount = 0;
    private int placedBrickCount;
    private Vector3 bridgePos;


    private void Start() 
    {
        ins = LevelManager.Ins;
        currentStage = Stage.Stage1;
        brickGenerator = FindObjectOfType<BrickGenerator>();
        poolingCarryPoint = carryPoint.GetComponent<Pooling>();
        poolingPlacedBrick = placeBrickHolder.GetComponent<Pooling>();

        placedBrickSizey = placedBrickPrefab.GetComponent<Renderer>().bounds.size.y;
        placedBrickSizez = placedBrickPrefab.GetComponent<Renderer>().bounds.size.z;
    }

    private void FixedUpdate() 
    {
        CheckBridge();
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Stage1"))
        {
            currentStage = ins.switchStage(1);
            brickGenerator = ins.ChooseSpawner(currentStage);
        }
        else if(other.CompareTag("Stage2"))
        {
            currentStage = ins.switchStage(2);
            brickGenerator = ins.ChooseSpawner(currentStage);
        }
        else if(other.CompareTag("Stage3"))
        {
            currentStage = ins.switchStage(3);
            brickGenerator = ins.ChooseSpawner(currentStage);
        }
    }

    private void CheckBridge()
    {
        RaycastHit hit;
        if(Physics.Raycast(placePoint.position, playerModel.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
        {
            Debug.DrawRay(placePoint.position, playerModel.TransformDirection(Vector3.down) * hit.distance, Color.magenta);
            PlacedBrick checkBrick = hit.collider.GetComponent<PlacedBrick>();

            if(hit.collider.CompareTag("Bridge"))
            {
                brideWay = hit.collider.gameObject;
                if(brickCount > 0 && placedBrickCount < 12)
                {
                    PlaceBrick(hit);
                }
                else
                {
                    brideWay.GetComponent<BridgeWay>().TurnOnStartWall();
                    placedBrickCount = hit.collider.GetComponent<BridgeWay>().bricksPlaced;
                }
            }

            if(hit.collider.CompareTag("Ground"))
            {
                if(brideWay != null)
                {
                    brideWay.GetComponent<BridgeWay>().TurnOffStartWall();
                } 
            }

            if(checkBrick != null && checkBrick.colorName != selectedColorName && brickCount <= 0)
            {
                BlockMoveOnOtherBrick(hit);
            }
            else if(checkBrick != null && checkBrick.colorName != selectedColorName && brickCount > 0)
            {
                ReplaceOtherBrick(hit);
            }
        }
        else
        {
            Debug.DrawRay(placePoint.position, placePoint.TransformDirection(Vector3.down) * 1000, Color.yellow);
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

    private void RemovedTopBrick()
    {
        GameObject lastChild = carryPoint.GetChild(carryPoint.childCount - brickCount).gameObject;
        poolingCarryPoint.ReturnObject(lastChild);
        brickCount--;
    }

    private void PlaceBrick(RaycastHit hit)
    {
        placedBrickCount = hit.collider.GetComponent<BridgeWay>().bricksPlaced;

        bridgePos = hit.collider.transform.position;

        Vector3 placeBrickPos = new Vector3(bridgePos.x, bridgePos.y + (placedBrickCount * placedBrickSizey), 
                                                bridgePos.z + (placedBrickCount * placedBrickSizez) - 1.025f);

        GameObject placedBrick = poolingPlacedBrick.GetObject();
        placedBrick.transform.position = placeBrickPos;
        placedBrick.GetComponent<Renderer>().material.SetColor("_Color", selectedColor);
        placedBrick.GetComponent<PlacedBrick>().colorName = selectedColorName;
        placedBrick.SetActive(true);

        hit.collider.GetComponent<BridgeWay>().bricksPlaced++;

        RemovedTopBrick();

        brickGenerator.GeneratedRemovedBrick();

        hit.collider.GetComponent<BridgeWay>().MoveStartWall(placeBrickPos + new Vector3(.0f, .0f, 0.6f));
    }

    private void BlockMoveOnOtherBrick(RaycastHit hit)
    {
        Vector3 blockWallPos = hit.collider.transform.position;

        brideWay.GetComponent<BridgeWay>().MoveStartWall(blockWallPos + new Vector3(.0f, .0f, 0.6f));
    }

    private void ReplaceOtherBrick(RaycastHit hit)
    {
        Vector3 newBrickPos = hit.transform.position;

        Destroy(hit.collider.gameObject);

        Transform newBrick = Instantiate(placedBrickPrefab, newBrickPos, placedBrickPrefab.transform.rotation, placeBrickHolder);

        newBrick.GetComponent<Renderer>().material.SetColor("_Color", selectedColor);
        newBrick.GetComponent<PlacedBrick>().colorName = selectedColorName;

        RemovedTopBrick();

        brickGenerator.GeneratedRemovedBrick();
    }
}
