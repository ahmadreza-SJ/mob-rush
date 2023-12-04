using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Mob
{
    public class MobMovementBehaviour : MonoBehaviour
    {
        private static Dictionary<Side, Vector3> _sideMoveDirectionDictionary = new Dictionary<Side, Vector3>()
        {
            { Side.Friend, Vector3.forward },
            { Side.Enemy, Vector3.back },
        };

        [SerializeField] private float moveSpeed = 1f;
        [SerializeField] private float maxDistance = 100;
        
        private Vector3 _moveDirection;
        private Transform _transform;
        private Rigidbody _rigidbody;
        private bool _isMoving;
        private bool _targetLocked;
        
        private Vector3 _target = Vector3.zero;

        public bool TargetLocked => _targetLocked;

        public void Initialize()
        {
            _transform = transform;
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void ReInitialize()
        {
            _targetLocked = false;
            _isMoving = false;
        }
        
        public void StartMove(Side side)
        {
            _moveDirection = _sideMoveDirectionDictionary[side];
            _target = _transform.position + _moveDirection * maxDistance;
            _isMoving = true;
            KeepMoving().Forget();
        }

        public async UniTask KeepMoving()
        {
            while (_isMoving)
            {
                _rigidbody.velocity = (_target - _transform.position).normalized * moveSpeed;
                await UniTask.Yield();
            }
        }

        public void SetTarget(Vector3 newTarget)
        {
            if (_targetLocked)
            {
                return;
            }
            
            _target = newTarget;
        }
        
        public void ForceSetTarget(Vector3 newTarget)
        {
            _target = newTarget;
        }

        public void LockTarget(bool locked)
        {
            _targetLocked = locked;
        }
        
        private void OnDestroy()
        {
            _isMoving = false;
        }
    }
}
