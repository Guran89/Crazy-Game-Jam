using _Project.Particle_Effects;
using UnityEngine;
using _Project.Player.Scripts.Components;

namespace _Project.Player.Scripts.Managers
{
    public class ElementParticleManager : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _elementParticlePrefab;
        [SerializeField] private ElementComponent _playerElement;
        

        private void Awake()
        {
            ValidateReferences();
        }
        
        private void Start()
        {
            if (_playerElement == null)
                _playerElement = GameObject.Find("Player")?.GetComponent<ElementComponent>();
                
            if (_elementParticlePrefab == null)
                _elementParticlePrefab = GetComponentInChildren<ParticleSystem>();
                
            ValidateReferences();
        }

        private void Update()
        {
            if (_playerElement != null && _elementParticlePrefab != null)
            {
                ParticleColorChanger.SetParticleElementColor(_elementParticlePrefab, _playerElement.CurrentElement);
            }
        }
        
        private void ValidateReferences()
        {
            if (_elementParticlePrefab == null)
                Debug.LogWarning($"Particle System not assigned on {gameObject.name}!");
            if (_playerElement == null)
                Debug.LogWarning($"Player Element Component not found on {gameObject.name}!");
        }
    }
}