using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(BallController))]

public class BallInput : MonoBehaviour
{
    public BallController ballController;
    public Joystick virtualJoystick;
    public InputActionAsset inputActions;

    private InputAction moveAction;
    private InputAction jumpAction;

    private Vector2 moveInput;
    private bool jumpPressed;

    private void Awake()
    {
        moveAction = inputActions.FindAction("Move");
        jumpAction = inputActions.FindAction("Jump");

        moveAction.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        //print(moveInput);
        moveAction.canceled += ctx => moveInput = Vector2.zero;
        //print(moveInput);
        jumpAction.performed += ctx => jumpPressed = true;
        //print(jumpAction);
    }

    private void OnEnable()
    {
        moveAction.Enable();
        jumpAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        jumpAction.Disable();
    }

    private void FixedUpdate()
    {
        // Combina la entrada del InputAction y la del joystick virtual (si existe)
        Vector2 finalInput = moveInput;
        if (virtualJoystick != null)
        {
            //finalInput += virtualJoystick.InputVector;
            finalInput = Vector2.ClampMagnitude(finalInput, 1f);
        }
        ballController.Move(finalInput);
        print(finalInput);
        if (jumpPressed)
        {
            ballController.Jump();
            jumpPressed = false;
        }
    }
}
