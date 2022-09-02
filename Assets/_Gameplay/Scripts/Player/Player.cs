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
    private Vector3 move;
    
    private void Awake() 
    {
        playerInput = new PlayerInput();
        controller = GetComponent<CharacterController>();
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
}

