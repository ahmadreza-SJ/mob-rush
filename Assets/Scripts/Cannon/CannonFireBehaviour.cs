using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Cannon
{
    public class CannonFireBehaviour : MonoBehaviour
    {

        [SerializeField] private Transform spawnPoint;
        [SerializeField] [Range(0, 5)] private float fireInterval;
        
        public event Action FiredAction;

        public Transform SpawnPoint => spawnPoint;
        
        private void Fire()
        {
            FiredAction?.Invoke();
        }

        public async UniTask KeepFiring(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                Fire();
                await UniTask.WaitForSeconds(fireInterval, cancellationToken:ct);
            }
        }

    }
}
