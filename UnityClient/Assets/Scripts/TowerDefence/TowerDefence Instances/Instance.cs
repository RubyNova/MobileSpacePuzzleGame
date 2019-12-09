using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class Instance
{
    [FormerlySerializedAs("spawnPoint")] [SerializeField] private Transform _spawnPoint;
    [FormerlySerializedAs("target")] [SerializeField] private GameObject _target;
    [FormerlySerializedAs("instanceName")] [SerializeField] private String _instanceName;
    
    public Transform SpawnPoint
    {
        get => _spawnPoint;
        set => _spawnPoint = value;
    }

    public GameObject Target
    {
        get => _target;
        set => _target = value;
    }
    
    public String InstanceName
    {
        get => _instanceName;
        set => _instanceName = value;
    }
}
