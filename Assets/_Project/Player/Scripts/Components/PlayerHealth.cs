using _Project.Player.Data;
using UnityEngine;

namespace _Project.Player.Scripts.Components
{
    public class PlayerHealth : MonoBehaviour
    {
        [SerializeField] private PlayerData _playerData;

        private float _maxHealth;
        public float CurrentHealth;
        public bool IsDead { get; private set; }

        private void Start()
        {
            InitializeComponents();
            _maxHealth = _playerData.MaxHealth;
            CurrentHealth = _maxHealth;
            IsDead = _playerData.IsDead;
        }

        private void InitializeComponents()
        {
            ValidateReferences();
        }
        
        private void ValidateReferences()
        {
            if (_playerData == null)
                Debug.LogError($"PlayerData not assigned to {gameObject.name}!");
        }

        public void TakeDamage(float damage)
        {
            CurrentHealth -= damage;

            if (CurrentHealth <= 0)
            {
                IsDead = true;
                Destroy(gameObject);
            }
        }
    }
}