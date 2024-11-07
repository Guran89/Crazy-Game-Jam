using System;
using _Project.Enemies.Data;
using _Project.Enemies.Scripts.Managers;
using UnityEngine;

namespace _Project.Enemies.Scripts.Components
{
    public class ProjectileComponent : MonoBehaviour
    {
        [SerializeField] private EnemyData _enemyData;
        [SerializeField] private float _projectileSpeed = 10f;
        private EnemyDamageManager _damageManager;
        private Rigidbody _rb;
        private Vector3 _spawnPosition;

        private void Start()
        {
            _damageManager = FindFirstObjectByType<EnemyDamageManager>();
            _rb = GetComponent<Rigidbody>();
            _spawnPosition = transform.position;
            
            Invoke(nameof(DestroyBullet), 3f);
            ValidateReferences();
        }
        
        private void ValidateReferences()
        {
            if (_enemyData == null)
                Debug.LogError($"EnemyData not assigned to {gameObject.name}!");
            if (_damageManager == null)
                Debug.LogError($"EnemyDamageManager not found!");
            if (_rb == null)
                Debug.LogError($"Rigidbody not found on {gameObject.name}!");
        }
        
        private void DestroyBullet()
        {
            Destroy(gameObject);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                _damageManager.DealDamage(_enemyData.AttackDamage);
                _damageManager.ApplyKnockback(_spawnPosition);
                DestroyBullet();
            }
        }
    }
}