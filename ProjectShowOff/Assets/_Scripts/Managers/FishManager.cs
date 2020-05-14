using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;


public class FishManager : MonoBehaviour
{
    [SerializeField, Required, SceneObjectsOnlyAttribute]
    private Transform fishHolder = null;

    [SerializeField] private int _damagePerThrash;

    private GameManager _gameManager;

    private void Awake()
    {
        _gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }


    public void CheckFishCount()
    {
        StartCoroutine(delayedFishCount());
    }


    // Delayed, because it's called before the fish is destroyed
    private IEnumerator delayedFishCount()
    {
        yield return new WaitForEndOfFrame();
        checkGameOver();
    }


    private void checkGameOver()
    {
        if(fishCount() <= 0)
        {
            _gameManager.GameOver();
        }
    }


    private int fishCount()
    {
        return fishHolder.childCount;
    }
}
