using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadLevelTimer : MonoBehaviour
{
    [Header("Load Level")]
    [SerializeField] private float _loadLevelafterSeconds = 1;
    [SerializeField] private string _levelToLoad = "";
    [SerializeField] private TextMeshProUGUI _countDownText = null;

    private AFKTimeOut _afkTimer = null;

    private float _timer = 0;
    private bool _levelLoaded = false;

    void Start()
    {
        _timer = _loadLevelafterSeconds;
        _afkTimer = FindObjectOfType<AFKTimeOut>();
    }


    private void Update()
    {
        _timer -= Time.deltaTime;

        if (_timer <= 0)
        {
            if (!_levelLoaded)
            {
                FindObjectOfType<LevelManager>().LoadLevel();
                _levelLoaded = true;
                _countDownText.text = "0";
            }
        }
        else
        {
            _countDownText.text = _timer.ToString("0.0");
        }

        if (_afkTimer != null) _afkTimer.ResetTimer();
    }


    private IEnumerator loadLevel(float pDuration)
    {
        yield return new WaitForSeconds(pDuration);

    }
}
