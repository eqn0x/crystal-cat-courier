using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // input stuff
    private PlayerInput playerInput;
    private InputAction attackAction;

    // look
    [SerializeField] private Transform crosshair;
    private Vector2 lookVector;

    // attack
    [SerializeField] private int attackPerSecond = 2;
    private bool isShooting;

    private Coroutine shootingCoroutine;
    
    private ProjectileController projectileController; // refactor to attackController
    private PlayerAnimationController animationController;
    private MovementController movementController;
    private InputController inputController;


    private void Awake()
    {
        projectileController = GetComponentInChildren<ProjectileController>();
        animationController = GetComponent<PlayerAnimationController>();
        movementController = GetComponent<MovementController>();
        inputController = GetComponent<InputController>();

        playerInput = GetComponent<PlayerInput>();
        attackAction = playerInput.actions["Attack"];
    }
    private void OnEnable()
    {
        attackAction.performed += OnAttackPerformed;
        attackAction.canceled += OnAttackCanceled;

        inputController.OnInputDataChanged += HandleInputData;
    }

    private void OnDisable()
    {
        attackAction.performed -= OnAttackPerformed;
        attackAction.canceled -= OnAttackCanceled;

        inputController.OnInputDataChanged -= HandleInputData;
    }

    private void OnAttackPerformed(InputAction.CallbackContext context)
    {
        isShooting = true;
        shootingCoroutine = StartCoroutine(FireContinuously());
    }

    private void OnAttackCanceled(InputAction.CallbackContext context)
    {
        if (isShooting == true)
            new WaitForSeconds(1);
        isShooting = false;
        if (shootingCoroutine != null)
        {
            StopCoroutine(shootingCoroutine);
        }
    }

    private void HandleInputData(InputData data)
    {
        lookVector = data.lookInput;
        crosshair.SetPositionAndRotation(Mouse.current.position.ReadValue(), Quaternion.identity);

        movementController.SetMovementData(data.moveInput, data.isMovingBackwards);
        animationController.SetMovementData(data.isMoving, data.isMovingBackwards, data.moveInput, isShooting);
    }

    private IEnumerator FireContinuously()
    {
        while (true)
        {
            projectileController.SetProjectileSpawn(transform.position, lookVector);
            projectileController.FireProjectile();
            yield return new WaitForSeconds(1f / attackPerSecond);
        }
    }
}
