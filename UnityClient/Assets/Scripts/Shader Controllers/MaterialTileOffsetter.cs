using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MaterialTileOffsetter : MonoBehaviour
{
    [SerializeField] 
    private Material _target;

    [SerializeField] 
    private Vector2 _threshold;
    
    private Vector2 _initialOffset;

    private void Start() => _initialOffset = _target.mainTextureOffset;
    
    private void Update()
    {
        if (_target.mainTextureOffset.x > _threshold.x || _target.mainTextureOffset.y > _threshold.y)
        {
            _target.mainTextureOffset = _initialOffset;
        }
        else
        {
            _target.mainTextureOffset += new Vector2(Time.deltaTime, Time.deltaTime);
        }

    }

    private void OnApplicationQuit() => _target.mainTextureOffset = _initialOffset;
}
