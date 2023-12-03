using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cannon;
using Cysharp.Threading.Tasks;
using Mob;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private MobManager mobManager;
    [SerializeField] private EnemyCastleManager enemyCastleManager;
    [SerializeField] private CannonController cannonController;

    private CancellationTokenSource _cannonFireCancellationToken;
    
    private readonly InputManager _inputManager = new InputManager();
    
    void Start()
    {
        InitializeInput();
        InitializeCannon();
        InitializeMob();
        InitializeEnemyCastle();
    }

    private void InitializeInput()
    {
        _inputManager.Initialize();
    }

    private void InitializeCannon()
    {
        cannonController.Initialize();
        
        _inputManager.TouchMoveReceived += MoveCannonXCallback;
        _inputManager.TouchStarted += CannonStartFire;
        _inputManager.TouchEnded += CannonEndFire;
        
        
        cannonController.FireBehaviour.FiredAction += SpawnFriendlyMob;
    }
    
    private void CannonEndFire()
    {
        _cannonFireCancellationToken?.Cancel();
    }

    private void CannonStartFire()
    {
        _cannonFireCancellationToken?.Dispose();
        _cannonFireCancellationToken = new CancellationTokenSource();
        cannonController.FireBehaviour.KeepFiring(_cannonFireCancellationToken.Token).Forget();
    }

    private void InitializeEnemyCastle()
    {
        enemyCastleManager.Initialize();
        EnemyCastleManager.CastleSpawnTimeReached += CastleSpawnEnemyMob;
    }

    private void CastleSpawnEnemyMob(EnemyCastleController castle, Vector3 position)
    {
        mobManager.SpawnEnemy(position);
    }

    private void InitializeMob()
    {
        mobManager.Initialize();
    }

    private void SpawnFriendlyMob()
    {
        mobManager.SpawnFriendly(cannonController.FireBehaviour.SpawnPoint.position);
    }

    private void MoveCannonXCallback(Vector2 delta)         
    {
        cannonController.MovementBehaviour.MoveCannonX(delta.x);
    }
}
