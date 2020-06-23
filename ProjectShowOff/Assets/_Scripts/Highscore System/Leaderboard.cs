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
    [SerializeField] private GameObject _dailyHighscorePrefab = null;
    [SerializeField] private GameObject _alltimeHighscorePrefab = null;
    [SerializeField] private Transform _dailyHighscoreHolder = null;
    [SerializeField] private Transform _globalHighscoreHolder = null;
    [SerializeField] private TMP_InputField _nameInputField = null;
    [SerializeField] private TextMeshProUGUI _PlayersScore = null;
    [SerializeField] private TextMeshProUGUI _fishSavedScore = null;
    [SerializeField] private TextMeshProUGUI _powerupsUseddScore = null;



    private void Start()
    {
        _session = FindObjectOfType<PlaySession>();
        _highscoreManager = FindObjectOfType<HighscoresManager>();

        loadDailyLeaderboard();
        loadGlobalLeaderboard();
        loadPlayerStats();
    }


    public void InsertScore()
    {
        if (_highscoreManager != null)
        {
            _highscoreManager.InsertNewScore(_session.score, _nameInputField.text);
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
            HighscoreItem item = Instantiate(_alltimeHighscorePrefab, _globalHighscoreHolder).GetComponent<HighscoreItem>();
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
            HighscoreItem item = Instantiate(_dailyHighscorePrefab, _dailyHighscoreHolder).GetComponent<HighscoreItem>();
            item.Initialize(index, score.name, score.score);
        }
    }


    private void loadPlayerStats()
    {
        PlaySession session = FindObjectOfType<PlaySession>();
        if (session != null) _PlayersScore.text = session.score.ToString();

        AchievementManager achievement = FindObjectOfType<AchievementManager>();

        if (achievement != null)
        {
            _powerupsUseddScore.text = achievement.powerupsUsed.ToString();
            _fishSavedScore.text = achievement.fishSaved.ToString();
        }
    }


    private void cleanLeaderboards()
    {
        foreach (Transform trans in _dailyHighscoreHolder)
        {
            Destroy(trans.gameObject);
        }

        foreach (Transform trans in _globalHighscoreHolder)
        {
            Destroy(trans.gameObject);
        }
    }
}
