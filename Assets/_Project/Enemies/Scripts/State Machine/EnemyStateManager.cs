using _Project.Enemies.Data;
using UnityEngine;

namespace _Project.Enemies.Scripts.State_Machine
{
    public class EnemyStateManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private EnemyData _enemyData;
        [SerializeField] private State _currentState;
        
        [Header("Debug Visualization")]
        [SerializeField] private bool _showRanges = true;
        [SerializeField] private Color _detectionRangeColor = new Color(0f, 1f, 0f, 1f);
        [SerializeField] private Color _attackRangeColor = new Color(1f, 0f, 0f, 1f);
        
        private void Start()
        {
            InitializeComponents();
        }

        private void InitializeComponents() 
        {
            if(_currentState == null) _currentState = GetComponent<EnemyIdleState>();
        }

        private void Update()
        {
            State nextState = _currentState?.RunCurrentState();
            if (nextState != null && _currentState != nextState)
            {
                _currentState = nextState;
            }
        }
        
        private void OnDrawGizmos()
        {
            if (!_showRanges || _enemyData == null) return;

            // Draw detection range
            Gizmos.color = _detectionRangeColor;
            Gizmos.DrawWireSphere(transform.position, _enemyData.DetectionRange);
        
            // Draw attack range
            Gizmos.color = _attackRangeColor;
            Gizmos.DrawWireSphere(transform.position, _enemyData.AttackRange);
        }
    }
}
