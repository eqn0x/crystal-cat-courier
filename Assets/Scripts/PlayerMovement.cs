using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField]private float moveSpeed = 5f;
    private Rigidbody2D _rb;
    private Vector2 moveInput;
    private Animator _animator;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
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
}
