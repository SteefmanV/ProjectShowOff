using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class ItemCollectionManager : MonoBehaviour
{
    public enum Item { empty, bottle, rings, straw }
    private Dictionary<Item, int> _itemsCollected = new Dictionary<Item, int>();
    private List<Item> _justCollected = new List<Item>();
    private PowerUp _powerUp;

    [SerializeField] private Transform[] _uiHolders = new Transform[4];

    [Title("UI Icon Prefabs")]
    [SerializeField] private GameObject _bottleIcon = null;
    [SerializeField] private GameObject _ringsIcon = null;
    [SerializeField] private GameObject _strawIcon = null;

    [Title("Power-up Combinations")]
    [SerializeField] private List<Combination> powerUps = new List<Combination>();


    private void Start()
    {
        _powerUp = FindObjectOfType<PowerUp>();
        setupItems();
        updateUI(null);
    }


    /// <summary>
    /// Registers a collected Item
    /// </summary>
    public void CollectedItem(Item pItem)
    {
        _itemsCollected[pItem]++;
        _justCollected.Add(pItem);

        checkForPowerUp();
    }


    public void ResetCount()
    {
        _justCollected.Clear();
        updateUI(null);
    }


    private void setupItems()
    {
        _itemsCollected.Add(Item.bottle, 0);
        _itemsCollected.Add(Item.rings, 0);
        _itemsCollected.Add(Item.straw, 0);
        _itemsCollected.Add(Item.empty, 0);
    }


    private void checkForPowerUp()
    {
        Combination combination = null;
        foreach(Combination combi in powerUps)
        {
            if (checkCombinations(combi))
            {
                _powerUp.ActivatePowerup(combi.type);
                combination = combi;
                break;
            }
        }

        updateUI(combination);
    }


    private bool checkCombinations(Combination pCombi)
    {
        return pCombi.hasCombination(getItemInListCount(Item.bottle), getItemInListCount(Item.straw), getItemInListCount(Item.rings));
    }


    private int getItemInListCount(Item pItem)
    {
        int itemCount = 0;
        foreach (Item item in _justCollected)
        {
            if (item == pItem) itemCount++;
        }

        return itemCount;
    }


    private void updateUI(Combination pPowerUp)
    {
        cleanUI();

        for (int i = 0; i < 3; i++) // Put Item icon in each UI slot
        {
            Item item = (_justCollected.Count - 1 >= i)? _justCollected[i] : Item.empty;
            GameObject iconObject = null;

            switch (item)
            {
                case Item.bottle:
                    iconObject = _bottleIcon;
                    break;
                case Item.rings:
                    iconObject = _ringsIcon;
                    break;
                case Item.straw:
                    iconObject = _strawIcon;
                    break;
                case Item.empty:
                    // do nothing
                    break;
            }

            if(iconObject != null) Instantiate(iconObject, _uiHolders[i]);
        }

        if(pPowerUp != null) Instantiate(pPowerUp.powerUpIcon, _uiHolders[3]); // Put powerup in UI slot
    }


    private void cleanUI()
    {
        for (int i = 0; i < _uiHolders.Length; i++)
        {
            foreach(Transform trans in _uiHolders[i])
            {
                if (!trans.CompareTag("backgroundUi"))
                {
                    Destroy(trans.gameObject); // Destroy all children in UI Holder
                }
            }
        }
    }
}
