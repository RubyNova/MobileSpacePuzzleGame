using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


[Serializable]
public class Wave
{
    [SerializeField] GameObject enemy;
    [SerializeField] int count;
    [SerializeField] float rate;

    public GameObject Enemy
    {
        get { return enemy; }
    }

    public int Count
    {
        get { return count; }
    }

    public float Rate
    {
        get { return rate; }
    }


}
