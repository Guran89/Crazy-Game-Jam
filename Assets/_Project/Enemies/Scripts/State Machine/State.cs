using UnityEngine;

namespace _Project.Enemies.Scripts.State_Machine
{
    public abstract class State : MonoBehaviour
    {
        public abstract State RunCurrentState();
        
        public abstract void SetAnimation();
    }
}
