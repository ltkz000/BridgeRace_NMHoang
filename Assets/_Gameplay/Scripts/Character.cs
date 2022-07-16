using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Character : MonoBehaviour
{
    //Components
    PlayerInput playerInput;
    CharacterController characterController;
    Animator animator;
    
    //Test
    public Rigidbody rb;
    public float moveSpeed;

    //Variables
    Vector2 currentMovementInput;
    Vector3 currentMovement;
    bool isMovementPressed;
    float rotationFactorPerFrame = 1.0f;

    private void Awake() {
        playerInput = new PlayerInput();

        characterController = GetComponent<CharacterController>();

        animator = GetComponent<Animator>();

        playerInput.CharacterControls.Move.started += onMovementInput;

        playerInput.CharacterControls.Move.canceled += onMovementInput;
    }

    private void onMovementInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x;
        currentMovement.z = currentMovementInput.y;
        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
    }

    private void animationHandle()
    {
        bool isRunning = animator.GetBool("isRunning");

        if(isMovementPressed && !isRunning)
        {
            animator.SetBool("isRunning", true);
        }
        else if(!isMovementPressed && isRunning)
        {
            animator.SetBool("isRunning", false);
        }
    }

    private void RotationHandle()
    {
        Vector3 positionToLookAt;

        positionToLookAt.x = currentMovement.x;
        positionToLookAt.y = 0.0f;
        positionToLookAt.z = currentMovement.z;

        Quaternion currentRotation = transform.rotation;

        Debug.Log("Rotation" + currentRotation);

        if(isMovementPressed)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);

            Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame);
        }
    }

    private void Update() {
        characterController.Move(currentMovement * Time.deltaTime);

        Debug.Log("Movement" + currentMovement);

        animationHandle();

        RotationHandle();
    }

    // private void FixedUpdate() {
    //     if(Input.GetKeyDown("W"))
    //     {
    //         rb.velocity = Vector3.forward * moveSpeed * Time.deltaTime;   
    //     }
    //     else if(Input.GetKeyDown("S"))
    //     {
    //         rb.velocity = Vector3.back * moveSpeed * Time.deltaTime;   
    //     }
    //     else if(Input.GetKeyDown("D"))
    //     {
    //         rb.velocity = Vector3.right * moveSpeed * Time.deltaTime;   
    //     }
    //     else if(Input.GetKeyDown("A"))
    //     {
    //         rb.velocity = Vector3.left * moveSpeed * Time.deltaTime;   
    //     }
    // }

    private void OnEnable() {
        playerInput.CharacterControls.Enable();
    }

    private void OnDisable() {
        playerInput.CharacterControls.Disable();
    }
}
