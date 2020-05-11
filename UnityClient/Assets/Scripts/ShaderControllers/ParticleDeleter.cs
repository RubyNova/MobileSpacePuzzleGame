using System;
using UnityEngine;

namespace ShaderControllers
{
    public class ParticleDeleter : MonoBehaviour
    {
        [SerializeField] private float _deleteDelay;

        private float _timeToDelete;

        private void Start() => _timeToDelete = _deleteDelay;

        // Update is called once per frame
        void Update()
        {
            if (_timeToDelete <= 0)
            {
                Destroy(gameObject);
            }

            _timeToDelete -= Time.deltaTime;
        }
    }
}
