using _Project.Element_Resources.Data;
using _Project.Player.Data;
using _Project.Player.Scripts.Components;
using _Project.Player.Scripts.Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Element_Resources.Scripts.Components
{
    [RequireComponent(typeof(SphereCollider))]
    public class ElementDrawPoint : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private ResourceData _resourceDataData;
        [SerializeField] private ShootManager _shootManager;
        [SerializeField] private PlayerData _playerData;
        
        [Header("State")]
        [SerializeField] private int _currentCharges;
        
        // Private references
        private SphereCollider _triggerArea;
        private ElementManager _elementManager;
        private PlayerController _playerController;
        private InputAction _reloadAction;
        
        // State tracking
        private bool _playerInRange;
        private bool _active = true;

        //private int _drawSpeed = 1;
        private float _timeBetweenDraws;
        private float _timeSinceLastDraw;

        private void Start()
        {
            InitializeComponents();
            SetupInitialState();
            SubscribeToEvents();
            _timeBetweenDraws = 1f;
            
            // Get reload action reference
            _reloadAction = InputSystem.actions.FindAction("Reload");
            if (_reloadAction == null) Debug.LogError("Reload action not found!");
        }

        private void InitializeComponents()
        {
            // Get and validate components
            _triggerArea = GetComponent<SphereCollider>();
            _triggerArea.isTrigger = true;
            
            _playerController = FindFirstObjectByType<PlayerController>();
            _elementManager = FindFirstObjectByType<ElementManager>();
            
            ValidateReferences();
        }

        private void ValidateReferences()
        {
            if (_elementManager == null)
                Debug.LogError($"No ElementManager found for {gameObject.name}!");
            if (_playerController == null)
                Debug.LogError($"No PlayerController found for {gameObject.name}!");
            if (_resourceDataData == null)
                Debug.LogError($"No DrawPointData assigned to {gameObject.name}!");
            if (_shootManager == null)
                Debug.LogError($"No ShootManager assigned to {gameObject.name}!");
            if (_playerData == null)
                Debug.LogError($"No PlayerData assigned to {gameObject.name}!");
        }

        private void Update()
        {
            // Check if player is holding reload button (RMB)
            bool isReloading = _reloadAction != null && _reloadAction.IsPressed();
            
            // If conditions for drawing are met and RMB is held, draw ammo
            if (CanDraw() && isReloading && _shootManager.CurrentAmmo < _playerData.MaxAmmo)
            {
                DrawElement();
                if (Time.time - _timeSinceLastDraw >= _timeBetweenDraws)
                {
                    _shootManager.CurrentAmmo += 3;
                    _timeSinceLastDraw = Time.time;
                    
                    // Update draw point state
                    _currentCharges -= 3;
                    
                    // Check if depleted
                    if (_currentCharges <= 0)
                    {
                        _active = false;
                        Debug.Log($"{_resourceDataData.Name} draw point depleted!");
                    }
                    else
                    {
                        Debug.Log($"Drew ammo from {_resourceDataData.Name}. {_currentCharges} charges remaining");
                    }
                }
            }
        }

        private void SetupInitialState()
        {
            _currentCharges = _resourceDataData.MaxCharges;
            _active = true;
            _playerInRange = false;
        }

        private void SubscribeToEvents()
        {
            if (_playerController != null)
            {
                _playerController.OnInteractInput += DrawElement;
            }
        }

        private void OnDisable()
        {
            if (_playerController != null)
            {
                _playerController.OnInteractInput -= DrawElement;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _playerInRange = true;
                Debug.Log($"Player entered {_resourceDataData.Name} draw point range");
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _playerInRange = false;
            }
        }
        
        private bool CanDraw()
        {
            return _active && _playerInRange && _currentCharges > 0;
        }

        private void DrawElement()
        {
            if (!CanDraw()) return;

            // Change player's element
            _elementManager.HandleElementChange(_resourceDataData.ElementType);
            Debug.Log($"Changed element to {_resourceDataData.ElementType}");
        }
    }
}