using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Button startBtn;

    [SerializeField] private Transform enemyPoint1;
    [SerializeField] private Transform enemyPoint2;
    [SerializeField] private Transform friendPoint1;

    [SerializeField] private GameObject friendObject;
    [SerializeField] private GameObject enemyObject;
    // Start is called before the first frame update
    void Start()
    {
        startBtn.onClick.RemoveAllListeners();
        startBtn.onClick.AddListener(OnStartBtnClicked);
        ShowAnimation();
    }

    private void ShowAnimation()
    {
        enemyObject.transform.DOMove(enemyPoint1.transform.position, 2).SetSpeedBased(true).SetEase(Ease.Linear).OnComplete(() =>
        {
            enemyObject.transform.Rotate(Vector3.up, 180);
            friendObject.transform.DOMove(friendPoint1.position, 6).SetSpeedBased(true).SetEase(Ease.Linear);
            enemyObject.transform.DOMove(enemyPoint2.position, 6).SetSpeedBased(true).SetEase(Ease.Linear);
        });
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
