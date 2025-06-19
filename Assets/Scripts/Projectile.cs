using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private int health, maxHealth = 1;
    [SerializeField] private float maxDistance = 10;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        maxDistance -= rb.velocity.magnitude * Time.deltaTime;
        if (maxDistance <= 0)
            GameObject.Destroy(gameObject);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent<Enemy>(out Enemy enemyComponent))
        {
            enemyComponent.TakeDamage(damage);
            health--;
            damage -= damage / 2;
            if (health == 0)
                Destroy(gameObject);
        }
       
    }
}
