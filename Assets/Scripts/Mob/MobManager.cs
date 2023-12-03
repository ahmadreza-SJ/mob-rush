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
        [SerializeField] private Transform friendMobParent;

        private ObjectPool<MobController> _friendMobPool;
        
        public void Initialize()
        {
            _friendMobPool = new ObjectPool<MobController>((() =>
            {
                MobController instance = Object.Instantiate(friendlyMobPrefab, friendMobParent)
                    .GetComponent<MobController>();
                instance.Initialize();
                return instance;
            }));
        }

        public void Spawn(Vector3 position)
        {
            MobController mob = _friendMobPool.Get();
            mob.transform.position = position;
            mob.gameObject.SetActive(true);
            mob.StartMove();
        }
    }
}
