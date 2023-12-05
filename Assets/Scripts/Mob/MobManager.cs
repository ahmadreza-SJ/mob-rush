using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Mob
{
    [Serializable]
    public class MobManager
    {
        [SerializeField] private MobController friendlyMobPrefab;
        [SerializeField] private MobController enemyMobPrefab;
        [SerializeField] private Transform friendMobParent;
        [SerializeField] private Transform enemyMobParent;

        private ObjectPool<MobController> _friendMobPool;
        private ObjectPool<MobController> _enemyMobPool;

        public readonly List<MobController> ActiveEnemyMobs = new List<MobController>();

        public readonly List<MobController> ActiveFriendMobs = new List<MobController>();
        
        public void Initialize()
        {
            
            _friendMobPool = new ObjectPool<MobController>((() =>
            {
                MobController instance = Object.Instantiate(friendlyMobPrefab, friendMobParent)
                    .GetComponent<MobController>();
                instance.Initialize();
                return instance;
            }), controller =>
            {
                controller.Reinitialize();
                controller.released = false;
            }, controller =>
            {                
                controller.released = true;

                controller.gameObject.SetActive(false);
                
            }, collectionCheck:true);
            
            _enemyMobPool = new ObjectPool<MobController>((() =>
            {
                MobController instance = Object.Instantiate(enemyMobPrefab, enemyMobParent)
                    .GetComponent<MobController>();
                instance.Initialize();
                return instance;
            }), controller =>
            {
                controller.released = false;
                controller.Reinitialize();
            }, actionOnRelease: controller =>
            {
                controller.released = true;
                controller.gameObject.SetActive(false);
            }, collectionCheck:true);
        }

        public void ReInitialize()
        {
            RemoveAll();
        }
        
        public void Remove(MobController mob)
        {
            
            if (mob.Side == Side.Friend)
            {
                if (!mob.released)
                {
                    _friendMobPool.Release(mob);;
                }
                ActiveFriendMobs.Remove(mob);
            }
            else
            {
                if (!mob.released)
                {
                    _enemyMobPool.Release(mob);
                }
                ActiveEnemyMobs.Remove(mob);
            }
        }

        public List<MobController> Spawn(Side side, Vector3 position, int count = 1, float randFactor = 0)
        {
            List<MobController> mobs = SpawnDeferred(side, position, count);
            FinishSpawn(mobs);
            return mobs;
        }
        
        public List<MobController> SpawnDeferred(Side side, Vector3 position, int count = 1 , float randFactor = 0)
        {
            List<MobController> spawnedMobs = new List<MobController>();
            for (int i = 0; i < count; i++)
            {
                Vector3 rand = Random.insideUnitSphere;
                rand.y = 0;
                MobController mob = side == Side.Friend ? _friendMobPool.Get() : _enemyMobPool.Get();
                mob.transform.position = position +  rand * randFactor;
                spawnedMobs.Add(mob);
                
            }
            return spawnedMobs;
        }

        public void FinishSpawn(List<MobController> mobControllers)
        {
            foreach (MobController mobController in mobControllers)
            {
                mobController.gameObject.SetActive(true);
                mobController.StartMove();
                if (mobController.Side == Side.Friend)
                {
                    ActiveFriendMobs.Add(mobController);
                }
                else
                {
                    ActiveEnemyMobs.Add(mobController);
                }
            }
        }

        public void RemoveAllFriendlies()
        {
            foreach (MobController mob in ActiveFriendMobs)
            {
                if(!mob.released)
                {
                    _friendMobPool.Release(mob);
                }
            }

            ActiveFriendMobs.Clear();
        }
        
        public void RemoveAllEnemies()
        {
            foreach (MobController mob in ActiveEnemyMobs)
            {
                if (!mob.released)
                {
                    _enemyMobPool.Release(mob);
                }
            }

            ActiveFriendMobs.Clear();
        }

        public void RemoveAll()
        {
            RemoveAllFriendlies();
            RemoveAllEnemies();
        }
    }
}
