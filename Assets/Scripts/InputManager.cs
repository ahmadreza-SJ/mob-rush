using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class InputManager
{
    
    private GameInput _gameInput;
    private bool _isTouching;

    public event Action TouchStarted;
    public event Action TouchEnded;
    public event Action<Vector2> TouchMoveReceived; 
    
    public void Initialize()
    {
        _gameInput = new GameInput();
        _gameInput.Gameplay.Enable();
        _gameInput.Gameplay.PrimaryTouchContact.started += PrimaryContactOnStarted;
        _gameInput.Gameplay.PrimaryTouchContact.canceled += PrimaryContactOnCanceled;
    }

    public void ReInitialize()
    {
        _gameInput.Gameplay.Enable();
    }

    public void DisableGameplayInput()
    {
        _gameInput.Gameplay.Disable();
    }

    private void PrimaryContactOnCanceled(InputAction.CallbackContext ctx)
    {
        _isTouching = false;
        TouchEnded?.Invoke();
    }

    private void PrimaryContactOnStarted(InputAction.CallbackContext ctx)
    {
        _isTouching = true;
        GetDelta().Forget();
        TouchStarted?.Invoke();
    }
    

    private async UniTask GetDelta()
    {
        while (_isTouching)
        {
            Vector2 delta = _gameInput.Gameplay.PrimaryTouchDelta.ReadValue<Vector2>();
            if (delta.magnitude > 0)
            {
                TouchMoveReceived?.Invoke(delta);
            }
            
            await UniTask.Yield();
        }
    }
}
