using PostProcessing;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace ShaderControllers
{
    public class PerlinNoiseTextureGenerator : MonoBehaviour
    {
        [SerializeField, Header("Dependencies")] 
        private Renderer _targetRenderer;
        
        [SerializeField, Header("Noise Config")]
        private float _coordinateColourScale = 10f;
        
        [SerializeField] private float _offsetX = 100f;
        [SerializeField] private float _offsetY = 100f;
        [SerializeField] private string _shaderColourTintArgName;
        [SerializeField] private Color _colourTint = Color.white;
        

        private const int Width = 256;
        private const int Height = 256;
        
        private void Start()
        {
            _targetRenderer.material = new Material(_targetRenderer.material.shader);
            _targetRenderer.material.mainTexture = SupplyPerlinNoise();

            if (string.IsNullOrWhiteSpace(_shaderColourTintArgName)) return;
            
            _targetRenderer.material.SetColor(_shaderColourTintArgName, _colourTint);
        }

        private Texture SupplyPerlinNoise()
        {
            var noiseTexture = new Texture2D(Width, Height);

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    var pixelColour = CalculateRGBValue(x, y);
                    noiseTexture.SetPixel(x, y, pixelColour);
                }
            }
            
            noiseTexture.Apply();
            return noiseTexture;
        }

        private Color CalculateRGBValue(int x, int y)
        {
            float sample = Mathf.PerlinNoise((float)x / Width * _coordinateColourScale + _offsetX, (float)y / Height * _coordinateColourScale + _offsetY);
            return new Color(sample, sample, sample);
        }
    }
}
