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
        get { return session.score; }
        private set
        {
            session.score = value;
            NewScore?.Invoke(this, value);
        }
    }

    [Title("Score Settings")]
    [SerializeField] private int scorePerThrash = 0;

    [Title("UI")]
    [SerializeField, Required, SceneObjectsOnlyAttribute] private TextMeshProUGUI _scoreUI = null;

    private PlaySession session;



    private void Start()
    {
        session = FindObjectOfType<PlaySession>();
        NewScore += updateUI;

        updateUI(null, score);
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
