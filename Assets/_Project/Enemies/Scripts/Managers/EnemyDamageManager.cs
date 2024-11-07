using _Project.Enemies.Data;
using _Project.Player.Scripts.Components;
using UnityEngine;

namespace _Project.Enemies.Scripts.Managers
{
    public class EnemyDamageManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerHealth _playerHealth;
        [SerializeField] private EnemyData _enemyData;
        [SerializeField] private Animator _animator;
        private Rigidbody _playerRigidBody;
        private Transform _player;
        private float _knockbackForce;
        private bool _isKnockedBack;

        private void Start()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            _player = GameObject.FindGameObjectWithTag("Player").transform;
            _animator = _player.GetComponent<Animator>();

            if (_player != null)
            {
                _playerRigidBody = _player.GetComponent<Rigidbody>();
                _playerHealth = _player.GetComponent<PlayerHealth>();
                _knockbackForce = _enemyData.KnockbackForce;
            }
            
            ValidateReferences();
        }
        
        private void ValidateReferences()
        {
            if (_player == null)
                Debug.LogError($"Player not assigned to {gameObject.name}!");
            if (_enemyData == null)
                Debug.LogError($"EnemyData not assigned to {gameObject.name}!");
        }

        public void DealDamage(float damage)
        {
            if (_playerHealth != null && _playerHealth.CurrentHealth >= 0)
            {
                _playerHealth.TakeDamage(_enemyData.AttackDamage);
                Debug.Log($"Enemy attacked for {_enemyData.AttackDamage} damage!");
            }
        }

        private void Update()
        {
            print("Knocked back: " + _isKnockedBack);
        }

        public void ApplyKnockback(Vector3 sourcePosition)
        {
            _isKnockedBack = true;
            if (_playerRigidBody != null && !_playerHealth.IsDead)
            {
                Vector3 knockbackDirection = (_player.position - sourcePosition).normalized;
                _playerRigidBody.AddForce(knockbackDirection * _knockbackForce, ForceMode.Impulse);
                _animator.SetBool("IsKnockedBack", _isKnockedBack);
                Invoke(nameof(DisableKnockback), 1.2f);
            }
        }

        private void DisableKnockback()
        {
            _isKnockedBack = false;
            _animator.SetBool("IsKnockedBack", _isKnockedBack);
        }
    }
}