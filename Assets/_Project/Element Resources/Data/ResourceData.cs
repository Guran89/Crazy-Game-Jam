using _Project.Data;
using UnityEngine;

namespace _Project.Element_Resources.Data
{
    [CreateAssetMenu(fileName = "ResourceData", menuName = "Resources/Data")]
    public class ResourceData : ScriptableObject
    {
        [Header("Basic Info")]
        public string Name;
        [TextArea(3, 5)]
        public string Description;
        
        [Header("Element Settings")]
        public ElementData.Element ElementType;
        public int MaxCharges = 60;  // Default value
        
        [Header("Visual Settings")]
        public Mesh Mesh;
        // Could add more visual properties here:
        // public Color ElementColor;
        // public GameObject EffectsPrefab;
        // public float VisualRange = 5f;
    }
}