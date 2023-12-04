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
        private Tween _moveTween;
        private bool _isMoving;
        

        public void Initialize()
        {
            _transform = transform;
            _rigidbody = GetComponent<Rigidbody>();
        }
        
        
        public void StartMove(Side side)
        {
            _moveDirection = _sideMoveDirectionDictionary[side];
            Vector3 destination = _transform.position + _moveDirection * maxDistance;
            _isMoving = true;
            KeepMoving().Forget();
        }

        public async UniTask KeepMoving()
        {
            while (_isMoving)
            {
                _rigidbody.velocity = _moveDirection * moveSpeed;
                await UniTask.Yield();
            }
        }

        private void OnDestroy()
        {
            _isMoving = false;
        }
    }
}
