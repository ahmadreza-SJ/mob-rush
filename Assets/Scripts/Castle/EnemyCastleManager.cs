using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Castle
{
    [Serializable]
    public class EnemyCastleManager
    {
        [SerializeField] [Range(10, 100)] private int health = 10;
        [SerializeField] [Range(0.1f, 10)] private float spawnInterval = 1f;
        
        private List<EnemyCastleController> _castles;
        private List<EnemyCastleController> _destroyedCastles;
    
    
        public static Action<EnemyCastleController, Vector3> CastleSpawnTimeReached;
        
        private static Action<EnemyCastleController> _castleSubscribed;

        public List<EnemyCastleController> Castles => _castles;

        public static void SubscribeCastle(EnemyCastleController castle)
        {
            _castleSubscribed?.Invoke(castle);
        }

        public bool TryGetNearestCastle(Vector3 to, out EnemyCastleController nearestCastle)
        {
            nearestCastle = null;
            if (_castles.Count == 0)
            {
                return false;
            }
        
            float minDistance = float.MaxValue;
            foreach (EnemyCastleController castle in _castles)
            {
                float dist = (to - castle.GetPosition()).magnitude;
                if (minDistance > dist)
                {
                    minDistance = dist;
                    nearestCastle = castle;
                }
            
            }

            return true;
        }

        public void RemoveCastle(EnemyCastleController castle)
        {
            _castles.Remove(castle);
            _destroyedCastles.Add(castle);
        }
        
        
        // Start is called before the first frame update
        public void Initialize()
        {
            _castles = new List<EnemyCastleController>();
            _destroyedCastles = new List<EnemyCastleController>();
            
            // EnemyCastleController.CastleDestroyed += OnCastleDestroyed;
            _castleSubscribed += OnCastleSubscribed;
        }

        public void ReInitialize()
        {
            foreach (EnemyCastleController castle in _destroyedCastles)
            {
                _castles.Add(castle);
                castle.Initialize(health, spawnInterval);
            }
            
            _destroyedCastles.Clear();
        }


        private void OnCastleSubscribed(EnemyCastleController castle)
        {
            castle.Initialize(health, spawnInterval);
            _castles.Add(castle);
        }
    }
}
