using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Mob;
using UnityEngine;

namespace Castle
{
    public class EnemyCastleController : MonoBehaviour
    {
        public static Action<EnemyCastleController, MobController> FriendMobEnteredArea;
        public static Action<EnemyCastleController> CastleDestroyed;
    
        [SerializeField] private Transform spawnPoint;
    
        private int _health;
        private float _spawnInterval;
        private CancellationTokenSource _spawnCancellationToken;
        private Transform _transform;

        public Vector3 GetPosition()
        {
            return _transform.position;
        }
        
        public void Initialize(int health, float spawnInterval)
        {
            _transform = transform;
            _health = health;
            _spawnInterval = spawnInterval;
            ResetSpawnTimer();
        }

        private void OnDestroy()
        {
            CastleDestroyed?.Invoke(this);
            _spawnCancellationToken.Cancel();
        }

        private void ResetSpawnTimer()
        {
            _spawnCancellationToken?.Dispose();
            _spawnCancellationToken = new CancellationTokenSource();
            StartSpawnTimer().Forget();
        }

        private async UniTask StartSpawnTimer()
        {
            while (true)
            {
                if (_spawnCancellationToken.IsCancellationRequested)
                {
                    break;
                }

                await UniTask.WaitForSeconds(_spawnInterval, cancellationToken: _spawnCancellationToken.Token);
                EnemyCastleManager.CastleSpawnTimeReached?.Invoke(this, spawnPoint.position);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out MobController mobController))
            {
                if (mobController.Side == Side.Friend)
                {
                    FriendMobEnteredArea?.Invoke(this, mobController);
                }
            }
        }
    }
}
