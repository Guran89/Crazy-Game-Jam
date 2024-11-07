using _Project.Enemies.Data;
using UnityEngine;

namespace _Project.Enemies.Scripts.Managers
{
    public class EnemyRangedAttack : MonoBehaviour
    {
        // Get references
        [SerializeField] private Rigidbody _projectilePrefab;
        [SerializeField] private EnemyData _enemyData;
        [SerializeField] private float _projectileSpeed = 15f;
        
        public void ShootBullet()
        {
            Rigidbody projectileClone = Instantiate(_projectilePrefab, transform.position, Quaternion.identity);
            projectileClone.linearVelocity = transform.TransformDirection(Vector3.forward * _projectileSpeed);
        }
    }
}