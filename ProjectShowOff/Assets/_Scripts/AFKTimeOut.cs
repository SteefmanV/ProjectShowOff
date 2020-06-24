using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AFKTimeOut : MonoBehaviour
{
    [SerializeField] private float _resetGameAfterSeconds = 30;
    [SerializeField] private string _loadSceneWhenInactive = "MainScreen";

    [SerializeField, ReadOnly] private float _inactiveTimer = 0;
    private bool _inactive = false;


    void Update()
    {
        _inactiveTimer += Time.deltaTime;

        if(Input.GetMouseButton(0))
        {
            _inactiveTimer = 0;
        }

        if(_inactiveTimer > _resetGameAfterSeconds && !_inactive)
        {
            _inactive = true;
            SceneManager.LoadSceneAsync(_loadSceneWhenInactive);
        }
    }


    public void ResetTimer()
    {
        _inactiveTimer = 0;
    }
}
