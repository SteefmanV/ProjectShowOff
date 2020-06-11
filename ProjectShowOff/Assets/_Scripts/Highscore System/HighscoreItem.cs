using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighscoreItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _highscoreIndex = null;
    [SerializeField] private TextMeshProUGUI _playerName = null;
    [SerializeField] private TextMeshProUGUI _score = null;

    public void Initialize(int pIndex, string pPlayerName, int pScore)
    {
        _highscoreIndex.text = pIndex.ToString();
        _playerName.text = pPlayerName.ToString();
        _score.text = pScore.ToString();
    }
}
