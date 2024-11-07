using _Project.Data;
using UnityEngine;

namespace _Project.Enemies.Scripts.Components
{
    public class EnemyElementComponent : MonoBehaviour
    {
        public ElementData.Element CurrentElement;
        [SerializeField] private ElementData _elementData;
        
        private void Start()
        {
            CurrentElement = _elementData.CurrentElement;
        }
    }
}