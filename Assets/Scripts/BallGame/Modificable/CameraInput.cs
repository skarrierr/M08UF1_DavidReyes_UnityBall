using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CameraController))]
public class CameraInput : MonoBehaviour
{
    public CameraController cameraController;
    public InputActionAsset inputActions;

    private InputAction zoomAction;
    private InputAction rotateAction;
    private InputAction switchTargetAction;

    private float rotationInput;
    private float zoomInput;    

    private void Awake()
    {
        zoomAction = inputActions.FindAction("Zoom");
        rotateAction = inputActions.FindAction("Rotate");
        switchTargetAction = inputActions.FindAction("SwitchTarget");

        zoomAction.performed += ctx => zoomInput = ctx.ReadValue<Vector2>().y;
        zoomAction.canceled += ctx => zoomInput = 0;

        rotateAction.performed += ctx => rotationInput = ctx.ReadValue<Vector2>().x;    
        rotateAction.canceled += ctx => rotationInput = 0;

        switchTargetAction.performed += ctx => SwitchCameraTarget();
    }

    private void Update()
    {
        
        cameraController.Zoom(zoomInput * Time.deltaTime * 50);

        //print("zoomInput" + zoomInput);

        
        cameraController.Rotate(rotationInput * Time.deltaTime * 100);
        //print("rotationInput" + rotationInput);
    }

    private void SwitchCameraTarget()
    {
        // Cambia entre los objetivos definidos en el CameraController
        if (cameraController.target == CameraController.Target.Ball)
            cameraController.SwitchTargetTarget();
        else if (cameraController.target == CameraController.Target.Target)
            cameraController.SwitchTargetMiddlepoint();
        else
            cameraController.SwitchTargetBall();
    }

    private void OnEnable()
    {
        zoomAction.Enable();
        rotateAction.Enable();
        switchTargetAction.Enable();
    }

    private void OnDisable()
    {
        zoomAction.Disable();
        rotateAction.Disable();
        switchTargetAction.Disable();
    }
}
