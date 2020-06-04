using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;
using UnityEngine.PlayerLoop;
using System;

public class ScoreManager : MonoBehaviour
{
    public event EventHandler<int> NewScore;

    [ShowInInspector]
    public int score
    {
        get { return _score; }
        private set
        {
            _score = value;
            NewScore?.Invoke(this, value);
        }
    }

    [Title("Score Settings")]
    [SerializeField] private int scorePerThrash = 0;

    [Title("UI")]
    [SerializeField, Required, SceneObjectsOnlyAttribute] private TextMeshProUGUI _scoreUI = null;

    private int _score;




    private void Start()
    {
        NewScore += updateUI;
        score = 0;
    }


    public void AddPoints(int pValue)
    {
        if (pValue < 0) return;
        score += pValue;
    }


    public void ThrashDestroyed()
    {
        score += scorePerThrash;
    }


    private void updateUI(object pSender, int pScore)
    {
        _scoreUI.text = score.ToString();
    }
}
