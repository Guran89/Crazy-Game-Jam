using UnityEngine;

namespace _Project.Data
{
    [CreateAssetMenu(fileName = "ElementData", menuName = "Game/ElementData", order = 0)]
    public class ElementData : ScriptableObject
    {
        public Element CurrentElement;
        public enum Element
        {
            None,
            Fire,
            Water,
            Earth,
            Wind
        }
    }
}