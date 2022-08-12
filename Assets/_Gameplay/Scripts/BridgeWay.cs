using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeWay : MonoBehaviour
{
    public int bricksPlaced;

    public Transform startWall;

    public void MoveStartWall(Vector3 newPos)
    {
        startWall.position = newPos;
        startWall.GetComponent<BoxCollider>().enabled = true;
    }

    public void TurnOnStartWall()
    {
        startWall.GetComponent<BoxCollider>().enabled = true;
    }

    public void TurnOffStartWall()
    {
        startWall.GetComponent<BoxCollider>().enabled = false;
    }
}
