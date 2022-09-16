using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickController : MonoBehaviour
{
    public Pooling spawnerPooling;
    public BrickGenerator brickGenerator;
    
    [SerializeField] private Transform playerModel;
    [SerializeField] private Transform dropedBrickHolder;
    [SerializeField] private Transform carryPoint;
    [SerializeField] private Transform placedBrickPrefab;
    [SerializeField] private Color selectedColor;
    [SerializeField] protected string selectedColorName;
    [SerializeField] private PlacedBrick placedBrickScript;


    private Pooling poolingDropedBrick;
    private Pooling poolingCarryPoint;
    private float placedBrickSizey, placedBrickSizez;
    public int brickCount = 0;
    private float force = 50f;
    private float subForce = 10f;

    public virtual void Start()
    {
        poolingDropedBrick = dropedBrickHolder.GetComponent<Pooling>();
        poolingCarryPoint = carryPoint.GetComponent<Pooling>();

        // placedBrickSizey = placedBrickPrefab.GetComponent<Renderer>().bounds.size.y;
        // placedBrickSizez = placedBrickPrefab.GetComponent<Renderer>().bounds.size.z;

        placedBrickSizey = placedBrickScript.meshRenderer.bounds.size.y;
        placedBrickSizez = placedBrickScript.meshRenderer.bounds.size.z;
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

        if(brick != null)
        {
            if(brick.colorName == selectedColorName)
            {
                brickGenerator.RemovePickedBrick(brick.brickNumber);
                spawnerPooling.ReturnObject(other.gameObject);
                UpdateBrickHolder();
            }
        else if(brick.colorName == ConstValue.DROPPED_GRAYBRICK_TAG)
            {
                poolingDropedBrick.ReturnObject(other.gameObject);
                brickGenerator.GeneratedRemovedBrick();
                UpdateBrickHolder();
            }
        }
    }

    public void UpdateBrickHolder()
    {
        Vector3 newPositon = new Vector3(carryPoint.position.x, carryPoint.position.y + brickCount*0.05f, carryPoint.position.z);
        GameObject brick = poolingCarryPoint.GetObject(newPositon);
        brick.transform.rotation = playerModel.rotation;
        brick.GetComponent<Renderer>().material.SetColor("_Color", selectedColor);
        brickCount++;
    }

    public void DropBrick()
    {
        while(brickCount > 0)
        {
            GameObject lastChild = carryPoint.GetChild(carryPoint.childCount - brickCount).gameObject;
            GameObject dropedBrick = poolingDropedBrick.GetObject(lastChild.transform.position);

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
