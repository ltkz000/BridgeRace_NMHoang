using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickGenerator : MonoBehaviour
{
    [SerializeField] private Transform brickPrefab;

    private Vector3 startPoint;
    private Vector3 position;

    private int  length = 40;
    private int line = 8;
    private int xOrder = 0;
    [SerializeField] private float brickDistance;

    private float xPosition;
    private float zPosition;

    [System.Serializable]
    public class ColorData
    {
        public Color color;
        public string colorName;
    }
    public ColorData[] colorArray;

    [System.Serializable]
    public class SpawnedBricks
    {
        public Color color;
        public string colorName;
        public Vector3 position;
        public bool isRemoved;
    } 
    public SpawnedBricks[] spawnedBricks;

    private void Start(){
        startPoint = transform.position;
        xPosition = transform.position.x;
        zPosition = transform.position.z;

        spawnedBricks = new SpawnedBricks[length];

        CreateBrick();
    }

    private void CreateBrick()
    {
        for(int i = 0; i < length; i++)
        {
            xOrder++;
            if(i % line == 0)
            {
                zPosition -= 1 * brickDistance;
                xOrder = 0;
                position = new Vector3(xPosition, startPoint.y, zPosition);
            }
            else
            {
                position = new Vector3(xPosition + xOrder * brickDistance, startPoint.y, zPosition);
            }
            Transform createdBrick = Instantiate(brickPrefab, position, brickPrefab.transform.rotation, transform);

            //giveColor
            GiveColor(createdBrick, i);
        }
    }

    private void GiveColor(Transform createdBrick, int i)
    {
        int randomColor = Random.Range(0, colorArray.Length);
        createdBrick.GetComponent<Renderer>().material.SetColor("_Color", colorArray[randomColor].color);
        createdBrick.GetComponent<Brick>().colorName = colorArray[randomColor].colorName;
        createdBrick.GetComponent<Brick>().brickNumber = i;

        //InsertIntoArray
        InsertIntoArray(colorArray[randomColor].color, colorArray[randomColor].colorName, createdBrick, i);
    }

    private void InsertIntoArray(Color _color, string _colorName, Transform createdBrick, int i)
    {
        var temp = new SpawnedBricks();

        temp.color = _color;
        temp.colorName = _colorName;
        temp.position = createdBrick.position;
        temp.isRemoved = false;

        spawnedBricks[i] = temp;
    }

    public void RemovePickedBrick(int brickNumber)
    {
        spawnedBricks[brickNumber].isRemoved = true;
    }

    public void GeneratedRemovedBrick()
    {
        for(int i = 0; i < length; i++)
        {
            if(spawnedBricks[i].isRemoved == true)
            {
                Transform createdBrick = Instantiate(brickPrefab, spawnedBricks[i].position, brickPrefab.rotation, transform);

                createdBrick.GetComponent<Renderer>().material.SetColor("_Color", spawnedBricks[i].color);
                createdBrick.GetComponent<Brick>().colorName = spawnedBricks[i].colorName;
                createdBrick.GetComponent<Brick>().brickNumber = i;

                spawnedBricks[i].isRemoved = false;
                return;
            }
        }
    }
}
