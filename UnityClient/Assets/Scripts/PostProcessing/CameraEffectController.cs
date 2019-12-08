using System;
using UnityEngine;

namespace PostProcessing
{
    public class CameraEffectController : MonoBehaviour
    {
        [SerializeField]
        private Material _effectMaterial;

        [SerializeField] 
        private float _intensity = 0.5f;

        [SerializeField]
        private string _effectName = "CHANGE THIS";

        private int _intensityPropertyId;

        public float Intensity
        {
            get => _intensity;
            set
            {
                if (value < 0)
                {
                    _intensity = 0;
                    Debug.LogWarning("An attempt was made to assign a negative intensity. You cannot do this! Value clamped to 0.");
                    return;
                }
                else if (value > 1)
                {
                    _intensity = 1;
                    Debug.LogWarning("An attempt was made to assign a value larger than 1. You cannot do this! Value clamped to 1.");
                    return;
                }
                _intensity = value;
            }
        }

        public string EffectName
        {
            get => _effectName;
            set => _effectName = value;
        }

        private void Start() => _intensityPropertyId = Shader.PropertyToID("_Intensity");

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (Math.Abs(Intensity) < 0)
            {
                Graphics.Blit(source, destination);
                return;
            }
            
            _effectMaterial.SetFloat(_intensityPropertyId, Intensity);
            Graphics.Blit(source, destination, _effectMaterial);
        }
    }
}
