using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelManager : MonoBehaviour
{
    
    private readonly InputManager _inputManager = new InputManager();
    
    void Start()
    {
        _inputManager.Initialize();
    }

    
    

   
}
