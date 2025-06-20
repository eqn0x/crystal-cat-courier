using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    private PlayerInput playerInput;
    private InputAction fireAction;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Transform crosshair;
    [SerializeField] private float projectileSpeed = 4f;
    [SerializeField] private int attackPerSecond = 2;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 animationVector;
    private Vector2 lookVector;
    private bool isWalking;
    private bool isShooting;
    private bool isWalkingBackwards;



    private Coroutine shootingCoroutine;
    

    [SerializeField] private GameObject projectilePrefab;


    private Animator _animator;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        fireAction = playerInput.actions["Fire"];
        rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        Cursor.visible = false;
    }
    private void OnEnable()
    {
        fireAction.performed += OnFirePerformed;
        fireAction.canceled += OnFireCanceled;
    }

    private void OnDisable()
    {
        fireAction.performed -= OnFirePerformed;
        fireAction.canceled -= OnFireCanceled;
    }

    void Update()
    {
        rb.velocity = moveInput * moveSpeed * (isWalkingBackwards ? 0.8f : 1);
    }

    private void OnFirePerformed(InputAction.CallbackContext context)
    {
        isShooting = true;
        shootingCoroutine = StartCoroutine(FireContinuously());
    }

    private void OnFireCanceled(InputAction.CallbackContext context)
    {
        if (isShooting == true)
            new WaitForSeconds(1);
        isShooting = false;
        if (shootingCoroutine != null)
        {
            StopCoroutine(shootingCoroutine);
        }

    }

    public void Move(InputAction.CallbackContext context)
    {
        isWalking = true;

        if (context.canceled)
        {
            isWalking = false;
        }

        _animator.SetBool("isWalking", isWalking);
        moveInput = context.ReadValue<Vector2>();



        UpdateParameters();
    }

    public void Look(InputAction.CallbackContext context)
    {
        crosshair.SetPositionAndRotation(Mouse.current.position.ReadValue(), Quaternion.identity);
        animationVector = playerCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - new Vector3(rb.position.x, rb.position.y, 0f);

        animationVector.Normalize();
        float angle = Vector2.Angle(Vector2.right, animationVector);
        lookVector = animationVector.normalized;
        animationVector = GetAnimationVectorByCrosshairAngle(angle);



        UpdateParameters();
    }

    private IEnumerator FireContinuously()
    {
        while (true)
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.FromToRotation(Vector2.right, lookVector));
            projectile.GetComponent<Rigidbody2D>().velocity = lookVector * projectileSpeed;
            yield return new WaitForSeconds(1f / attackPerSecond);
        }
    }

    private void UpdateParameters()
    {
        isWalkingBackwards = Vector2.Dot(moveInput, animationVector) < 0;
        _animator.SetFloat("isWalkingBackwards", isWalkingBackwards ? 1f : 0f);

        if (isWalking || isShooting)
        {
            _animator.SetFloat("InputX", animationVector.x);
            _animator.SetFloat("InputY", animationVector.y);
        }
    }

    private Vector2 GetAnimationVectorByCrosshairAngle(float angle)
    {
        // note that animations are divided in 8 states
        if (angle < 45f / 2)
            return new Vector2(1f, 0f);
        else if (angle < 45f * 3 / 2)
            if (animationVector.y > 0)
                return new Vector2(1f, 1f);
            else
                return new Vector2(1f, -1f);
        else if(angle < 45f * 5 / 2)
            if (animationVector.y > 0)
                return new Vector2(0f, 1f);
            else
                return new Vector2(0f, -1f);
        else if (angle < 45f * 7 / 2)
            if (animationVector.y > 0)
                return new Vector2(-1f, 1f);
            else
                return new Vector2(-1f, -1f);
        else 
            return new Vector2(-1f, 0f);
    }
}
