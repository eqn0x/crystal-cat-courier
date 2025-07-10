using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    // tmp projectile stuff
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float projectileSpeed = 4f;


    [SerializeField] float attacksPerSecond = 2;
    private Vector2 attackVector;

    private Coroutine attackCoroutine;
    private float nextAttackTime;

    public void SetLookData(in Vector2 lookInput)
    {
        attackVector = lookInput.normalized;
    }

    private void FireProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.FromToRotation(Vector2.right, attackVector), transform);
        projectile.GetComponent<Rigidbody2D>().velocity = attackVector * projectileSpeed;
    }

    public void StartAttacking()
    {
        if (attackCoroutine == null)
        {
            attackCoroutine = StartCoroutine(AttackContinuously());
        }
    }

    public void StopAttacking()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }
    }

    private IEnumerator AttackContinuously()
    {
        while (true)
        {
            TryAttack();
            yield return null;
        }

    }

    private void TryAttack()
    {
        if (Time.time >= nextAttackTime)
        {
            FireProjectile();
            nextAttackTime = Time.time + 1f / attacksPerSecond;
        }
    }
}
