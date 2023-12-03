using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mob
{
    [RequireComponent(typeof(MobMovementBehaviour))]
    public class MobController : MonoBehaviour
    {
        [SerializeField] private Side side;
        [SerializeField] private Material friendMat;
        [SerializeField] private Material enemyMat;
        [SerializeField] private List<Renderer> sideColoredRenderers;
        [SerializeField] private MobMovementBehaviour movementBehaviour;

        private Material _sideMaterial;

        public Side Side => side;

        private void OnValidate()
        {
            SetSideMaterial();
            foreach (Renderer r in sideColoredRenderers)
            {
                r.material = _sideMaterial;
            }
        }

        public void Initialize()
        {
            movementBehaviour.Initialize();
        }
        
        public void StartMove()
        {
            movementBehaviour.StartMove(side);
        }
        
        private void SetSideMaterial()
        {
            switch (side)
            {
                case Side.Friend:
                {
                    _sideMaterial = friendMat;
                    break;
                }
                case Side.Enemy:
                {
                    _sideMaterial = enemyMat;
                    break;
                }
            }
        }
    }
}
