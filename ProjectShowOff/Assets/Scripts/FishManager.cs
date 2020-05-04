using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FishManager : MonoBehaviour
{
    [SerializeField] private Transform _fishHolder = null;
    [SerializeField] private GameObject gameOverScreen = null;
    [SerializeField] private TextMeshProUGUI _fishCount = null;

    public bool gameOver { get; private set; }

    private void Update()
    {
        updateFishState();
    }

    private void updateFishState()
    {
        if (gameOver) return;

        _fishCount.text = "Fish: " + getFishCount();

        if(getFishCount() <= 0)
        {
            gameOverScreen.SetActive(true);
        }
    }

    private int getFishCount()
    {
        return _fishHolder.childCount;
    }
}
