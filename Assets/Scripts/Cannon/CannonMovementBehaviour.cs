using UnityEngine;

namespace Cannon
{
    public class CannonMovementBehaviour : MonoBehaviour
    {
        [SerializeField] private float moveHalfWidth;
        [SerializeField] private float moveSpeed = 0.1f;

        private Transform _transform;

        private void Start()
        {
            _transform = gameObject.transform;
        }

        public void MoveCannonX(float delta)
        {
            float dX = delta * moveSpeed;
            Vector3 currentPos = _transform.position;
            if (Mathf.Abs(currentPos.x + dX) < moveHalfWidth)
            {
                Vector3 newPos = currentPos;
                newPos.x += dX;
                _transform.position = newPos;
            }
        }
    }
}
