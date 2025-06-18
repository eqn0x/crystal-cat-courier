using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField]private float moveSpeed = 5f;
    private Rigidbody2D _rb;
    private Vector2 moveInput;
    private Vector2 animationVector;
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _crosshair;
    private bool _is_walking;
    private bool _is_walking_backwards;


    private Animator _animator;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        Cursor.visible = false;
    }

    void Update()
    {
        _rb.velocity = moveInput * moveSpeed * (_is_walking_backwards ? 0.8f : 1);
        
    }

    public void Move(InputAction.CallbackContext context)
    {
        _is_walking = true;

        if (context.canceled)
        {
            _is_walking = false;
        }

        _animator.SetBool("isWalking", _is_walking);
        moveInput = context.ReadValue<Vector2>();



        UpdateParameters();
    }

    public void Look(InputAction.CallbackContext context)
    {
        _crosshair.SetPositionAndRotation(Mouse.current.position.ReadValue(), Quaternion.identity);
        animationVector = _camera.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - new Vector3(_rb.position.x, _rb.position.y, 0f);

        animationVector.Normalize();
        float angle = Vector2.Angle(Vector2.right, animationVector);

        animationVector = GetAnimationVectorByCrosshairAngle(angle);



        UpdateParameters();
    }

    private void UpdateParameters()
    {
        _is_walking_backwards = Vector2.Dot(moveInput, animationVector) < 0;
        _animator.SetFloat("isWalkingBackwards", _is_walking_backwards ? 1f : 0f);

        if (_is_walking)
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
