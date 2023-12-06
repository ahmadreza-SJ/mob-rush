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
        private Transform _transform;
        private bool _destroyed;
        private bool _spawnStopped;
        private Coroutine _spawnCoroutine;

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
            _spawnStopped = false;
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

            _spawnStopped = true;
            _destroyed = true;
            gameObject.SetActive(false);
            CastleDestroyed?.Invoke(this);
        }

        private void ResetSpawnTimer()
        {
            _spawnCoroutine = StartCoroutine(StartSpawnTimer());
        }

        public void StopSpawning()
        {
            _spawnStopped = true;
            if(_spawnCoroutine != null)
            {
                StopCoroutine(_spawnCoroutine);
            }

        }
        
        private IEnumerator StartSpawnTimer()
        {
            while (!_spawnStopped)
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
