using _Project.Enemies.Data;
using _Project.Enemies.Scripts.Managers;
using UnityEngine;
using UnityEngine.AI;

namespace _Project.Enemies.Scripts.State_Machine
{
    public class EnemyAttackState : State
    {
        [Header("References")]
        [SerializeField] private EnemyData _enemyData;
        [SerializeField] private EnemyRangedAttack _rangedAttack;
        [SerializeField] private EnemyMeleeAttack _meleeAttack;
        [SerializeField] private Animator _animator;
        private EnemyChaseState _chaseState;
        private Transform _player;
        private NavMeshAgent _agent;

        private float _attackRange;
        private float _attackCoolDown;
        private float _lastAttackTime;
        
        private bool _isAttacking;
        private bool _isPlayingAttackAnimation;
        
        private static readonly int IsAttacking = Animator.StringToHash("IsAttacking");
        private Vector3 _attackPosition;

       private void Start()
       {
           InitializeComponents();
           _attackRange = _enemyData.AttackRange;
           _attackCoolDown = _enemyData.AttackCooldown;
           _agent = GetComponent<NavMeshAgent>();
       }

       private void InitializeComponents()
       {
           _player = GameObject.FindGameObjectWithTag("Player").transform;
           _chaseState = GetComponent<EnemyChaseState>();

           if (_player != null)
           {
               if (_enemyData.CombatStyle == EnemyData.CombatStyles.Ranged)
               {
                   _rangedAttack = GetComponentInChildren<EnemyRangedAttack>();   
               }
               
               if (_enemyData.CombatStyle == EnemyData.CombatStyles.Melee)
               {
                   _meleeAttack = GetComponentInChildren<EnemyMeleeAttack>();   
               }
           }
           
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
           if (_enemyData.CombatStyle == EnemyData.CombatStyles.Ranged)
           {
               if (_rangedAttack == null)
                   Debug.LogError($"EnemyRangedAttack not found on {gameObject.name}!");
           }
           if (_enemyData.CombatStyle == EnemyData.CombatStyles.Melee)
           {
               if (_meleeAttack == null)
                   Debug.LogError($"EnemyMeleeAttack not found on {gameObject.name}!");
           }
       }

         public override State RunCurrentState()
        {
            if(_player == null) return _chaseState;
            
            float distanceToPlayer = Vector3.Distance(_player.position, transform.position);
            
            if (distanceToPlayer > _attackRange && !_isPlayingAttackAnimation)
            {
                StopAttacking();
                return _chaseState;
            }

            if (!_isPlayingAttackAnimation && Time.time >= _lastAttackTime + _attackCoolDown)
            {
                StartAttacking();
            }
            
            if (_isPlayingAttackAnimation)
            {
                // Keep position fixed during attack but allow rotation
                transform.position = _attackPosition;
                Vector3 targetPosition = _player.position;
                targetPosition.y = transform.position.y;
                transform.LookAt(targetPosition);
            }
            
            return this;
        }

        public override void SetAnimation()
        {
            //throw new System.NotImplementedException();
        }

        private void StartAttacking()
        {
            _isAttacking = true;
            _attackPosition = transform.position;
            PerformAttack();
            _lastAttackTime = Time.time;
        }

        private void StopAttacking()
        {
            _isAttacking = false;
            _isPlayingAttackAnimation = false;
            _animator.SetBool(IsAttacking, false);
            
            if (_agent != null)
            {
                _agent.isStopped = false;
                _agent.updatePosition = true;
            }
        }

        private void PerformAttack()
        {
            if (_agent != null)
            {
                _agent.isStopped = true;
                _agent.velocity = Vector3.zero;
                _agent.updatePosition = false; // Prevent position updates during attack
            }
            
            _isPlayingAttackAnimation = true;
            _animator.SetBool(IsAttacking, true);
            
            if (_enemyData.CombatStyle == EnemyData.CombatStyles.Ranged)
            {
                _rangedAttack.ShootBullet();
            }
        }

        // Animation Event - Called at the attack point in animation
        public void MakeMeleeAttack()
        {
            if (_enemyData.CombatStyle == EnemyData.CombatStyles.Melee)
            {
                _meleeAttack.MakeMeleeAttack();
            }
            StopAttacking();
        }

        public void OnAttackAnimationEnd()
        {
            StopAttacking();
        }
    }
}