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
        

        switchTargetAction.performed += ctx => SwitchCameraTarget();
    }

    private void Update()
    {
        
        cameraController.Zoom(zoomInput * Time.deltaTime * 50);


        
        cameraController.Rotate(rotationInput * Time.deltaTime * 12);
        print("rotationInput" + rotationInput);
    }
    
    private void SwitchCameraTarget()
    {
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
   
    public void ZoomInButtonPressed()
    {
        cameraController.Zoom(0.5f);
    }

    
    public void ZoomOutButtonPressed()
    {
        cameraController.Zoom(-0.5f);
    }

    public void RotateLeftButtonPressed()
    {
        cameraController.Rotate(-10f);
    }
    public void RotateRightButtonPressed()
    {
        cameraController.Rotate(10f);
    }
}
