using System;
using _Project.Player.Data;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Player.Scripts.Components
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerData _playerData;
        [SerializeField] private Animator _animator;
        [SerializeField] private Camera _camera;
        
        [SerializeField] private LayerMask _groundLayer;
        
        // Input Actions
        private InputAction _moveAction;
        private InputAction _shootAction;
        private InputAction _interactAction;
        
        // Components
        private Rigidbody _rb;
        
        // Events
        public event System.Action OnShootInput;
        public event System.Action OnInteractInput;

        private void Awake()
        {
            Cursor.lockState = CursorLockMode.Confined;
        }

        private void Start()
        {
            InitializeComponents();
            SetupInputActions();
            ValidateReferences();
        }

        private void InitializeComponents()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void SetupInputActions()
        {
            _moveAction = InputSystem.actions.FindAction("Move");
            _shootAction = InputSystem.actions.FindAction("Attack");
            _interactAction = InputSystem.actions.FindAction("Interact");
            
            if (_moveAction == null) Debug.LogError("Move action not found!");
            if (_shootAction == null) Debug.LogError("Attack action not found!");
            if (_interactAction == null) Debug.LogError("Interact action not found!");
        }

        private void ValidateReferences()
        {
            if (_playerData == null)
                Debug.LogError($"PlayerData not assigned to {gameObject.name}!");
            if (_rb == null)
                Debug.LogError($"Rigidbody not found on {gameObject.name}!");
        }

        private void Update()
        {
            HandleInput();
        }

        private void HandleInput()
        {
            if (_shootAction.WasPressedThisFrame())
            {
                OnShootInput?.Invoke();
            }

            if (_interactAction.IsPressed())
            {
                OnInteractInput?.Invoke();
            }
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            if (_rb == null || _moveAction == null || _playerData == null) return;

            Vector2 moveInput = _moveAction.ReadValue<Vector2>();
            Vector3 movement = transform.right * moveInput.x + transform.forward * moveInput.y;
            
            _rb.transform.position += movement * (_playerData.MovementSpeed * Time.deltaTime);
            
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _groundLayer))
            {
                // Get direction to mouse cursor on ground
                Vector3 directionToMouse = (hit.point - transform.position).normalized;
                directionToMouse.y = 0; // Keep it on the horizontal plane

                // Get the movement direction normalized
                Vector3 normalizedMovement = movement.normalized;
                normalizedMovement.y = 0; // Keep it on the horizontal plane

                // Get the angle between movement and mouse direction
                float angle = Vector3.Angle(normalizedMovement, directionToMouse);

                // To determine left/right strafe, use cross product
                float cross = Vector3.Cross(directionToMouse, normalizedMovement).y;

                // Only process if actually moving
                if (movement.magnitude > 0.1f)
                {
                    bool isMoving = true;
                    // Forward/backward check
                    bool isMovingForward = angle < 45f;
                    bool isMovingBackward = angle > 135f;

                    // Strafe check (angle between 45° and 135°)
                    bool isStrafing = angle >= 45f && angle <= 135f;
                    bool isStrafingLeft = isStrafing && cross < 0;
                    bool isStrafingRight = isStrafing && cross > 0;

                    // Update animator
                    _animator.SetBool("IsMoving", isMoving);
                    _animator.SetBool("IsMovingForward", isMovingForward);
                    _animator.SetBool("IsMovingBackward", isMovingBackward);
                    _animator.SetBool("IsStrafingLeft", isStrafingLeft);
                    _animator.SetBool("IsStrafingRight", isStrafingRight);
                }
                else
                {
                    // Reset all movement bools when not moving
                    _animator.SetBool("IsMoving", false);
                    _animator.SetBool("IsMovingForward", false);
                    _animator.SetBool("IsMovingBackward", false);
                    _animator.SetBool("IsStrafingLeft", false);
                    _animator.SetBool("IsStrafingRight", false);
                    
                    
                }
            }
        }

        private void OnDisable()
        {
            // Clean up event subscribers
            OnShootInput = null;
            OnInteractInput = null;
        }
    }
}