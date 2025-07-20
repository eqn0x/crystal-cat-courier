using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform crosshair;
    private AttackController attackController;
    private PlayerAnimationController animationController;
    private MovementController movementController;
    private InputController inputController;
    private CharacterController characterController;

    private void Awake()
    {
        inputController = GetComponent<InputController>();
        movementController = GetComponent<MovementController>();
        attackController = GetComponent<AttackController>();
        animationController = GetComponent<PlayerAnimationController>();
        characterController = GetComponent<CharacterController>();
    }
    private void OnEnable()
    {
        inputController.AttackStarted += attackController.StartAttacking;
        inputController.AttackCanceled += attackController.StopAttacking;

        inputController.AttackStarted += characterController.OnAttackPerformed;
        inputController.AttackCanceled += characterController.OnAttackCanceled;
        inputController.BlockStarted += characterController.OnBlockPerformed;
        inputController.BlockCanceled += characterController.OnBlockCanceled;
        inputController.DodgePerformed += characterController.OnDodgePerformed;
        inputController.SpecialPerformed += characterController.OnSpecialPerformed;

        inputController.InputDataChanged += HandleInputData;
    }

    private void OnDisable()
    {
        inputController.AttackStarted -= attackController.StartAttacking;
        inputController.AttackCanceled -= attackController.StopAttacking;
        
        inputController.AttackStarted -= characterController.OnAttackPerformed;
        inputController.AttackCanceled -= characterController.OnAttackCanceled;
        inputController.BlockStarted -= characterController.OnBlockPerformed;
        inputController.BlockCanceled -= characterController.OnBlockCanceled;
        inputController.DodgePerformed -= characterController.OnDodgePerformed;
        inputController.SpecialPerformed -= characterController.OnSpecialPerformed;

        inputController.InputDataChanged -= HandleInputData;
    }

    private void HandleInputData(InputData data)
    {
        crosshair.SetPositionAndRotation(Mouse.current.position.ReadValue(), Quaternion.identity);

        movementController.SetMovementData(data.moveInput, data.isMovingBackwards);
        animationController.SetMovementData(data.isMoving, data.isMovingBackwards, data.moveInput, data.isAttacking);
        attackController.SetLookData(data.lookInput);
    }
}
