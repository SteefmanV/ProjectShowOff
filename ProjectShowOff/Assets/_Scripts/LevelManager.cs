using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private ThrashSpawner _spawner = null;
    [SerializeField] private Animator _sceneSwitcher = null;
    [SerializeField] private string _levelToLoad = "";
    [SerializeField] private TutorialLevelManager _tutorial = null;
    [SerializeField] private Animator _barierAnimator = null;
    [SerializeField] private GameObject _indicatorArrows = null;

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
            openBarier();
        }
    }


    public void LoadLevel()
    {
        if (!_sceneLoaded)
        {
            _sceneLoaded = true;
            _sceneSwitcher.SetTrigger("end");
            StartCoroutine(delayedSceneLoad());
        }
    }


    private IEnumerator delayedSceneLoad()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadSceneAsync(_levelToLoad);
    }


    private void openBarier()
    {
        _barierAnimator.SetTrigger("Open");
        if(_indicatorArrows != null) _indicatorArrows.SetActive(true);
    }



    private int trashCount()
    {
        Thrash[] thrash = FindObjectsOfType<Thrash>();
        return thrash.Count();
    }
}
