using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class TestController : MonoBehaviour
{
    public float Velocity;
    [Space]

    public float InputX;
    public float InputZ;
    public Vector3 desiredMoveDirection;
    public bool blockRotationPlayer;
    public float desiredRotationSpeed = 0.1f;
    // public Animator anim;
    public float Speed;
    public float allowPlayerRotation = 0.1f;
    public CharacterController controller;
    public bool isGrounded;
    public float verticalVel;
    private Vector3 moveVector;
    public Camera cam;

    private void Start() {
        // anim = this.GetComponent<Animator>();

        cam = Camera.main;

        controller = this.GetComponent<CharacterController>();
    }

    private void Update() {
        InputMagnitude ();

        isGrounded = controller.isGrounded;
        if (isGrounded)
        {
            verticalVel -= 0;
        }
        else
        {
            verticalVel -= 1;
        }
        moveVector = new Vector3(0, verticalVel * .2f * Time.deltaTime, 0);
        controller.Move(moveVector);
        
    }

    void PlayerMoveAndRotation()
    {
        InputX = Input.GetAxis ("Horizontal");
		InputZ = Input.GetAxis ("Vertical");

        var camera = Camera.main;
		var forward = cam.transform.forward;
		var right = cam.transform.right;

		forward.y = 0f;
		right.y = 0f;

		forward.Normalize ();
		right.Normalize ();

		desiredMoveDirection = forward * InputZ + right * InputX;

        if (blockRotationPlayer == false) {
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (desiredMoveDirection), desiredRotationSpeed);
            controller.Move(desiredMoveDirection * Time.deltaTime * Velocity);
		}
    }

    void InputMagnitude() {
		//Calculate Input Vectors
		InputX = Input.GetAxis ("Horizontal");
		InputZ = Input.GetAxis ("Vertical");

		//anim.SetFloat ("InputZ", InputZ, VerticalAnimTime, Time.deltaTime * 2f);
		//anim.SetFloat ("InputX", InputX, HorizontalAnimSmoothTime, Time.deltaTime * 2f);

		//Calculate the Input Magnitude
		Speed = new Vector2(InputX, InputZ).sqrMagnitude;

        //Physically move player

        // bool isRunning = anim.GetBool("isRunning");
		if (Speed > allowPlayerRotation) {
            // anim.SetBool("isRunning", true);
			PlayerMoveAndRotation ();
		} else if (Speed < allowPlayerRotation) {
			// anim.SetBool("isRunning", false);
		}
	}
}
