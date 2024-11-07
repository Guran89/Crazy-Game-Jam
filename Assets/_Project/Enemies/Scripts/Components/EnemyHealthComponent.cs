using _Project.Enemies.Data;
using _Project.Enemies.Scripts.Managers;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace _Project.Enemies.Scripts.Components
{
    public class EnemyHealthComponent : MonoBehaviour
    {
        [SerializeField] private EnemyData _enemyData;
        [SerializeField] private Animator _animator;
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private EnemyDeathHandler _enemyDeathHandler;
        public float CurrentHealth;
        public float MaxHealth;

        private void Start()
        {
            MaxHealth = _enemyData.MaxHealth;
            CurrentHealth = _enemyData.MaxHealth;
        }

        private void Update()
        {
            if (CurrentHealth <= 0)
            {
                EnemyKilled();
            }
        }

        public void TakeDamage(float damage)
        {
            CurrentHealth -= damage;
        }

        private void EnemyKilled()
        {
            if (_navMeshAgent != null)
            {
                _navMeshAgent.isStopped = true;  // This stops the agent from moving
                _navMeshAgent.velocity = Vector3.zero;  // This ensures they stop immediately
                _navMeshAgent.angularSpeed = 0f;
            }
            _enemyDeathHandler.TriggerDeath();
            //_animator.SetBool("IsDying", true);
        }
    }
}
