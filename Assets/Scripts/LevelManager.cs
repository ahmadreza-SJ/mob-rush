using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private CannonController cannonController;
    
    
    private readonly InputManager _inputManager = new InputManager();
    
    void Start()
    {
        InitializeInput();
        InitializeCannon();
    }

    private void InitializeInput()
    {
        
        _inputManager.Initialize();
    }

    private void InitializeCannon()
    {
        _inputManager.TouchMoveReceived += MoveCannonXCallback;
    }

    private void MoveCannonXCallback(Vector2 delta)         
    {
        cannonController.MovementBehaviour.MoveCannonX(delta.x);
    }
}
