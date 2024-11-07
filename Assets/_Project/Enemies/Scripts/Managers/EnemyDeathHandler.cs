using UnityEngine;

public class EnemyDeathHandler : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private SkinnedMeshRenderer _meshRenderer;
    [SerializeField] private bool _disableColliders = true;
        
    private static readonly int IsDying = Animator.StringToHash("IsDying");
    private bool _isDying;

    private void Awake()
    {
        // Validate references
        if (_animator == null)
        {
            _animator = GetComponent<Animator>();
            Debug.LogWarning($"Animator not assigned on {gameObject.name}, attempting to get component");
        }
        if (_meshRenderer == null)
        {
            _meshRenderer = GetComponent<SkinnedMeshRenderer>();
            Debug.LogWarning($"MeshRenderer not assigned on {gameObject.name}, attempting to get component");
        }
    }

    public void TriggerDeath()
    {
        Debug.Log($"TriggerDeath called on {gameObject.name}"); // Debug log
        
        if (_isDying)
        {
            Debug.Log("Already dying, ignoring call");
            return;
        }
        
        _isDying = true;
        
        if (_animator != null)
        {
            _animator.SetBool(IsDying, true);
            
            // Get the current animation state
            AnimatorStateInfo state = _animator.GetCurrentAnimatorStateInfo(0);
            float animationLength = state.length;
            
            Debug.Log($"Animation length: {animationLength}"); // Debug log
            
            Invoke(nameof(DeactivateComponents), animationLength);
        }
        else
        {
            Debug.LogError($"Animator is null on {gameObject.name}");
            DeactivateComponents(); // Deactivate immediately if no animator
        }
    }

    private void DeactivateComponents()
    {
        Debug.Log($"DeactivateComponents called on {gameObject.name}"); // Debug log
        
        var components = GetComponents<Component>();
        
        foreach (var component in components)
        {
            if (component == null) continue;
            
            // Log what we're processing
            Debug.Log($"Processing component: {component.GetType().Name}");
            
            // Skip these components
            if (component is Transform || 
                component is MeshFilter || 
                component is MeshRenderer || 
                component is EnemyDeathHandler)
            {
                Debug.Log($"Skipping {component.GetType().Name}");
                continue;
            }
            
            // Disable component
            if (component is Behaviour behaviour)
            {
                behaviour.enabled = false;
                Debug.Log($"Disabled {component.GetType().Name}");
            }
        }

        if (_disableColliders)
        {
            var colliders = GetComponents<Collider>();
            foreach (var collider in colliders)
            {
                if (collider != null)
                {
                    collider.enabled = false;
                    Debug.Log($"Disabled collider: {collider.GetType().Name}");
                }
            }
        }
    }
}