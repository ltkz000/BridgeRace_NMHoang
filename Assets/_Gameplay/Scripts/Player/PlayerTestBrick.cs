using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTestBrick : MonoBehaviour
{
    public Pooling spawnerPooling;
    public BrickGenerator brickGenerator;


    [SerializeField] private Transform playerModel;
    [SerializeField] private Transform dropedBrickHolder;
    [SerializeField] private Transform carryPoint;
    [SerializeField] private Transform placePoint;
    [SerializeField] private Transform placedBrickPrefab;
    [SerializeField] private Color selectedColor;
    [SerializeField] private string selectedColorName;
    [SerializeField] private GameObject brideWay;
    
    private Pooling poolingDropedBrick;
    private Pooling poolingCarryPoint;
    private PlacedBrick placedBrickScript;
    private float placedBrickSizey, placedBrickSizez;
    public int brickCount = 0;
    private float force = 50f;
    private float subForce = 10f;


    private void Start() 
    {
        poolingDropedBrick = dropedBrickHolder.GetComponent<Pooling>();
        poolingCarryPoint = carryPoint.GetComponent<Pooling>();

        placedBrickSizey = placedBrickPrefab.GetComponent<Renderer>().bounds.size.y;
        placedBrickSizez = placedBrickPrefab.GetComponent<Renderer>().bounds.size.z;
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
            dropedBrick.GetComponent<Rigidbody>().AddForce(playerModel.transform.forward * (force + (brickCount * subForce)) * (-1));

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
