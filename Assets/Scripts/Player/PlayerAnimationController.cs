using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] float animatorSpeed = 3f;
    private Animator _animator;
    private MovementController movementController;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _animator.speed = animatorSpeed;
        movementController = GetComponent<MovementController>();
    }

    public void SetMovementData(in bool isMoving, in bool isMovingBackwards, in Vector2 moveInput, in bool isShooting)
    {
        Vector2 animationVector = GetAnimationVectorByCrosshairAngle(Vector2.Angle(Vector2.right, moveInput), moveInput.y);

        _animator.speed = animatorSpeed * (isMovingBackwards ? movementController.GetBackwardsMovementFactor() : 1f);

        _animator.SetBool("isMoving", isMoving);
        _animator.SetFloat("isMovingBackwards", isMovingBackwards ? 1f : 0f);

        if (isMoving || isShooting)
        {
            _animator.SetFloat("InputX", isMovingBackwards ? -animationVector.x : animationVector.x);
            _animator.SetFloat("InputY", isMovingBackwards ? -animationVector.y : animationVector.y);
        }
    }

    private Vector2 GetAnimationVectorByCrosshairAngle(in float angle, in float y)
    {
        // note that animations are divided in 8 states
        if (angle < 45f / 2)
            return new Vector2(1f, 0f);
        else if (angle < 45f * 3 / 2)
            if (y > 0)
                return new Vector2(1f, 1f);
            else
                return new Vector2(1f, -1f);
        else if (angle < 45f * 5 / 2)
            if (y > 0)
                return new Vector2(0f, 1f);
            else
                return new Vector2(0f, -1f);
        else if (angle < 45f * 7 / 2)
            if (y > 0)
                return new Vector2(-1f, 1f);
            else
                return new Vector2(-1f, -1f);
        else
            return new Vector2(-1f, 0f);
    }
}
