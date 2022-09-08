using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform playerModel;
    [SerializeField] PlayerTestBrick playerBrickController;
    [SerializeField] private Collider collider;
    [SerializeField] private float playerSpeed = 2.0f;
    

    private PlayerInput playerInput;
    private bool isMoveable;
    private bool isWin;
    private CharacterController controller;
    private Vector3 move;
    
    private void Awake() 
    {
        playerInput = new PlayerInput();
        controller = GetComponent<CharacterController>();

        isMoveable = true;
        isWin = false;
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
        if(isMoveable)
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

    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Finish"))
        {
            isMoveable = false;
            playerBrickController.DropAllBrick();
            animator.SetTrigger("Win");
            UIManager.Ins.OpenUI<CanvasVictory>(UICanvasID.Result).SetResult(isWin);
            FrameManager.Ins.ChangeState(GameState.Result);
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.collider.CompareTag("Player"))
        {
            BotBrickControll _playerBrickController = hit.collider.GetComponent<BotBrickControll>();

            if(_playerBrickController != null)
            {
                HitFall(_playerBrickController);
            }
        }
    }

    private void HitFall(BotBrickControll _playerBrickController)
    {
        int temp = playerBrickController.brickCount - _playerBrickController.brickCount;

        Debug.Log("Temp: " + temp);

        if(temp == 0)
        {
            return;
        }
        else if(temp < 0)
        {
            TriggerFall();
            playerBrickController.DropBrick();
        }
    }

    protected virtual void TriggerFall()
    {
        StartCoroutine(Fall());
    }

    private IEnumerator Fall()
    {
        isMoveable = false;
        collider.enabled = false;

        animator.SetTrigger("Fall");

        yield return new WaitForSeconds(4.9f);

        isMoveable = true;
        collider.enabled = true;
    }
}

