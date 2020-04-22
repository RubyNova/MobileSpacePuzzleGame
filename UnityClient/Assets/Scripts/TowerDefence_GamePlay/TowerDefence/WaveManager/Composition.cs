using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class Composition
{
    [Header("Composition")]
    [FormerlySerializedAs("_enemy")] [SerializeField] private GameObject enemyType;
    [FormerlySerializedAs("_amount")] [SerializeField] private int amount;
    [Range(.1f, 10f)]
    [FormerlySerializedAs("_spawnRate")] [SerializeField] private float spawnRate;
    [Range(.1f, 10f)]
    [FormerlySerializedAs("_nextDelay")] [SerializeField] private float nextDelay;

    // Composition Attributes
    public GameObject EnemyType => enemyType;
    public int Amount => amount;
    public float SpawnRate => spawnRate;
    public float NextDelay => nextDelay;
}
