using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cannon;
using Cysharp.Threading.Tasks;
using Gate;
using Mob;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
public class LevelManager : MonoBehaviour
{
    [SerializeField] private MobManager mobManager;
    [SerializeField] private EnemyCastleManager enemyCastleManager;
    [SerializeField] private GateManager gateManager;
    [SerializeField] private CannonController cannonController;

    private CancellationTokenSource _cannonFireCancellationToken;
    
    private readonly InputManager _inputManager = new InputManager();
    
    void Start()
    {
        InitializeInput();
        InitializeCannon();
        InitializeMob();
        InitializeEnemyCastle();
        InitializeGates();
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

    private void InitializeGates()
    {
        gateManager.Initialize();
        GateController.MobPassed += MobPassedGate;
    }

    private void MobPassedGate(GateController gate, MobController mob, int count)
    {
        List<MobController> mobs = mobManager.SpawnDeferred(mob.Side, mob.transform.position, count, 0.1f);
        gate.IgnoreMob(mobs);
        mobManager.FinishSpawn(mobs);
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
        mobManager.Spawn(Side.Enemy, position);
    }

    private void InitializeMob()
    {
        mobManager.Initialize();
        MobCollisionBehaviour.Collided += MobCollidedCallback;
    }

    private void MobCollidedCallback(MobController self, GameObject other)
    {
        if (self.Side == Side.Friend)
        {
            if (other.CompareTag("EnemyMob"))
            {
                Destroy(self.gameObject);
                Destroy(other);
            }
        }
    }

    private void SpawnFriendlyMob()
    {
        mobManager.Spawn(Side.Friend, cannonController.FireBehaviour.SpawnPoint.position);
    }

    private void MoveCannonXCallback(Vector2 delta)         
    {
        cannonController.MovementBehaviour.MoveCannonX(delta.x);
    }
}
