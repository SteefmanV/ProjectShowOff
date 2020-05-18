using Sirenix.OdinInspector;
using UnityEngine;

public class BigBubblePowerUp : MonoBehaviour
{
    [ReadOnly] public bool bigBubbleActive = false;

    [SerializeField] private GameObject _player = null;

    private GameObject _bigBubble;


    private void Awake()
    {
        _bigBubble = _player.transform.Find("bigBubble").gameObject;
        _bigBubble.SetActive(false);
    }


    public void setUp()
    {
        bigBubbleActive = true;

        if (_bigBubble.activeSelf == false)
        {
            _bigBubble.SetActive(true);
        }
    }


    public void Land()
    {
        bigBubbleActive = false;

        if (_bigBubble.activeSelf == true)
        {
            _bigBubble.SetActive(false);
        }
    }
}
