using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float projectileSpeed = 4f;
    private Vector3 spawnTransform;
    private Vector3 spawnRotation;




    public void SetProjectileSpawn(Vector3 spawnTransformLocal, Vector3 spawnRotationLocal)
    {
        spawnTransform = spawnTransformLocal;
        spawnRotation = spawnRotationLocal;
    }

    public void FireProjectile()
    {
        //GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.FromToRotation(Vector2.right, spawnRotation));
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.FromToRotation(Vector2.right, spawnRotation), transform);
        projectile.GetComponent<Rigidbody2D>().velocity = spawnRotation * projectileSpeed;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
