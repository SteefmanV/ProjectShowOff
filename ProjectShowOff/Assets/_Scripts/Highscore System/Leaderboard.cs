using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    private PlaySession _session;
    private HighscoresManager _highscoreManager;

    [Title("Ui elements")]
    [SerializeField] private GameObject _highscorePrefab = null;
    [SerializeField] private Transform _dailyHighscoreHolder = null;
    [SerializeField] private Transform _globalHighscoreHolder = null;
    [SerializeField] private TMP_InputField _nameInputField = null;




    private void Start()
    {
        _session = FindObjectOfType<PlaySession>();
        _highscoreManager = FindObjectOfType<HighscoresManager>();

        loadDailyLeaderboard();
        loadGlobalLeaderboard();
    }


    public void InsertRandomScore()
    {
        _highscoreManager.InsertNewScore(Random.Range(0, 100), _nameInputField.text);
        cleanLeaderboards();
        loadDailyLeaderboard();
        loadGlobalLeaderboard();
        _nameInputField.text = "";
    }


    private void insertScore()
    {
        if (_highscoreManager != null)
        {
            _highscoreManager.InsertNewScore(_session.score, _nameInputField.name);
            cleanLeaderboards();
            loadDailyLeaderboard();
            loadGlobalLeaderboard();
            _nameInputField.text = "";
        }
    }


    private void loadGlobalLeaderboard()
    {
        List<Score> globalLeaderboard = _highscoreManager.GetGlobalLeaderboardData();

        int index = 0;
        foreach (Score score in globalLeaderboard)
        {
            index++;
            HighscoreItem item = Instantiate(_highscorePrefab, _globalHighscoreHolder).GetComponent<HighscoreItem>();
            item.Initialize(index, score.name, score.score);
        }
    }


    private void loadDailyLeaderboard()
    {
        List<Score> dailyLeaderboard = _highscoreManager.GetDailyLeaderboardData();

        int index = 0;
        foreach (Score score in dailyLeaderboard)
        {
            index++;
            HighscoreItem item = Instantiate(_highscorePrefab, _dailyHighscoreHolder).GetComponent<HighscoreItem>();
            item.Initialize(index, score.name, score.score);
        }
    }


    private void cleanLeaderboards()
    {
        foreach(Transform trans in _dailyHighscoreHolder)
        {
            Destroy(trans.gameObject);
        }

        foreach(Transform trans in _globalHighscoreHolder)
        {
            Destroy(trans.gameObject);
        }
    }
}
