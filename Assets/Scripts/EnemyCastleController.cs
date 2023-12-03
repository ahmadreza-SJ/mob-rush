using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class EnemyCastleController : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    
    private int _health;
    private float _spawnInterval;
    private CancellationTokenSource _spawnCancellationToken;
    
    
    public void Initialize(int health, float spawnInterval)
    {
        _health = health;
        _spawnInterval = spawnInterval;
        ResetSpawnTimer();
    }

    private void OnDestroy()
    {
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
}
