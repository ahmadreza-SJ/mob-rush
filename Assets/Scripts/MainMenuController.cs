using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Button startBtn;
    
    // Start is called before the first frame update
    void Start()
    {
        startBtn.onClick.RemoveAllListeners();
        startBtn.onClick.AddListener(OnStartBtnClicked);
    }

    private void OnStartBtnClicked()
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync("Game");
        StartCoroutine(ShowLoadProgress(loadOperation));

    }

    private IEnumerator ShowLoadProgress(AsyncOperation loadOperation)
    {
        while (loadOperation.progress < 1)
        {
            yield return null;
            Debug.Log(loadOperation.progress);
        }
    }
}
