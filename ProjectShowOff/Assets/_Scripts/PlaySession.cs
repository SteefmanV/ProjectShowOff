using UnityEngine;

public class PlaySession : MonoBehaviour
{
    public int score = 0;

    private static PlaySession _instance;
    public PlaySession instance { get { return _instance; } }

    private void Awake()
    {
        if(instance != this && _instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }


    public void StartNewSession()
    {
        score = 0;
    }
}
