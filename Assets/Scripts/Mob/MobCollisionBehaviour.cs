using System;
using UnityEngine;

namespace Mob
{
    public class MobCollisionBehaviour : MonoBehaviour
    {
        public static Action<MobController, GameObject> Collided;

        private MobController _mobController;
        
        // Start is called before the first frame update
        public void Initialize(MobController mobController)
        {
            _mobController = mobController;
        }

        public void Reinitialize()
        {
            
        }

        private void OnCollisionEnter(Collision other)
        {
            Collided?.Invoke(_mobController, other.gameObject);
        }
    }
}
