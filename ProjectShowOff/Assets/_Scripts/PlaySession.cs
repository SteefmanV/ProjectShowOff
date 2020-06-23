using System.Linq;
using UnityEngine;

public class PlaySession : MonoBehaviour
{
    public int score = 0;

    public int[] _scores = new int[4];

    private static PlaySession _instance;
    public PlaySession instance { get { return _instance; } }

    private void Awake()
    {
        if (instance != this && _instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }


    public void StartNewSession()
    {
        score = 0;
        _scores = new int[4];
    }


    public void SetScores(int pLevelIndex, int pScore)
    {
        _scores[pLevelIndex] = pScore;
    }


    public int[] GetScores()
    {
        return _scores;
    }


    public int GetTotalScore()
    {
        return _scores.Sum();
    }
}
