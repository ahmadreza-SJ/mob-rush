using UnityEngine;

namespace Cannon
{
    [RequireComponent(typeof(CannonMovementBehaviour), typeof(CannonFireBehaviour))]
    public class CannonController : MonoBehaviour
    { 
        private CannonMovementBehaviour _movementBehaviour;
        private CannonFireBehaviour _fireBehaviour;
    
        public CannonMovementBehaviour MovementBehaviour => _movementBehaviour;
        public CannonFireBehaviour FireBehaviour => _fireBehaviour;
    
        // Start is called before the first frame update
        public void Initialize()
        {
            _movementBehaviour = GetComponent<CannonMovementBehaviour>();
            _fireBehaviour = GetComponent<CannonFireBehaviour>();
        }

    }
}
