using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;
    public int Score { get { return _score; } }

    private int _score = 0;

    public void AddScore(int score)
    {
        _score += score;
        _scoreText.SetText($"{_score}");
    }

    public void ClearScore()
    {
        _score = 0;
        _scoreText.SetText($"{_score}");
    }
}
