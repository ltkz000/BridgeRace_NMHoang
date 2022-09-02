using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeWay : MonoBehaviour
{
    public int bricksPlaced;

    public Transform startWall;

    public BoxCollider boxCollider;

    private void Start() {
        boxCollider = startWall.GetComponent<BoxCollider>();
    }

    public void MoveStartWall(Vector3 newPos)
    {
        startWall.position = newPos;
        boxCollider.enabled = true;
    }

    public void TurnOnStartWall()
    {
        boxCollider.enabled = true;
    }

    public void TurnOffStartWall()
    {
        boxCollider.enabled = false;
    }
}
