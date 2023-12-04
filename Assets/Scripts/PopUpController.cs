using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PopUpController : MonoBehaviour
{
    [SerializeField] private Button restartBtn;
    [SerializeField] private Button menuBtn;

    public event Action RestartBtnClicked; 
    public event Action MenuBtnClicked; 
    
    
    public void Initialize()
    {
        restartBtn.onClick.RemoveAllListeners();
        menuBtn.onClick.RemoveAllListeners();
        
        restartBtn.onClick.AddListener(OnRestartClicked);
        menuBtn.onClick.AddListener(OnMenuClicked);
    }

    private void OnRestartClicked()
    {
        RestartBtnClicked?.Invoke();
    }

    private void OnMenuClicked()
    {
        MenuBtnClicked?.Invoke();
    }
}
