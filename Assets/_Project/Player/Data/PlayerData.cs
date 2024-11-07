using UnityEngine;
using _Project.Data;

namespace _Project.Player.Data
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Player/New PlayerData", order = 1)]
    public class PlayerData : ScriptableObject
    {
        [Header("Basic Stats")]
        [Range(1f, 20f)] public float MovementSpeed = 5f;

        [Header("Combat Stats")]
        [Range(1f, 20f)] public float AttackDamage = 10f;
        [Range(0.1f, 10f)]
        [Tooltip("Number of shots the player can fire per second")]
        public float AttackSpeed = 1f;
        [Range(1f, 50f)] public float AttackRange = 10f;
        [Range(1f, 100f)] public float MaxHealth = 100f;
        [Range(0f, 30f)] public int MaxAmmo = 15;
        
        [Header("Element Settings")]
        [SerializeField] private ElementData.Element _currentElement;
        public ElementData.Element CurrentElement
        {
            get => _currentElement;
            set
            {
                _currentElement = value;
                // Could add events here if needed
            }
        }

        [Header("State")]
        public bool IsDead;

        private void OnEnable()
        {
            // Reset state when ScriptableObject is enabled
            IsDead = false;
        }
    }
}