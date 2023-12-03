using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

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
        
        public void Initialize()
        {
            _friendMobPool = new ObjectPool<MobController>((() =>
            {
                MobController instance = Object.Instantiate(friendlyMobPrefab, friendMobParent)
                    .GetComponent<MobController>();
                instance.Initialize();
                return instance;
            }));
            
            _enemyMobPool = new ObjectPool<MobController>((() =>
            {
                MobController instance = Object.Instantiate(enemyMobPrefab, enemyMobParent)
                    .GetComponent<MobController>();
                instance.Initialize();
                return instance;
            }));
        }

        public void SpawnFriendly(Vector3 position)
        {
            MobController mob = _friendMobPool.Get();
            mob.transform.position = position;
            mob.gameObject.SetActive(true);
            mob.StartMove();
        }
        
        public void SpawnEnemy(Vector3 position)
        {
            MobController mob = _enemyMobPool.Get();
            mob.transform.position = position;
            mob.gameObject.SetActive(true);
            mob.StartMove();
        }
    }
}
