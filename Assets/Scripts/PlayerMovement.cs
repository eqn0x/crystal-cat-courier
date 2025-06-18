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
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _crosshair;


    private Animator _animator;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        Cursor.visible = false;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _rb.velocity = moveInput * moveSpeed;
    }

    public void Move(InputAction.CallbackContext context)
    {
        _animator.SetBool("isWalking", true);

        if (context.canceled)
        {
            _animator.SetBool("isWalking", false);
            _animator.SetFloat("LastInputX", moveInput.x);
            _animator.SetFloat("LastInputY", moveInput.y);

        }

        moveInput = context.ReadValue<Vector2>();
        _animator.SetFloat("InputX", moveInput.x);
        _animator.SetFloat("InputY", moveInput.y);

    }

    public void Look(InputAction.CallbackContext context)
    {
        _crosshair.SetPositionAndRotation(Mouse.current.position.ReadValue(), Quaternion.identity);
    }
}
