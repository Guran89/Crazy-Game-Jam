using _Project.Player.Data;
using _Project.Player.Scripts.Components;
using UnityEngine;
using _Project.Data;

namespace _Project.Player.Scripts.Managers
{
    public class ElementManager : MonoBehaviour
    {
        public static ElementManager Instance { get; private set; }
    
        [Header("References")]
        [SerializeField] private PlayerData _playerData;
        [SerializeField] private ElementComponent _playerElement;
        
        private void Awake()
        {
            SetupSingleton();
            ValidateReferences();
        }

        private void SetupSingleton()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogWarning($"Multiple ElementManagers found. Destroying {gameObject.name}");
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void ValidateReferences()
        {
            if (_playerData == null)
                Debug.LogError("Missing PlayerData reference in ElementManager!");
            if (_playerElement == null)
                Debug.LogError("Missing PlayerElement reference in ElementManager!");
        }
    
        public void HandleElementChange(ElementData.Element newElement)
        {
            if (_playerData == null || _playerElement == null) return;
            
            _playerData.CurrentElement = newElement;
            _playerElement.ChangeElement(newElement);
            
            Debug.Log($"Player element changed to {newElement}");
        }
    }
}