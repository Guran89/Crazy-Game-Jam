using _Project.Player.Scripts.Interfaces;
using UnityEngine;
using _Project.Data;

namespace _Project.Player.Scripts.Components
{
    public class ElementComponent : MonoBehaviour, IElementChangeable
    {
        //[SerializeField] private MeshRenderer _mesh;
        [SerializeField] private ElementData.Element _currentElement;
        public ElementData.Element CurrentElement => _currentElement;

        private void Start()
        {
            _currentElement = ElementData.Element.None;
        }

        public void ChangeElement(ElementData.Element newElement)
        {
            Debug.Log($"Changing element on {gameObject.name} from {_currentElement} to {newElement}");
            _currentElement = newElement;
            
            // Could add visual feedback here:
            // UpdateElementVisuals();
        }
    }
}