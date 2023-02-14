
using UnityEngine;

namespace Gameplay.Interfaces
{
    public interface IHorizontalMovable
    {
        public void Move(float speed);
        
        public void Move(Vector3 position);
        
    }
}