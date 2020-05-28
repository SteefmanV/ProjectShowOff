using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private ThrashSpawner _spawner = null;
    [SerializeField] private int _levelToLoad = 0;
    [SerializeField] private Animator _sceneSwitcher = null;
    [SerializeField] private TutorialLevelManager _tutorial = null;

    private bool _sceneLoaded = false;


    private void Start()
    {
        _sceneSwitcher.SetTrigger("start");
    }


    void Update()
    {
        if (_spawner.state == ThrashSpawner.SpawnerState.idle && trashCount() <= 0)
        {
            if (_tutorial != null && _tutorial._tutorialState != TutorialLevelManager.state.done) return;

            if (!_sceneLoaded)
            {
                _sceneLoaded = true;
                _sceneSwitcher.SetTrigger("end");
                StartCoroutine(delayedSceneLoad());
            }
        }
    }


    private IEnumerator delayedSceneLoad()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadSceneAsync(_levelToLoad);
    }


    private int trashCount()
    {
        Thrash[] thrash = FindObjectsOfType<Thrash>();
        return thrash.Count();
    }
}
