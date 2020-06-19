using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AchievementManager : MonoBehaviour
{
    [ShowInInspector, ReadOnly] public int fishSaved { get; private set; } = 0;
    [ShowInInspector, ReadOnly] public int powerupsUsed { get; private set; } = 0;

    [SerializeField] private PowerUp _powerUp = null;


    private void Start()
    {
        checkPowerupReference(); 
        SceneManager.sceneLoaded += onSceneLoaded;
    }


    public void EndOfLevel(int pSavedFish)
    {
        fishSaved += pSavedFish;
    }


    public void ResetAchievements()
    {
        fishSaved = 0;
        powerupsUsed = 0;
    }


    private void onPowerupUsed(object pSender, PowerUp.PowerUps poweupUp)
    {
        powerupsUsed++;
    }


    public void onSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        checkPowerupReference();
    }


    private void checkPowerupReference()
    {
        if(_powerUp == null)
        {
            _powerUp = FindObjectOfType<PowerUp>();
            if (_powerUp == null) /*Debug.Log("<color=red><b>Could not find PowerUp in scene.</b></color>")*/;
            else
            {
                _powerUp.OnPowerupUsed += onPowerupUsed;
            }
        }
    }


    private void OnDestroy()
    {
        if(_powerUp != null) _powerUp.OnPowerupUsed -= onPowerupUsed;
        SceneManager.sceneLoaded -= onSceneLoaded;
    }
}
