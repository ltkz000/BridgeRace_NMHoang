using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotBrickControll : MonoBehaviour
{
    [SerializeField] BotControll botControll;
    [SerializeField] private BrickGenerator brickGenerator;
    [SerializeField] private Transform brideWay;
    [SerializeField] private Transform playerModel;
    [SerializeField] private Transform carryPoint;
    private Pooling poolingCarryPoint;
    [SerializeField] private Transform placePoint;
    [SerializeField] private Transform placeBrickHolder;
    private Pooling poolingPlacedBrick;
    [SerializeField] private Transform placedBrickPrefab;


    private float placedBrickSizey, placedBrickSizez;
    public Color selectedColor;
    public string selectedColorName;
    public int brickCount = 0;
    private int placedBrickCount;
    private Vector3 bridgePos;

    // Start is called before the first frame update
    private void Start()
    {
        poolingCarryPoint = carryPoint.GetComponent<Pooling>();
        poolingPlacedBrick = placeBrickHolder.GetComponent<Pooling>();

        placedBrickSizey = placedBrickPrefab.GetComponent<Renderer>().bounds.size.y;
        placedBrickSizez = placedBrickPrefab.GetComponent<Renderer>().bounds.size.z;
    }

    private void FixedUpdate() {
        CheckBridge();
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
                if(brickCount > 0)
                {
                    PlaceBrick(hit);
                }
                else
                {
                    botControll.Place_CollectBrick();
                    hit.collider.GetComponent<BridgeWay>().TurnOnStartWall();

                    placedBrickCount = hit.collider.GetComponent<BridgeWay>().bricksPlaced;
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
    }

    private void BlockMoveOnOtherBrick(RaycastHit hit)
    {
        Vector3 blockWallPos = hit.collider.transform.position;

        brideWay.GetComponent<BridgeWay>().MoveStartWall(blockWallPos + new Vector3(.0f, .0f, 0.6f));
    }

    private void ReplaceOtherBrick(RaycastHit hit)
    {

    }
}
