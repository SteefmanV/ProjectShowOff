using Sirenix.OdinInspector;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField, Required, SceneObjectsOnlyAttribute]
    private GameObject _gameOverPanel = null;

    [SerializeField, Required, SceneObjectsOnlyAttribute]
    private GameObject _gameWonPanel = null;


    public void GameOver()
    {
        _gameOverPanel.SetActive(true);
    }


    public void GameWon()
    {
        _gameWonPanel.SetActive(true);
    }
}

