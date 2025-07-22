using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputData
{
    public Vector2 moveInput { get; set; }
    public bool isMoving { get; set; }
    public bool isMovingBackwards { get; set; }
    public Vector2 lookInput { get; set; }
    public bool isAttacking { get; set; }
    public bool isBlocking { get; set; }
}
public class InputController : MonoBehaviour
{

    [SerializeField] private Camera _playerCamera;
    private InputData inputData;

    public event Action<InputData> InputDataChanged;
    public event Action AttackStarted;
    public event Action AttackCanceled;
    public event Action BlockStarted;
    public event Action BlockCanceled;
    public event Action DodgePerformed;
    public event Action SpecialPerformed;

    

    private void DebugInputData()
    {
        //Debug.Log("moveInput = " + inputData.moveInput + "; isMoving = " + inputData.isMoving + "; isMovingBackwards = " + inputData.isMovingBackwards +
          //  "; lookInput = " + inputData.lookInput + "; isAttacking = " + inputData.isAttacking + "; isBlocking = " + inputData.isBlocking);
    }

    private void UpdateBackwardsMovement()
    {
        if (inputData.isAttacking || inputData.isBlocking)
            inputData.isMovingBackwards = Vector2.Angle(inputData.moveInput, inputData.lookInput) > 90f;
        else
            inputData.isMovingBackwards = false;
    }

    private void Awake()
    {
        Cursor.visible = false;
        inputData = new InputData();
    }
    public void Move(InputAction.CallbackContext context)
    {
        Debug.Log("Move");
        inputData.isMoving = true;
        if (context.canceled)
        {
            inputData.isMoving = false;
        }

        inputData.moveInput = context.ReadValue<Vector2>();
        UpdateBackwardsMovement();

        DebugInputData();
        InputDataChanged?.Invoke(inputData);
    }
    public void Look(InputAction.CallbackContext context)
    {
        Debug.Log("Look");

        Vector3 mousePos = Mouse.current.position.ReadValue();
        Vector3 worldPos = _playerCamera.ScreenToWorldPoint(mousePos);

        inputData.lookInput = (worldPos - transform.position).normalized;

        UpdateBackwardsMovement();
        DebugInputData();
        InputDataChanged?.Invoke(inputData);
    }

    public void OnAttackPerformed(InputAction.CallbackContext context)
    {

        if (context.performed)
        {
            inputData.isAttacking = true;
            Debug.Log("OnAttackPerformed");
            UpdateBackwardsMovement();
            DebugInputData();
            AttackStarted?.Invoke();
            InputDataChanged?.Invoke(inputData);
        }

    }
    public void OnAttackCanceled(InputAction.CallbackContext context)
    {

        if (context.canceled)
        {
            inputData.isAttacking = false;
            Debug.Log("OnAttackCanceled");
            UpdateBackwardsMovement();
            DebugInputData();
            AttackCanceled?.Invoke();
            InputDataChanged?.Invoke(inputData);
        }
    }

    public void OnBlockPerformed(InputAction.CallbackContext context)
    {

        if (context.performed)
        {
            inputData.isBlocking = true;
            Debug.Log("OnBlockPerformed");
            UpdateBackwardsMovement();
            DebugInputData();
            BlockStarted?.Invoke();
            InputDataChanged?.Invoke(inputData);
        }
    }

    public void OnBlockCanceled(InputAction.CallbackContext context)
    {

        if (context.canceled)
        {
            inputData.isBlocking = false;
            Debug.Log("OnBlockCanceled");
            UpdateBackwardsMovement();
            DebugInputData();
            BlockCanceled?.Invoke();
            InputDataChanged?.Invoke(inputData);
        }
    }

    public void OnDodge(InputAction.CallbackContext context)
    {

        inputData.isBlocking = false;
        inputData.isAttacking = false;
        if (context.performed)
        {
            Debug.Log("OnDodge");
            UpdateBackwardsMovement();
            DebugInputData();
            DodgePerformed?.Invoke();
            InputDataChanged?.Invoke(inputData);
        }
    }

    public void OnSpecial(InputAction.CallbackContext context)
    {

        inputData.isBlocking = false;
        inputData.isAttacking = false;
        if (context.performed)
        {
            Debug.Log("OnSpecial");
            UpdateBackwardsMovement();
            DebugInputData();
            SpecialPerformed?.Invoke();
            InputDataChanged?.Invoke(inputData);
        }
    }

    public Vector2 GetLookInput()
    {
        Debug.Log("GetLookInput");
        DebugInputData();
        return inputData.lookInput;
    }
}
