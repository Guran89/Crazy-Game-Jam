using _Project.Data;
using UnityEngine;

namespace _Project.Enemies.Data
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "Enemies/New Enemy", order = 1)]
    public class EnemyData : ScriptableObject
    {
        [Header("Basic Stats")]
        [Range(1f, 20f)] public float MovementSpeed = 5f;
        [Range(5f, 40f)] public float DetectionRange = 10f;
        
        [Header("Combat Stats")]
        [Tooltip("Number of shots the enemy can fire per second")]
        [Range(0.1f, 20f)] public float AttackSpeed = 1f;
        [Range(1f, 30f)] public float AttackRange = 10f;
        [Range(1f, 100f)] public float MaxHealth = 100f;
        [Range(1f, 50f)] public float AttackDamage = 30f;
        [Range(1f, 10f)] public float AttackCooldown = 3f;
        [Range(0f, 10f)] public float KnockbackForce = 5f;

        [Header("Combat Style")]
        [SerializeField] public CombatStyles CombatStyle;

        public enum CombatStyles
        {
            Melee,
            Ranged
        }
        
        [Header("Element Settings")]
        [SerializeField] private ElementData.Element _currentElement;
        public ElementData.Element CurrentElement
        {
            get => _currentElement;
            set => _currentElement = value;
            // Could add events here if needed
        }
        
        [Header("Visual")]
        public Mesh Mesh;
    }
}