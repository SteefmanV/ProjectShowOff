using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;
using UnityEngine.PlayerLoop;
using System;

public class ScoreManager : MonoBehaviour
{
    public event EventHandler<int> NewScore;

    private int _score;
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
    [SerializeField] private int scorePerFishSaved = 15;
    [SerializeField] private int scorePerPowerupUsed = 5;

    [SerializeField] private int _levelIndex = 0;

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


    public void PowerupUsed()
    {
        Debug.Log("Powerup used");
        AddPoints(scorePerPowerupUsed);
    }


    public void ThrashDestroyed()
    {
        score += scorePerThrash;
    }


    public void FishSaved(int pFishSaved)
    {
        score += pFishSaved * scorePerFishSaved;
    }


    //public void PowerUpsUsed(int pPowerUpsUsed)
    //{
    //    score += pPowerUpsUsed * scorePerPowerupUsed;
    //}


    public void UpdateScore()
    {
        session.SetScores(_levelIndex, score);
    }


    private void updateUI(object pSender, int pScore)
    {
        _scoreUI.text = score.ToString();
    }
}
