using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class Level
{
    [Header("Level")]
    [FormerlySerializedAs("_spawnPoint")] [SerializeField] private Transform spawnPoint;
    [FormerlySerializedAs("_target")] [SerializeField] private GameObject target;
    [FormerlySerializedAs("_levelName")] [SerializeField] private String levelName; 
    [Range(0, 100)]
    [FormerlySerializedAs("_timeBetweenWaves")] [SerializeField] private float timeBetweenWaves; 
    [FormerlySerializedAs("_waveRef")] [SerializeField] private Wave[] waves; 
    
    // Level Attributes
    public Transform SpawnPoint => spawnPoint;
    public GameObject Target => target;
    public String LevelName => levelName;
    public Wave[] Waves => waves;
    public float TimeBetweenWaves => timeBetweenWaves;
}
