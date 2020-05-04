using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int score { get; private set; }
    [SerializeField] private TextMeshProUGUI _score = null;
    [Header("Score Settings")]
    [SerializeField] private int _scorePerThrash = 10;

    private void Start()
    {
        score = 0;
        updateText();
    }


    public void ThrashDestroyed()
    {
        AddPoints(_scorePerThrash);
    }


    public void AddPoints(int pPoints)
    {
        score += pPoints;
        updateText();
    }

    private void updateText()
    {
        _score.text = "Score: " + score;
    }
}
