using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectController : MonoBehaviour
{
    private PlayerBrickController playerBrickController; 
    [SerializeField] private BrickGenerator brickGenerator;
    [SerializeField] private Transform playerModel;
    [SerializeField] private Transform brickHolder;
    [SerializeField] private bool isBot;

    public string playerColorName;

    private void Start() 
    {
        playerBrickController = GetComponent<PlayerBrickController>();    
    }

    private void OnTriggerEnter(Collider other) {
        Brick brick = other.transform.GetComponent<Brick>();
        
        if(brick.colorName == playerColorName)
        {
            brickGenerator.RemovePickedBrick(brick.brickNumber);
            Destroy(other.gameObject);
            playerBrickController.UpdateBrickHolder();
        }

        if(isBot)
        {

        }
    }
}
