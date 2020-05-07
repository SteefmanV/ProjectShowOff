using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.PlayerLoop;

public class ItemCollectionManager : MonoBehaviour
{
    public enum Item { bottle }
    private Dictionary<Item, int> _itemsCollected = new Dictionary<Item, int>();
    private List<Item> _justCollected = new List<Item>();
    private PowerUp _powerUp = null;

    [SerializeField] private TextMeshProUGUI _bottleUI = null;


    private void Start()
    {
        _powerUp = FindObjectOfType<PowerUp>();
        setupItems();
        updateUI();
    }


    /// <summary>
    /// Registers a collected Item
    /// </summary>
    public void CollectedItem(Item pItem)
    {
        _itemsCollected[pItem]++;
        _justCollected.Add(pItem);

        checkForPowerUp();
        updateUI();
    }


    private void setupItems()
    {
        _itemsCollected.Add(Item.bottle, 0);
    }


    private void checkForPowerUp()
    {
        if (netPowerUp()) return;
    }


    private bool netPowerUp()
    {
        if(getItemInListCount(Item.bottle) >= 3)
        {
            _justCollected.Clear();
            _powerUp.ActivateNet();
            return true;
        }

        return false;
    }


    private int getItemInListCount(Item pItem)
    {
        int itemCount = 0;
        foreach(Item item in _justCollected)
        {
            if (item == pItem) itemCount++;            
        }

        return itemCount;
    }


    private void updateUI()
    {
        _bottleUI.text = getItemInListCount(Item.bottle) + " / 3";
    }
}
