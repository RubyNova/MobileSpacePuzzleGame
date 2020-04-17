using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Serialization;

public class Enemy : MonoBehaviour
{
    [Header("Attributes")] 
    [FormerlySerializedAs("_startHealth")] [SerializeField] private float startHealth;
    [FormerlySerializedAs("_speed")] [SerializeField] private float speed;
    [FormerlySerializedAs("_fireResistance")] [SerializeField] [Range(0, 100)] private int fireResist;
    [FormerlySerializedAs("_physicalResistance")] [SerializeField] [Range(0, 100)] private int physicalResist;
    
    [Header("UI")]
    [FormerlySerializedAs("_healthBar")] [SerializeField] private Image healthBar;
    
    [Header("OnDeath")]
    [FormerlySerializedAs("_drops")] [SerializeField] private GameObject[] drops;
    [FormerlySerializedAs("_deathEffect")] [SerializeField] private GameObject deathEffect;

    // Attributes
    public float StartHealth => startHealth;
    public float Speed
    {
        get => speed;
        set => speed = value;
    }

    public int FireResist => fireResist;
    public int PhysicalResist => physicalResist;
    
    // UI
    public Image HealthBar => healthBar;
    
    // OnDeath
    public GameObject[] Drops => drops;
    public GameObject DeathEffect => deathEffect;
}
