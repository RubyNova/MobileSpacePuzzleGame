using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float startSpeed = 10f;

    private float speed;
    
    [SerializeField] float startHealth = 100;
    private float health;
    
    [SerializeField] GameObject deathEffect;

    [Header("Unity Stuff")]
    [SerializeField] Image healthBar;

    private bool _isDead = false;


    void Start()
    {
        speed = startSpeed;
        health = startHealth;
    }

    public void TakeDamage (float amount)
    {
        health -= amount;

        healthBar.fillAmount = health / startHealth;

        if (health <= 0 && !_isDead)
        {
            Die();
        }
    }

    void Die ()
    {
        _isDead = true;

        GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(effect, 1f);

        WaveSpawner.EnemiesAlive--;

        Destroy(gameObject);
    }
    
    
}
