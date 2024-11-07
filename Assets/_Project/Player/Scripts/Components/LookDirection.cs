using UnityEngine;

namespace _Project.Player.Scripts.Components
{
    public class LookDirection : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Camera _camera;
        
        [Header("Settings")]
        [SerializeField] private LayerMask _groundLayer; // Optional: for specific layer targeting

        private void Start()
        {
            ValidateReferences();
        }

        private void ValidateReferences()
        {
            if (_camera == null)
            {
                _camera = Camera.main;
                if (_camera == null)
                    Debug.LogError($"No camera assigned or found for {gameObject.name}!");
            }
        }

        private void Update()
        {
            UpdateLookDirection();
        }

        private void UpdateLookDirection()
        {
            if (_camera == null) return;

            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _groundLayer))
            {
                Vector3 targetPoint = hit.point;
                targetPoint.y = transform.position.y;  // Always lock Y-axis for top-down view
                transform.LookAt(targetPoint);
            }
        }
    }
}