﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Serialization;

public class Enemy : MonoBehaviour
{
    private float health;
    private WaveSpawner _controllingSpawner;
    private bool _isDead; 
   
    [Header("Attributes")]
    [FormerlySerializedAs("start Health")] [SerializeField] private float _startHealth = 100;
    [FormerlySerializedAs("Movement Speed")] [SerializeField] private float _speed = 100;
    
    [Header("Unity Stuff")]
    [FormerlySerializedAs("Health Bar")] [SerializeField] private Image _healthBar;
    [FormerlySerializedAs("Death Effect")] [SerializeField] private GameObject _deathEffect;
    
    public void Init(WaveSpawner controllingSpawner) => _controllingSpawner = controllingSpawner;

    void Start()
    {
        health = _startHealth;

    }

    public void TakeDamage (float amount)
    {
        health -= amount;

        _healthBar.fillAmount = health / _startHealth;

        if (health <= 0 && !_isDead)
        {
            Die();
        }
    }

    void Die ()
    {
        _isDead = true;

        GameObject effect = Instantiate(_deathEffect, transform.position, Quaternion.identity);
        Destroy(effect, 1f);

        _controllingSpawner.EnemiesAlive--;

        Destroy(gameObject);
    }
    
    
}
