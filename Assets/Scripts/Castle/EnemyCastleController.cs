using System;
using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using Mob;
using TMPro;
using UnityEngine;

namespace Castle
{
    public class EnemyCastleController : MonoBehaviour
    {
        public static Action<EnemyCastleController, MobController> FriendMobEnteredArea;
        public static Action<EnemyCastleController> CastleDestroyed;
    
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private TMP_Text healthText;
    
        private int _health;
        private float _spawnInterval;
        private CancellationTokenSource _spawnCancellationToken;
        private Transform _transform;
        private bool _destroyed;

        private void Start()
        {
            EnemyCastleManager.SubscribeCastle(this);
        }

        public Vector3 GetPosition()
        {
            return _transform.position;
        }

        public void TakeDamage(int damage)
        {
            _health -= damage;
            if(_health <= 0)
            {
                DestroyCastle();
            }

            healthText.text = _health.ToString();
        }
        
        public void Initialize(int health, float spawnInterval)
        {
            _destroyed = false;
            _transform = transform;
            _health = health;
            _spawnInterval = spawnInterval;
            healthText.text = _health.ToString();
            gameObject.SetActive(true);
            ResetSpawnTimer();
        }

        private void DestroyCastle()
        {
            if (_destroyed)
            {
                return;
            }

            _destroyed = true;
            gameObject.SetActive(false);
            CastleDestroyed?.Invoke(this);
        }

        private void ResetSpawnTimer()
        {
            _spawnCancellationToken?.Dispose();
            _spawnCancellationToken = new CancellationTokenSource();
            StartCoroutine(StartSpawnTimer());
        }

        private void OnDestroy()
        {
            _spawnCancellationToken.Cancel();
        }
        private IEnumerator StartSpawnTimer()
        {
            while (_health > 0)
            {
                yield return new WaitForSeconds(_spawnInterval);
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
