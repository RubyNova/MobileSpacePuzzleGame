using UnityEngine;

namespace ShaderControllers
{
    public class MaterialOffsetAdjuster : MonoBehaviour
    {
        [SerializeField] private Vector2 _offsetSpeedAndDirection = Vector2.one;
        [SerializeField] private Renderer _targetRenderer;

        // Update is called once per frame
        void Update() => _targetRenderer.material.mainTextureOffset += (_offsetSpeedAndDirection * Time.deltaTime);
    }
}