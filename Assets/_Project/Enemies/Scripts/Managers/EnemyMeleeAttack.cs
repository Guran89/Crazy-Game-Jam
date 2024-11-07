using _Project.Enemies.Data;
using UnityEngine;

namespace _Project.Enemies.Scripts.Managers
{
    public class EnemyMeleeAttack : MonoBehaviour
    {
        [SerializeField] private EnemyData _enemyData;
        [SerializeField] private GameObject _meleeHitPrefab; // This would be like your bullet prefab
        [SerializeField] private Transform _hitBoxSpawnPoint;
        [SerializeField] private float _meleeSpeed = 20f; // Faster than ranged attacks
        [SerializeField] private float _meleeLifetime = 0.1f; // Very short lifetime

        private void Start()
        {
            if (_hitBoxSpawnPoint == null)
                _hitBoxSpawnPoint = transform;
            
            ValidateReferences();
        }

        private void ValidateReferences()
        {
            if (_meleeHitPrefab == null)
                Debug.LogError($"MeleeHitPrefab not assigned to {gameObject.name}!");
            if (_hitBoxSpawnPoint == null)
                Debug.LogError($"HitBoxSpawnPoint not found on {gameObject.name}!");
            if (_enemyData == null)
                Debug.LogError($"EnemyData not assigned to {gameObject.name}!");
        }

        public void MakeMeleeAttack()
        {
            if (_meleeHitPrefab == null || _hitBoxSpawnPoint == null) return;
            
            // Spawn the melee hitbox and set it in motion
            GameObject meleeHit = Instantiate(_meleeHitPrefab, _hitBoxSpawnPoint.position, transform.rotation);
            
            // Add forward momentum (like a bullet but shorter range)
            if (meleeHit.TryGetComponent<Rigidbody>(out var rb))
            {
                rb.linearVelocity = transform.forward * _meleeSpeed;
            }
            
            // Destroy after short duration
            Destroy(meleeHit, _meleeLifetime);
        }
    }
}