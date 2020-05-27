using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private ThrashSpawner _spawner = null;
    [SerializeField] private int _levelToLoad = 0;
    [SerializeField] private Animator _sceneSwitcher = null;

    private bool _sceneLoaded = false;


    private void Start()
    {
        _sceneSwitcher.SetTrigger("start");
    }


    void Update()
    {
        if (_spawner.state == ThrashSpawner.SpawnerState.idle && trashCount() <= 0)
        {
            if (!_sceneLoaded)
            {
                _sceneLoaded = true;
                _sceneSwitcher.SetTrigger("end");
                StartCoroutine(delayedSceneLoad());
            }
        }
        else
        {
            Debug.Log("Spawner state: " + _spawner.state + " trash count: " + trashCount());
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
