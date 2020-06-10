using UnityEngine.SceneManagement;
using UnityEngine;

public class LoadLevelWhenTrigger : MonoBehaviour
{
    [SerializeField] private LevelManager _levelManager;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _levelManager.LoadLevel();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _levelManager.LoadLevel();
            other.gameObject.SetActive(false); // Disable player
        }
    }
}
