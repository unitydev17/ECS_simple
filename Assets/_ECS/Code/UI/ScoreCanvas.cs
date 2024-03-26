using TMPro;
using UnityEngine;

public class ScoreCanvas : MonoBehaviour
{
    [SerializeField] private TMP_Text _playerScore;
    [SerializeField] private TMP_Text _botScore;


    public void SetScore(int playerScore, int botScore)
    {
        _playerScore.text = playerScore.ToString();
        _botScore.text = botScore.ToString();
    }
}