using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public struct InputData
{
    public Vector2 moveInput { get; set; }
    public bool isMoving { get; set; }
    public bool isMovingBackwards { get; set; }
    public Vector2 lookInput { get; set; }
    public float lookAngle { get; set; }
}
public class InputController : MonoBehaviour
{

    [SerializeField] private Camera _playerCamera;

    private PlayerInput playerInput;
    private InputData inputData;

    private InputAction attackAction;


    public event Action<InputData> OnInputDataChanged;
    public event Action OnAttackPerformed;

    private void Awake()
    {
        //playerInput = GetComponent<PlayerInput>();
        //attackAction = playerInput.actions["Attack"];

        Cursor.visible = false;
    }
    public void Move(InputAction.CallbackContext context)
    {
        inputData.isMoving = true;
        if (context.canceled)
        {
            inputData.isMoving = false;
        }

        inputData.moveInput = context.ReadValue<Vector2>();
        inputData.isMovingBackwards = Vector2.Angle(inputData.moveInput, inputData.lookInput) > 90f;
        OnInputDataChanged?.Invoke(inputData);
    }
    public void Look(InputAction.CallbackContext context)
    {
        //crosshair.SetPositionAndRotation(Mouse.current.position.ReadValue(), Quaternion.identity); //ui stuff
        Vector3 mousePos = Mouse.current.position.ReadValue();
        Vector3 worldPos = _playerCamera.ScreenToWorldPoint(mousePos);

        inputData.lookInput = (worldPos - transform.position).normalized;
        inputData.lookAngle = Vector2.Angle(Vector2.right, inputData.lookInput);

        inputData.isMovingBackwards = Vector2.Angle(inputData.moveInput, inputData.lookInput) > 90f;
        //animationVector = GetAnimationVectorByCrosshairAngle(angle);

        OnInputDataChanged?.Invoke(inputData);
    }
}
