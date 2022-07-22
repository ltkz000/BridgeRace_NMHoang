using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerInput playerInput;
    private CharacterController controller;
    public Animator animator;
    [SerializeField] private Transform playerModel;
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private Transform brickHolder;

    // [SerializeField] private float rotationSpeed;
    // Variables
    // private float currentGravity;
    private Vector3 move;
    private int brickCount;
    public string playerColor;
    [SerializeField] private bool isBot;

    private void Awake() 
    {
        playerInput = new PlayerInput();
        controller = GetComponent<CharacterController>();

        brickCount = 0;
        // currentGravity = .0f;
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable() 
    {
        playerInput.Disable();
    }

    void Update()
    {
        Vector2 movementInput = playerInput.CharacterControls.Move.ReadValue<Vector2>();

        // gravityHandle();

        move = new Vector3(movementInput.x, .0f, movementInput.y);
        
        controller.Move(move * Time.deltaTime * playerSpeed);

        if(move != Vector3.zero)
        {
            playerModel.forward = move;
        }

        AnimationHandle();
    }

    private void AnimationHandle()
    {
        bool isRunning = animator.GetBool("isRunning");
        if(move != Vector3.zero && !isRunning)
        {
            animator.SetBool("isRunning", true);
        }
        else if(move == Vector3.zero && isRunning)
        {
            animator.SetBool("isRunning", false);
        }
    }

    // private void gravityHandle()
    // {
    //     if(controller.isGrounded)
    //     {
    //         // float groundedGravity = -.05f;
    //         currentGravity = .0f;
    //     }
    //     else
    //     {
    //         // float gravity = -9.8f;
    //         currentGravity = -9.8f;
    //     }   
    // }

    private void OnTriggerEnter(Collider other) {
        // Brick brick = other.transform.GetComponent<Brick>();
        Debug.Log("Enter");
        
        Vector3 currentpositon = other.transform.position;
        Vector3 nextpositon = brickHolder.position;

        other.transform.position = Vector3.Lerp(currentpositon, nextpositon, Time.deltaTime);
        other.transform.SetParent(brickHolder);
    }
}

