using System;
using _Project.Enemies.Data;
using UnityEngine;
using UnityEngine.AI;

namespace _Project.Enemies.Scripts.State_Machine
{
    public class EnemyChaseState : State
    {
        [Header("References")]
        [SerializeField] private EnemyData _enemyData;
        [SerializeField] private Animator _animator;
        private EnemyIdleState _idleState;
        private EnemyAttackState _attackState;
        
        private Transform _player;
        private NavMeshAgent _agent;
        private float _detectionRange;
        private float _attackRange;
        private bool _isWalking;

        private void Start()
        {
            InitializeComponents();
            _detectionRange = _enemyData.DetectionRange;
            _attackRange = _enemyData.AttackRange;
        }

        private void InitializeComponents()
        {
            _player = GameObject.FindGameObjectWithTag("Player").transform;
            _agent = GetComponent<NavMeshAgent>();
            _idleState = GetComponent<EnemyIdleState>();
            _attackState = GetComponent<EnemyAttackState>();
            
            ValidateReferences();
        }

        private void ValidateReferences()
        {
            if (_player == null)
                Debug.LogError($"Player not assigned to {gameObject.name}!");
            if (_enemyData == null)
                Debug.LogError($"EnemyData not assigned to {gameObject.name}!");
            if (_idleState == null)
                Debug.LogError($"Idle State not assigned to {gameObject.name}!");
            if (_attackState == null)
                Debug.LogError($"Attack State not assigned to {gameObject.name}!");
        }

        private void Update()
        {
            SetAnimation();
        }

        public override State RunCurrentState()
        {
            if(_player == null) return _idleState;
            
            float distanceToPlayer = Vector3.Distance(transform.position, _player.position);

            if (distanceToPlayer > _detectionRange)
            {
                _agent.isStopped = true;
                _isWalking = false;
                return _idleState;
            }

            if (distanceToPlayer <= _attackRange)
            {
                _agent.isStopped = true;
                _isWalking = false;
                return _attackState;
            }

            _isWalking = true;
            _agent.isStopped = false;
            _agent.SetDestination(_player.position);
            return this;
        }

        public override void SetAnimation()
        {
            _animator.SetBool("IsWalking", _isWalking);
        }
    }
}