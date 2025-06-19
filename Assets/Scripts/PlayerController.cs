using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [SerializeField]private float moveSpeed = 5f;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Transform crosshair;
    [SerializeField] private float projectileSpeed = 4f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 animationVector;
    private Vector2 lookVector;
    private bool isWalking;
    private bool isWalkingBackwards;
    

    [SerializeField] private GameObject projectilePrefab;


    private Animator _animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        Cursor.visible = false;
    }

    void Update()
    {
        rb.velocity = moveInput * moveSpeed * (isWalkingBackwards ? 0.8f : 1);
        
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

    public void Fire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.FromToRotation(Vector2.right, lookVector));
            projectile.GetComponent<Rigidbody2D>().velocity = lookVector * projectileSpeed;

        }

    }

    private void UpdateParameters()
    {
        isWalkingBackwards = Vector2.Dot(moveInput, animationVector) < 0;
        _animator.SetFloat("isWalkingBackwards", isWalkingBackwards ? 1f : 0f);

        if (isWalking)
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
