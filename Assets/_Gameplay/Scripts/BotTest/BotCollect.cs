using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotCollect : MonoBehaviour
{
    [SerializeField] private BotBrickControll botBrickControll;
    [SerializeField] private BotControll botControll;
    [SerializeField] private Pooling spawnerPooling;
    [SerializeField] private BrickGenerator brickGenerator;

    public string playerColorName;

    private void OnTriggerEnter(Collider other)
    {
        Brick brick = other.transform.GetComponent<Brick>();

        if(brick.colorName == playerColorName)
        {
            brickGenerator.RemovePickedBrick(brick.brickNumber);
            spawnerPooling.ReturnObject(other.gameObject);
            botBrickControll.UpdateBrickHolder();
            botControll.CollectBrick();
        }
    }
}
