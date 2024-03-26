using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class UI : MonoBehaviour
{
    [SerializeField] private ScoreCanvas _scoreCanvas;
    private Action _callback;


    public void CloseScore()
    {
        _scoreCanvas.gameObject.SetActive(false);
        _callback?.Invoke();
    }

    public void OpenScore(Action closeCallback)
    {
        _callback = closeCallback;
        _scoreCanvas.SetScore(Random.Range(1, 9), Random.Range(1, 9));
        _scoreCanvas.gameObject.SetActive(true);
    }
}