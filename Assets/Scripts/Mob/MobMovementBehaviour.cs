using System;
using System.Collections.Generic;
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
        private Tween _moveTween;
        

        public void Initialize()
        {
            _transform = transform;
        }
        
        
        public void StartMove(Side side)
        {
            _moveDirection = _sideMoveDirectionDictionary[side];
            Vector3 destination = _transform.position + _moveDirection * maxDistance;
            _moveTween = transform.DOMove(destination, moveSpeed).SetEase(Ease.Linear).SetSpeedBased(true);
        }
    }
}
