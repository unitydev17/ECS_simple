using System;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private ScoreCanvas _scoreCanvas;
    private Action _callback;


    public void CloseScore()
    {
        _scoreCanvas.gameObject.SetActive(false);
        _callback?.Invoke();
    }

    public void OpenScore(Vector2Int score, Action closeCallback)
    {
        _callback = closeCallback;
        _scoreCanvas.SetScore(score.x, score.y);
        _scoreCanvas.gameObject.SetActive(true);
    }
}