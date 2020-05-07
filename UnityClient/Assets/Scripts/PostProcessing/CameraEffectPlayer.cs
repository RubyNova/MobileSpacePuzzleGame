using System;
using System.Collections.Generic;
using UnityEngine;

namespace PostProcessing
{
    public class CameraEffectPlayer : MonoBehaviour
    {
        [SerializeField]
        private Material _effectMaterial;

        [SerializeField]
        private string _effectName = "CHANGE THIS";

        public string EffectName
        {
            get => _effectName;
            set => _effectName = value;
        }

        public Material EffectMaterial
        {
            get => _effectMaterial;
            set => _effectMaterial = value;
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination) => Graphics.Blit(source, destination, EffectMaterial);
    }
}
