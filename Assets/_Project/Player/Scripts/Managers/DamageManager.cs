using _Project.Enemies.Data;
using _Project.Enemies.Scripts.Components;
using _Project.Player.Scripts.Components;
using UnityEngine;

namespace _Project.Player.Scripts.Managers
{
    public class DamageManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private EnemyHealthComponent _playerHealth;
        [SerializeField] private EnemyData _enemyData;
    }
}
