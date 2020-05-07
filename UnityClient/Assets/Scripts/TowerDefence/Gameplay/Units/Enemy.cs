using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Serialization;

public class Enemy : MonoBehaviour
{
    [Header("Attributes")] 
    [FormerlySerializedAs("_startHealth")] [SerializeField] private float startHealth;
    private float health; // Keeps track of enemies current Health
    [FormerlySerializedAs("_speed")] [SerializeField] private float startSpeed;
    private float speed; // Keep track of enemies current Speed
    [FormerlySerializedAs("_fireResistance")] [SerializeField] [Range(0f, 1f)] private float fireResist;
    [FormerlySerializedAs("_physicalResistance")] [SerializeField] [Range(0f, 1f)] private float physicalResist;
    [Tooltip("Includes Ice Damage")]
    [FormerlySerializedAs("_physicalResistance")] [SerializeField] [Range(0f, 1f)] private float magicResist;
    
    
    [Header("Rewards")] 
    [FormerlySerializedAs("_worth")] [SerializeField] private int worth;

    [Header("UI")]
    [FormerlySerializedAs("_healthBar")] [SerializeField] private Image healthBar;
    
    [Header("OnDeath")]
    [FormerlySerializedAs("_deathEffect")] [SerializeField] private GameObject deathEffect;

    private void Awake()
    { // Set Up Attributes
        health = startHealth;
        speed = startSpeed;
    }

    // Attributes
    public float StartHealth => startHealth;
    public float StartSpeed => startSpeed;

    public float Health
    {
        get => health;
        set => health = value;
    }
    public float Speed
    {
        get => speed;
        set => speed = value;
    }
    
    // Resistances
    public float FireResist
    {
        get => fireResist;
        set => fireResist = value;
    }
    
    public float PhysicalResist
    {
        get => physicalResist;
        set => physicalResist = value;
    }
    
    public float MagicResist
    {
        get => magicResist;
        set => magicResist = value;
    }

    // Rewards
    public int Worth => worth;

    // UI
    public Image HealthBar => healthBar;
    
    // OnDeath
    public GameObject DeathEffect => deathEffect;
}
