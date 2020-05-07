using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class Wave
{
    [Header("Wave")] 
    [FormerlySerializedAs("_compositionRef")] [SerializeField] private Composition[] compositions; 
    private readonly float amountOfWaves;

    // Wave Attributes
    public float AmountOfWaves { get; set; } 
    public Composition[] Compositions=> compositions;
}
