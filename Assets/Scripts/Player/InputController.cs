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

    private void Awake()
    {
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

        InputDataChanged?.Invoke(inputData);
    }
    public void Look(InputAction.CallbackContext context)
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        Vector3 worldPos = _playerCamera.ScreenToWorldPoint(mousePos);

        inputData.lookInput = (worldPos - transform.position).normalized;

        inputData.isMovingBackwards = Vector2.Angle(inputData.moveInput, inputData.lookInput) > 90f;

        InputDataChanged?.Invoke(inputData);
    }

    public void OnAttackPerformed(InputAction.CallbackContext context) 
    {
        inputData.isAttacking = true;
        if (context.performed) AttackStarted?.Invoke();
        InputDataChanged?.Invoke(inputData);
    }

    public void OnAttackCanceled(InputAction.CallbackContext context)
    {
        inputData.isAttacking = false;
        if (context.canceled) AttackCanceled?.Invoke();
        InputDataChanged?.Invoke(inputData);
    }

    public void OnBlockPerformed(InputAction.CallbackContext context)
    {
        inputData.isBlocking = true;
        if (context.performed) BlockStarted?.Invoke();
        InputDataChanged?.Invoke(inputData);
    }

    public void OnBlockCanceled(InputAction.CallbackContext context)
    {
        inputData.isBlocking = false;
        if (context.canceled) BlockCanceled?.Invoke();
        InputDataChanged?.Invoke(inputData);
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        inputData.isBlocking = false;
        inputData.isAttacking = false;
        if (context.performed) DodgePerformed?.Invoke();
        InputDataChanged?.Invoke(inputData);
    }

    public void OnSpecial(InputAction.CallbackContext context)
    {
        inputData.isBlocking = false;
        inputData.isAttacking = false;
        if (context.performed) SpecialPerformed?.Invoke();
        InputDataChanged?.Invoke(inputData);
    }

    public Vector2 GetLookInput()
    {
        return inputData.lookInput;
    }
}
