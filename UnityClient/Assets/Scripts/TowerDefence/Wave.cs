using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Serialization;


[Serializable]
public class Wave
{
    [FormerlySerializedAs("enemy")] [SerializeField] private GameObject _enemy;
    [FormerlySerializedAs("count")] [SerializeField] private int _count;
    [FormerlySerializedAs("rate")] [SerializeField] private float _rate;

    public GameObject Enemy
    {
        get { return _enemy; }
    }

    public int Count
    {
        get { return _count; }
    }

    public float Rate
    {
        get { return _rate; }
    }


}
