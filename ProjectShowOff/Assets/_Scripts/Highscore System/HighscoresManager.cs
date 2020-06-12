using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using System.IO;

public class HighscoresManager : MonoBehaviour
{
    [SerializeField] private List<Score> _globalScoreCache = new List<Score>();
    [SerializeField] private List<Score> _dailyHighscore = new List<Score>();
    private string scoresFilePath = "";

    const string relativeDirectory = "blob/stir";
    const string fileName = "highscores.json";

    [SerializeField] private List<string> randomNames = new List<string>();


    private void Awake()
    {
        scoresFilePath = getFilePath();
        _globalScoreCache = getScoresFromJson().scores;

        populateDailyHighscore();
    }


    /// <summary>
    /// Validates and inserts (if highscore) score into highscore list
    /// </summary>
    public void InsertNewScore(int pScore, string pName)
    {
        Score score = new Score(pScore, pName);
        // Global Leaderboard
        if (pScore > _globalScoreCache[24].score) // Score higher than lowest score in list
        {
            Debug.Log("<color=green> Score is in global highscore list </color>");

            _globalScoreCache.Add(score);
            _globalScoreCache = _globalScoreCache.OrderBy(x => -x.score).ToList();
            _globalScoreCache.Remove(_globalScoreCache[25]);

            saveToJson();
        }
        else
        {
            Debug.Log("<color=red> Score is not in global highscore list </color>");
        }

        // Daily leaderboard
        if (pScore > _dailyHighscore[9].score)
        {
            Debug.Log("<color=green> Score is in <b> DAILY </b> highscore list </color>");

            _dailyHighscore.Add(score);
            _dailyHighscore = _dailyHighscore.OrderBy(x => -x.score).ToList();

            if (_dailyHighscore.Count > 10)
            {
                _dailyHighscore.RemoveAt(10);
            }
        }
        else
        {
            Debug.Log("<color=red> Score is not in  <b> DAILY </b>  highscore list </color>");
        }
    }


    /// <summary>
    /// Returns copy of global leaderboard
    /// </summary>
    public List<Score> GetGlobalLeaderboardData()
    {
        return _globalScoreCache.ToList();
    }


    /// <summary>
    /// Returns copy of daily leaderboard
    /// </summary>
    public List<Score> GetDailyLeaderboardData()
    {
        return _dailyHighscore.ToList();
    }


    private void saveToJson()
    {
        Highscores highscores = new Highscores(_globalScoreCache);
        string json = JsonUtility.ToJson(highscores);
        System.IO.File.WriteAllText(scoresFilePath, json);
    }


    private Highscores getScoresFromJson()
    {
        try
        {
            string json = System.IO.File.ReadAllText(scoresFilePath);
            Highscores scores = JsonUtility.FromJson<Highscores>(json);
            return scores;
        }
        catch
        {
            return new Highscores();
        }
    }


    private string getFilePath()
    {
        string path = Path.Combine(Application.dataPath, relativeDirectory);

        if (!Directory.Exists(path)) Directory.CreateDirectory(path); // Make sure the path exists

        path = Path.Combine(path, fileName); // full path with filename
        return path;
    }


    private void populateDailyHighscore()
    {
        foreach(string name in randomNames)
        {
            _dailyHighscore.Add(new Score(UnityEngine.Random.Range(0,30), name));
        }
    }
}
