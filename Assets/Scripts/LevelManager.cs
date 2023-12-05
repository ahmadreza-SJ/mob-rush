using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cannon;
using Castle;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Gate;
using Mob;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-1)]
public class LevelManager : MonoBehaviour
{
    [SerializeField] private MobManager mobManager;
    [SerializeField] private EnemyCastleManager enemyCastleManager;
    [SerializeField] private GateManager gateManager;
    [SerializeField] private CannonController cannonController;
    [SerializeField] private PopUpController winPopUpController;
    [SerializeField] private PopUpController losePopUpController;

    private CancellationTokenSource _cannonFireCancellationToken;
    
    private readonly InputManager _inputManager = new InputManager();
    
    void Start()
    {
        InitializePopUps();
        InitializeInput();
        InitializeCannon();
        InitializeMob();
        InitializeEnemyCastle();
        InitializeGates();
    }

    private void OnDestroy()
    {
        
        winPopUpController.RestartBtnClicked -= PopUpControllerOnRestartBtnClicked;
        losePopUpController.RestartBtnClicked -= PopUpControllerOnRestartBtnClicked;
        winPopUpController.MenuBtnClicked -= PopUpControllerOnMenuBtnClicked;
        losePopUpController.MenuBtnClicked -= PopUpControllerOnMenuBtnClicked;
        
        _inputManager.TouchMoveReceived -= MoveCannonXCallback;
        _inputManager.TouchStarted -= CannonStartFire;
        _inputManager.TouchEnded -= CannonEndFire;
        
        
        cannonController.FireBehaviour.FiredAction -= SpawnFriendlyMob;
        
        GateController.MobPassed -= MobPassedGate;
        
        
        EnemyCastleManager.CastleSpawnTimeReached -= CastleSpawnEnemyMob;
        EnemyCastleController.FriendMobEnteredArea -= FriendMobEnteredCastleArea;
        EnemyCastleController.CastleDestroyed -= OnCastleDestroyed;
        
        
        MobCollisionBehaviour.Collided -= MobCollidedCallback;
    }

    private void ReInitialize()
    {
        _inputManager.ReInitialize();
        mobManager.ReInitialize();
        enemyCastleManager.ReInitialize();
        gateManager.ReInitialize();
    }

    private void InitializeInput()
    {
        _inputManager.Initialize();
    }

    private void InitializePopUps()
    {
        winPopUpController.Initialize();
        losePopUpController.Initialize();
        
        winPopUpController.RestartBtnClicked += PopUpControllerOnRestartBtnClicked;
        losePopUpController.RestartBtnClicked += PopUpControllerOnRestartBtnClicked;
        winPopUpController.MenuBtnClicked += PopUpControllerOnMenuBtnClicked;
        losePopUpController.MenuBtnClicked += PopUpControllerOnMenuBtnClicked;
        
        
        
        
    }

    private void PopUpControllerOnMenuBtnClicked()
    {

        SceneManager.LoadScene("MainMenu");
        winPopUpController.gameObject.SetActive(false);
        losePopUpController.gameObject.SetActive(false);
    }

    private void PopUpControllerOnRestartBtnClicked()
    {
        ReInitialize();
        winPopUpController.gameObject.SetActive(false);
        losePopUpController.gameObject.SetActive(false);
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
        EnemyCastleController.FriendMobEnteredArea += FriendMobEnteredCastleArea;
        EnemyCastleController.CastleDestroyed += OnCastleDestroyed;
    }

    private void OnCastleDestroyed(EnemyCastleController castle)
    {
        
        enemyCastleManager.RemoveCastle(castle);
        if (enemyCastleManager.Castles.Count == 0)
        {
            Win();
            return;
        }
        
        foreach (MobController mob in mobManager.ActiveFriendMobs)
        {
            if(mob.MovementBehaviour.TargetLocked && enemyCastleManager.TryGetNearestCastle(mob.transform.position, out EnemyCastleController nearestCastle))
            {
                mob.MovementBehaviour.ForceSetTarget(nearestCastle.GetPosition());
            }
        }
    }

    private void FriendMobEnteredCastleArea(EnemyCastleController castle, MobController mob)
    {
        mob.MovementBehaviour.SetTarget(castle.transform.position);
        mob.MovementBehaviour.LockTarget(true);
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
                MobController otherMc = other.GetComponent<MobController>();
                otherMc.ShowDissolve().OnComplete(() => mobManager.Remove(other.GetComponent<MobController>()));
                self.ShowDissolve().OnComplete(() => mobManager.Remove(self));
            }

            if (other.CompareTag("EnemyCastle"))
            {
                mobManager.Remove(self);
                EnemyCastleController castle = other.GetComponent<EnemyCastleController>();
                castle.TakeDamage(1);
            }
        }
        else
        {
            if (other.CompareTag("LoseTrigger"))
            {
                Lose();
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

    private void Lose()
    {
        Debug.Log("Lost");
        _inputManager.DisableGameplayInput();
        mobManager.RemoveAll();
        losePopUpController.gameObject.SetActive(true);
    }

    private void Win()
    {
        Debug.Log("Won");
        _inputManager.DisableGameplayInput();
        mobManager.RemoveAll();
        winPopUpController.gameObject.SetActive(true);
    }
}
