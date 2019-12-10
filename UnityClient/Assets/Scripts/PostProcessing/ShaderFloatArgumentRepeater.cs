using System;
using System.Collections.Generic;
using UnityEngine;

namespace PostProcessing
{
    public class ShaderFloatArgumentRepeater : MonoBehaviour
    {
        [SerializeField]
        private List<ShaderFloatArgumentPair> _arguments;

        [SerializeField] 
        private Material _targetMaterial;

        [SerializeField] 
        private float _speedModifier = 1;

        private readonly Dictionary<int, ShaderFloatArgumentStatePair> _pushValues = new Dictionary<int, ShaderFloatArgumentStatePair>();
    
        // Start is called before the first frame update
        void Start()
        {
            foreach (var argument in _arguments)
            {
                _pushValues.Add(Shader.PropertyToID(argument.ShaderArgumentName), new ShaderFloatArgumentStatePair(argument, argument.MaxValue));
            }
        }

        // Update is called once per frame
        private void Update()
        {
            foreach (var pair in _pushValues)
            {
                pair.Value.CurrentState += (Time.deltaTime * _speedModifier);
            
                if (pair.Value.CurrentState > pair.Value.ArgumentData.MaxValue)
                {
                    pair.Value.CurrentState = pair.Value.ArgumentData.MinValue;
                }
            
                _targetMaterial.SetFloat(pair.Key, pair.Value.CurrentState);
            }
        }

        private void OnApplicationQuit()
        {
            foreach (var argument in _pushValues)
            {
                _targetMaterial.SetFloat(argument.Key, argument.Value.ArgumentData.MinValue);
            }
        }


        [Serializable]
        public class ShaderFloatArgumentPair
        {
            [SerializeField]
            private string _shaderArgumentName;
    
            [SerializeField]
            private float _minValue;
    
            [SerializeField]
            private float _maxValue;

            public string ShaderArgumentName => _shaderArgumentName;

            public float MinValue => _minValue;

            public float MaxValue => _maxValue;
        }

        private class ShaderFloatArgumentStatePair
        {
            public ShaderFloatArgumentPair ArgumentData { get; }
            public float CurrentState { get; set; }
        
            public ShaderFloatArgumentStatePair(ShaderFloatArgumentPair argumentData, float currentState)
            {
                ArgumentData = argumentData;
                CurrentState = currentState;
            }
        }
    }
}



