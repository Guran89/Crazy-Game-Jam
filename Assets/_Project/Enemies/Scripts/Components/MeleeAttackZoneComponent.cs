using _Project.Enemies.Data;
using _Project.Enemies.Scripts.Managers;
using UnityEngine;

namespace _Project.Enemies.Scripts.Components
{
    public class MeleeAttackZoneComponent : MonoBehaviour
    {
        [SerializeField] private EnemyData _enemyData;
        [SerializeField] private float _projectileSpeed = 20f;
        private EnemyDamageManager _damageManager;
        private Rigidbody _rb;
        private bool _hasHit;
        private Vector3 _spawnPosition;
       
        private void Start()
        {
            _damageManager = FindFirstObjectByType<EnemyDamageManager>();
            _rb = GetComponent<Rigidbody>();
            _spawnPosition = transform.position;
           
            if (_rb != null)
            {
                _rb.linearVelocity = transform.forward * _projectileSpeed;
            }
           
            Invoke(nameof(DestroyAttackZone), 0.2f);
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

        private void DestroyAttackZone()
        {
            Destroy(gameObject);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Player") && !_hasHit)
            {
                _hasHit = true;
               
                _damageManager.DealDamage(_enemyData.AttackDamage);
                _damageManager.ApplyKnockback(_spawnPosition);
               
                print($"Player Hit from position: {_spawnPosition}");
                DestroyAttackZone();
            }
        }
    }
}