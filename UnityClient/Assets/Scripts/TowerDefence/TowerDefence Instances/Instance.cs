using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class Instance
{
    [SerializeField] Transform spawnPoint;
    [SerializeField] GameObject target;
    [SerializeField] String instanceName;
    
    public Transform SpawnPoint
    {
        get => spawnPoint;
        set => spawnPoint = value;
    }

    public GameObject Target
    {
        get => target;
        set => target = value;
    }
    
    public String InstanceName
    {
        get => instanceName;
        set => instanceName = value;
    }
}
