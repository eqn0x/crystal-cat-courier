using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int health, maxHealth = 5;
    [SerializeField] Transform hpDisplay;
    void Start()
    {
        health = maxHealth;
    }



    public void TakeDamage(in int damage)
    {
        health -= damage;
        //Debug.Log(gameObject.name + ": damage taken = " + damage + ", health remaining = " + health);
        hpDisplay.localScale = new Vector3((float)health/maxHealth, hpDisplay.localScale.y, hpDisplay.localScale.z);
        if (health <= 0)
        {
            GameObject.Destroy(gameObject);
        }
    }
}
