using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBrickController : MonoBehaviour
{
    private BrickGenerator brickGenerator;
    private BotController botController;
    [SerializeField] private Transform brideWay;
    [SerializeField] private Transform playerModel;
    [SerializeField] private Transform carryPoint;
    [SerializeField] private Transform placePoint;
    [SerializeField] private Transform placeBrickHolder;
    [SerializeField] private Transform normalBrick;
    [SerializeField] private Transform placedBrickPrefab;
    [SerializeField] private bool isBot;


    private float placedBrickSizey, placedBrickSizez;
    public Color selectedColor;
    public string selectedColorName;
    private int brickCount = 0;
    private int placedBrickCount;
    private Vector3 bridgePos;


    private void Start() 
    {
        brickGenerator = FindObjectOfType<BrickGenerator>();

        placedBrickSizey = placedBrickPrefab.GetComponent<Renderer>().bounds.size.y;
        placedBrickSizez = placedBrickPrefab.GetComponent<Renderer>().bounds.size.z;

        if(isBot)
        {
            botController = gameObject.GetComponent<BotController>();
        }
    }

    private void FixedUpdate() 
    {
        CheckBridge();
        Debug.Log("brickCount - " + brickCount);
    }

    private void CheckBridge()
    {
        RaycastHit hit;
        if(Physics.Raycast(placePoint.position, playerModel.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
        {
            Debug.DrawRay(placePoint.position, playerModel.TransformDirection(Vector3.down) * hit.distance, Color.magenta);
            PlacedBrick checkBrick = hit.collider.GetComponent<PlacedBrick>();

            // if(hit.collider.CompareTag("Test"))
            // {
            //     hit.collider.GetComponent<ColliderTest>().TurnColliOn();
            // }

            if(hit.collider.CompareTag("Bridge"))
            {
                if(brickCount > 0)
                {
                    PlaceBrick(hit);
                }
                else
                {
                    placedBrickCount = hit.collider.GetComponent<BridgeWay>().bricksPlaced;
                    hit.collider.GetComponent<BridgeWay>().TurnOnStartWall();
                    // BlockFallOff(hit);
                }
            }
            else
            {
                brideWay.GetComponent<BridgeWay>().TurnOffStartWall();
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
        Transform brick = Instantiate(normalBrick, newPositon, playerModel.rotation, carryPoint);
        brick.GetComponent<Renderer>().material.SetColor("_Color", selectedColor);
        brickCount++;
    }

    private void RemovedTopBrick()
    {
        GameObject lastChild = carryPoint.GetChild(brickCount - 1).gameObject;
        Destroy(lastChild);
        brickCount--;
    }

    private void PlaceBrick(RaycastHit hit)
    {
        placedBrickCount = hit.collider.GetComponent<BridgeWay>().bricksPlaced;

        bridgePos = hit.collider.transform.position;

        Vector3 placeBrickPos = new Vector3(bridgePos.x, bridgePos.y + (placedBrickCount * placedBrickSizey), 
                                                bridgePos.z + (placedBrickCount * placedBrickSizez) - 1.025f);
        // Vector3 placeBrickPos = bridgePos;

        Transform placedBrick = Instantiate(placedBrickPrefab, placeBrickPos, placedBrickPrefab.rotation, placeBrickHolder);

        placedBrick.GetComponent<Renderer>().material.SetColor("_Color", selectedColor);
        placedBrick.GetComponent<PlacedBrick>().colorName = selectedColorName;

        hit.collider.GetComponent<BridgeWay>().bricksPlaced++;

        RemovedTopBrick();

        brickGenerator.GeneratedRemovedBrick();

        Debug.Log("placeBrickPos: " + placeBrickPos);
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
