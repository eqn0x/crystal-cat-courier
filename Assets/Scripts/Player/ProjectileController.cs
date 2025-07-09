using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float projectileSpeed = 4f;
    private Vector3 spawnRotation;

    public void SetProjectileSpawn(in Vector3 spawnTransformLocal, in Vector3 spawnRotationLocal)
    {
        spawnRotation = spawnRotationLocal;
    }

    public void FireProjectile()
    {
        //GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.FromToRotation(Vector2.right, spawnRotation));
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.FromToRotation(Vector2.right, spawnRotation), transform);
        projectile.GetComponent<Rigidbody2D>().velocity = spawnRotation * projectileSpeed;
    }
}
