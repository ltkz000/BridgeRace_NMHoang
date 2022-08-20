using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectController : MonoBehaviour
{
    private PlayerBrickController playerBrickController; 
    private BotController botController;
    [SerializeField] private BrickGenerator brickGenerator;
    [SerializeField] private Transform playerModel;
    [SerializeField] private Transform brickHolder;
    [SerializeField] private bool isBot;

    public string playerColorName;

    private void Start() 
    {
        playerBrickController = GetComponent<PlayerBrickController>();    
        
        if(isBot)
        {
            botController = GetComponent<BotController>();
        }
    }

    private void OnTriggerEnter(Collider other) {
        Brick brick = other.transform.GetComponent<Brick>();
        Debug.Log("Bot test");
        
        if(brick.colorName == playerColorName)
        {
            Debug.Log("Bot brick");
            brickGenerator.RemovePickedBrick(brick.brickNumber);
            Destroy(other.gameObject);
            playerBrickController.UpdateBrickHolder();
        }

        if(isBot)
        {
            botController.CollectBrick();
            Debug.Log("Bot entered");
        }
    }
}
