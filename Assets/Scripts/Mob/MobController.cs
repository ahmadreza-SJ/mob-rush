using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

namespace Mob
{
    [RequireComponent(typeof(MobMovementBehaviour))]
    public class MobController : MonoBehaviour
    {
        [SerializeField] private Side side;
        [SerializeField] private Material friendMat;
        [SerializeField] private Material enemyMat;
        [SerializeField] Renderer sideColoredRenderer;

        [SerializeField] private MobMovementBehaviour movementBehaviour;
        [SerializeField] private MobCollisionBehaviour collisionBehaviour;


        private List<Material> _materials;

        public MobMovementBehaviour MovementBehaviour => movementBehaviour;
        public MobCollisionBehaviour CollisionBehaviour => collisionBehaviour;

        public Side Side => side;

        public bool released;
        

        public void Initialize()
        {
            _materials = sideColoredRenderer.materials.ToList();
            foreach (Material material in _materials)
            {
                material.SetFloat("_Level", -0.2f);
            }
            movementBehaviour.Initialize();
            collisionBehaviour.Initialize(this);
            
        }

        public Tween ShowDissolve()
        {
            Tween t = null;
            foreach (Material material in _materials)
            {
                t = material.DOFloat(1.2f, "_Level", 0.1f);
            }

            return t;
        }

        public void Reinitialize()
        {
            foreach (Material material in _materials)
            {
                material.SetFloat("_Level", -0.2f);
            }
            movementBehaviour.ReInitialize();
            collisionBehaviour.Reinitialize();
        }
        
        
        public void StartMove()
        {
            movementBehaviour.StartMove(side);
        }
        
    }
}
