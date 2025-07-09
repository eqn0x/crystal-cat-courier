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

    // movement
    [SerializeField] private float moveSpeed = 5f;
    private Vector2 moveInput;
    private bool isMoving;
    private bool isMovingBackwards;
    private Rigidbody2D rb;

    // look
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Transform crosshair;
    private Vector2 lookVector;

    // attack
    [SerializeField] private int attackPerSecond = 2;
    private bool isShooting;

    // animation
    private Vector2 animationVector;

    private Coroutine shootingCoroutine;
    
    [SerializeField] private ProjectileController projectileController; // refactor to attack controller
    [SerializeField] private PlayerAnimationController animationController; // move all animation there
    [SerializeField] private MovementController movementController; // move all movement there
    [SerializeField] private InputController inputController;


    private Animator _animator;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        attackAction = playerInput.actions["Attack"];

        rb = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
        _animator.speed = 3;

        Cursor.visible = false;
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

    void Update()
    {
        _animator.speed = 3 * (isMovingBackwards ? 0.8f : 1);
    }

    private void FixedUpdate()
    {
        float walking_multiplier = (isMovingBackwards ? 0.8f : 1);
        Vector2 current_velocity = new Vector2(Mathf.Lerp(rb.velocity.x, moveInput.x * moveSpeed * walking_multiplier, 0.3f), Mathf.Lerp(rb.velocity.y, moveInput.y * moveSpeed * walking_multiplier, 0.3f));

        rb.velocity = current_velocity;
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
        isMoving = data.isMoving;
        moveInput = data.moveInput;
        isMovingBackwards = data.isMovingBackwards;
        animationVector = GetAnimationVectorByCrosshairAngle(Vector2.Angle(Vector2.right, moveInput));
        Debug.Log(Vector2.Angle(Vector2.right, moveInput));
        Debug.Log(animationVector);
        lookVector = data.lookInput;
        crosshair.SetPositionAndRotation(Mouse.current.position.ReadValue(), Quaternion.identity);
        UpdateParameters();
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

    private void UpdateParameters()
    {
        _animator.SetBool("isMoving", isMoving);
        _animator.SetFloat("isMovingBackwards", isMovingBackwards ? 1f : 0f);

        if (isMoving || isShooting)
        {
            Debug.Log(animationVector);
            _animator.SetFloat("InputX", isMovingBackwards ? -animationVector.x : animationVector.x);
            _animator.SetFloat("InputY", isMovingBackwards ? -animationVector.y : animationVector.y);
        }
    }

    private Vector2 GetAnimationVectorByCrosshairAngle(float angle)
    {
        // note that animations are divided in 8 states
        if (angle < 45f / 2)
            return new Vector2(1f, 0f);
        else if (angle < 45f * 3 / 2)
            if (moveInput.y > 0)
                return new Vector2(1f, 1f);
            else
                return new Vector2(1f, -1f);
        else if(angle < 45f * 5 / 2)
            if (moveInput.y > 0)
                return new Vector2(0f, 1f);
            else
                return new Vector2(0f, -1f);
        else if (angle < 45f * 7 / 2)
            if (moveInput.y > 0)
                return new Vector2(-1f, 1f);
            else
                return new Vector2(-1f, -1f);
        else 
            return new Vector2(-1f, 0f);
    }
}
