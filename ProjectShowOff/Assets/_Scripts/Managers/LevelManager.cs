using Sirenix.OdinInspector;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private string _levelToLoad = "";
    [SerializeField, FoldoutGroup("Optional")] private ThrashSpawner _spawner = null;
    [SerializeField, FoldoutGroup("Optional")] private TutorialLevelManager _tutorial = null;
    [SerializeField, FoldoutGroup("Optional")] private Animator _barierAnimator = null;
    [SerializeField, FoldoutGroup("Optional")] private GameObject _indicatorArrows = null;


    [Title("Scene transition")]
    [SerializeField] private string _sceneTransition = null;
    [SerializeField] private float _sceneLoadDelay = 2;

    private bool _sceneLoaded = false;


    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }


    void Update()
    {
        if (_spawner != null)
        {
            if (_spawner.state == ThrashSpawner.SpawnerState.idle && trashCount() <= 0)
            {
                if (_tutorial != null && _tutorial._tutorialState != TutorialLevelManager.state.done) return;
                openBarier();
            }
        }
    }


    public void LoadLevel()
    {
        if (!_sceneLoaded)
        {
            _sceneLoaded = true;
            updateAchievements();

            StartCoroutine(delayedSceneLoad(SceneManager.GetActiveScene()));
            SceneManager.LoadSceneAsync(_sceneTransition, LoadSceneMode.Additive);

            StartCoroutine(unloadTransition(6));
        }
    }


    private IEnumerator delayedSceneLoad(Scene pOldScene)
    {
        yield return new WaitForSeconds(_sceneLoadDelay);
        yield return new WaitWhile(() => SceneManager.UnloadSceneAsync(pOldScene).isDone);
        SceneManager.LoadSceneAsync(_levelToLoad, LoadSceneMode.Additive);

    }


    private IEnumerator unloadTransition(float pDelay)
    {
        yield return new WaitForSeconds(pDelay);
        SceneManager.UnloadSceneAsync(_sceneTransition);
        Destroy(gameObject);
    }



    private void openBarier()
    {
        if (_barierAnimator != null) _barierAnimator.SetTrigger("Open");
        if (_indicatorArrows != null) _indicatorArrows.SetActive(true);
    }


    private int trashCount()
    {
        Thrash[] thrash = FindObjectsOfType<Thrash>();
        return thrash.Count();
    }


    private void updateAchievements()
    {
        AchievementManager achievements = FindObjectOfType<AchievementManager>();
        if (achievements != null)
        {
            int fishSaved = FindObjectsOfType<Fish>().Length;
            achievements.EndOfLevel(fishSaved);
        }
    }
}
