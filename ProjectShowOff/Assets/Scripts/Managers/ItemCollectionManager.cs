using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.PlayerLoop;

public class ItemCollectionManager : MonoBehaviour
{
    public enum Item { bottle, rings, straw }
    private Dictionary<Item, int> _itemsCollected = new Dictionary<Item, int>();
    private List<Item> _justCollected = new List<Item>();
    private PowerUp _powerUp = null;

    [SerializeField] private GameObject bottle1;
    [SerializeField] private GameObject bottle2;
    [SerializeField] private GameObject bottle3;


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


    public void ResetCount()
    {
        _justCollected.Clear();
        updateUI();
    }


    private void setupItems()
    {
        _itemsCollected.Add(Item.bottle, 0);
        _itemsCollected.Add(Item.rings, 0);
        _itemsCollected.Add(Item.straw, 0);
    }


    private void checkForPowerUp()
    {
        if (netPowerUp()) return;
        else if (bubblePackPowerUp()) return;
        else if (airTrapPowerUp()) return;
    }


    private bool netPowerUp()
    {
        if(getItemInListCount(Item.rings) >= 3)
        {           
            _powerUp.ActivateNet();
            return true;
        }

        return false;
    }


    private bool bubblePackPowerUp()
    {
        if (getItemInListCount(Item.bottle) >= 1 && getItemInListCount(Item.rings) >= 1)
        {
            _powerUp.ActivateBubblePack();
            return true;
        }

        return false;
    }

    private bool airTrapPowerUp()
    {
        if (getItemInListCount(Item.bottle) >= 3)
        {
            _powerUp.ActivateAirTrap();
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
        bottle1.SetActive(false);
        bottle2.SetActive(false);
        bottle3.SetActive(false);

        int count = getItemInListCount(Item.bottle);
        if (count >= 1) bottle1.SetActive(true);
        if (count >= 2) bottle2.SetActive(true);
        if (count >= 3) bottle3.SetActive(true);
    }
}
