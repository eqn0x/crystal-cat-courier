using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private float accelerationFactor = 0.5f;
    [SerializeField] private float moveSpeed = 1;
    [SerializeField] private float backwardsMovementFactor = 0.8f;

    private Vector2 moveInput;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        Vector2 current_velocity = new Vector2(Mathf.Lerp(_rb.velocity.x, moveInput.x * moveSpeed , 0.3f), Mathf.Lerp(_rb.velocity.y, moveInput.y * moveSpeed, accelerationFactor));

        _rb.velocity = current_velocity;
    }
    public void SetMovementData(in Vector2 movementDirection, in bool isMovingBackwards)
    {
        moveInput = movementDirection * (isMovingBackwards ? backwardsMovementFactor : 1f);
    }

    public float GetBackwardsMovementFactor()
    {
        return backwardsMovementFactor;
    }
}
