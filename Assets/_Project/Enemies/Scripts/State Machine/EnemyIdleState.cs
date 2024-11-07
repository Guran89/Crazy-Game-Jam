using _Project.Enemies.Data;
using UnityEngine;

namespace _Project.Enemies.Scripts.State_Machine
{
    public class EnemyIdleState : State
    {
        [Header("References")]
        [SerializeField] private EnemyData _enemyData;
        private EnemyChaseState _chaseState;
        private Transform _player;
        private float _detectionRange;
        
        public string StateName;

        private void Start()
        {
            InitializeComponents();
            _detectionRange = _enemyData.DetectionRange;
        }

        private void InitializeComponents()
        {
            _player = GameObject.FindGameObjectWithTag("Player").transform;
            _chaseState = GetComponent<EnemyChaseState>();
            
            ValidateReferences();
        }

        private void ValidateReferences()
        {
            if (_player == null)
                Debug.LogError($"Player not assigned to {gameObject.name}!");
            if (_enemyData == null)
                Debug.LogError($"EnemyData not assigned to {gameObject.name}!");
            if (_chaseState == null)
                Debug.LogError($"Chase State not assigned to {gameObject.name}!");
        }

        public override State RunCurrentState()
        {
            if (_player == null) return this;
            
            float distanceToPlayer = Vector3.Distance(transform.position, _player.position);
            if (distanceToPlayer <= _detectionRange)
            {
                return _chaseState;
            }
            return this;
        }

        public override void SetAnimation()
        {
        }
    }
}
