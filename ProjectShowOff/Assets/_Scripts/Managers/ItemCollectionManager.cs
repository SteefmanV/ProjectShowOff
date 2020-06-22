using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Linq;
using UnityEngine;

public class ItemCollectionManager : MonoBehaviour
{
    public enum Item { empty, bottle, rings, straw }
    private Dictionary<Item, int> _itemsCollected = new Dictionary<Item, int>();
    private List<Item> _justCollected = new List<Item>();
    private PowerUp _powerUp;

    [SerializeField, TabGroup("General")] private Transform[] _uiHolders = new Transform[4];

    [Title("Power-up Combinations"), TabGroup("General")]
    [SerializeField, TabGroup("General")] private List<Combination> powerUps = new List<Combination>();


    [Title("UI Icon Prefabs"), TabGroup("Visual-audio")]
    [SerializeField, TabGroup("Visual-audio")] private GameObject _bottleIcon = null;
    [SerializeField, TabGroup("Visual-audio")] private GameObject _ringsIcon = null;
    [SerializeField, TabGroup("Visual-audio")] private GameObject _strawIcon = null;

    [Title("Audio"), TabGroup("Visual-audio")]
    [SerializeField, TabGroup("Visual-audio")] private AudioClip _collectTrash = null;
    [SerializeField, TabGroup("Visual-audio")] private AudioClip _addItemToInventory = null;
    [SerializeField, TabGroup("Visual-audio")] private AudioClip _createPowerup = null;
    private AudioSource _audio;

    [Title("Particle effects"), TabGroup("Visual-audio")]
    [SerializeField, TabGroup("Visual-audio")] private ParticleSystem[] _itemParticleSystems = new ParticleSystem[3];
    [SerializeField, TabGroup("Visual-audio")] private ParticleSystem _powerupReady = null;
    [SerializeField, TabGroup("Visual-audio")] private ParticleSystem _powerupActive = null;

    private void Start()
    {
        _audio = GetComponent<AudioSource>();
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

        if (_justCollected.Count < 3) // If UI is not full
        {
            _justCollected.Add(pItem);

            ParticleSystem particle = null;
            if (_justCollected.Count <= _itemParticleSystems.Length)
            {
                particle = _itemParticleSystems[_justCollected.Count - 1];
                if (particle != null) particle.Play(); // Play particle effect in ui slot
            }


            if (_justCollected.Count == 3) // Powerup ready
            {
                playOneShot(_createPowerup);
            }
            else
            {
                playOneShot(_addItemToInventory);
            }
        }
        else
        {
            playOneShot(_collectTrash);
        }

        checkForPowerUp();
    }


    public void ResetCount()
    {
        _justCollected.Clear();
        updateUI(null);
        StopPowerupEffect();
    }


    public void ActivatePowerupEffect()
    {
        if (_justCollected.Count >= 3)
        {
            Debug.Log("Activate powperup effect!");
            _powerupReady.Stop();
            if (_powerupActive != null) _powerupActive.Play();
        }
    }


    private void StopPowerupEffect()
    {
        _powerupActive.Stop();
        _powerupReady.Stop();
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
        foreach (Combination combi in powerUps)
        {
            if (checkCombinations(combi))
            {
                _powerUp.PreparePowerup(combi.type);
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
        Debug.Log("Put items in slots");
        for (int i = 0; i < 3; i++) // Put Item icon in each UI slot
        {
            Item item = (_justCollected.Count - 1 >= i) ? _justCollected[i] : Item.empty;
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

            if (iconObject != null)
            {
                Instantiate(iconObject, _uiHolders[i]);
            }
        }

        if (pPowerUp != null)
        {
            Debug.Log("Powerup ready!");
            Instantiate(pPowerUp.powerUpIcon, _uiHolders[3]); // Put powerup in UI slot
            if (_powerupReady != null) _powerupReady.Play();
            Debug.Log("Powerup places");
        }
    }


    private void cleanUI()
    {
        Debug.Log("Clean UI");

        for (int i = 0; i < _uiHolders.Length; i++)
        {
            //if (_uiHolders[i] == null) continue;
            for (int j = 0; j < _uiHolders[i].transform.childCount; j++)
            {
                Transform trans = _uiHolders[i].transform.GetChild(j);
                if (trans != null && !trans.CompareTag("backgroundUi"))
                {
                    Destroy(trans.gameObject); // Destroy all children in UI Holder
                }
            }
        }
    }


    private void playOneShot(AudioClip pClip)
    {
        if (pClip != null && _audio != null) _audio.PlayOneShot(pClip);
    }
}
