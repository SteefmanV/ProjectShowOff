using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public event EventHandler OnDeath;

    [SerializeField] public int health = 3;


    //================= Thrash Searching =================
    [Title("Thrash Searching Settings")]
    [SerializeField] protected float _trashDetectionRadius = 0;
    [SerializeField] protected LayerMask _trashLayer = 1;
    [SerializeField] protected float _trashEatRadius = 0;
    [SerializeField] protected Thrash targetThrash = null;

    [FoldoutGroup("Sounds"), SerializeField] private AudioClip _die;
    [FoldoutGroup("Sounds"), SerializeField] protected AudioClip _thrashDetected;
    [FoldoutGroup("Sounds"), SerializeField] protected AudioClip _eating;
    protected AudioSource _audio;

    [Title("ParticleEffects")]
    [SerializeField] private GameObject _dieParticlePrefab = null;

    //================= Power Ups =================
    [Title("Protection Bubble")]
    private bool _isProtected = false;
    public bool isProtected
    {
        get
        {
            return _isProtected;
        }
        set
        {
            _isProtected = value;
            _protectionBubble.SetActive(value);
        }
    }
    [SerializeField] private GameObject _protectionBubble;
    private int _startHealth = 0;


    //================= Misc =================
    protected FishManager fishManager;
    protected bool dead = false;



    // Start is called before the first frame update
    void Awake()
    {
        _startHealth = health;
        fishManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<FishManager>();
        _audio = GetComponent<AudioSource>();
    }


    public void ResetHealth()
    {
        health = _startHealth;
    }


    protected void Die()
    {
        if (!dead)
        {
            dead = true;
            if (fishManager != null) fishManager.CheckFishCount();
            _audio.PlayOneShot(_die);
            Instantiate(_dieParticlePrefab, transform.position, Quaternion.identity, transform);
            OnDeath?.Invoke(this, EventArgs.Empty);
            StartCoroutine(delayedDestroy());
        }
    }


    protected Thrash checkFortrash()
    {
        if (targetThrash != null) return targetThrash;
        else
        {
            Transform trash = nearbytrash();
            if (trash != null)
            {
                if (!CameraView.isInCameraView(trash.transform.position)) return null;

                targetThrash = trash.GetComponentInChildren<Thrash>();
                targetThrash.fishTargetedThisTrash = this;
                return targetThrash;
            }

            return null;
        }
    }


    protected Transform nearbytrash()
    {
        Collider[] trashColliders = Physics.OverlapSphere(transform.position, _trashDetectionRadius, _trashLayer);

        if (trashColliders.Length == 0) return null;

        Transform closestThrash = null;
        float closestDistance = float.MaxValue;
        foreach (Collider trash in trashColliders)
        {
            Thrash trashObject = trash.GetComponentInChildren<Thrash>();
            if (trashObject == null || trashObject.fishTargetedThisTrash != null) continue; //If not trash or trash is already targeted by another fish

            float distance = (trash.transform.position - transform.position).magnitude;
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestThrash = trash.transform;
            }
        }

        return closestThrash;
    }


    /// <summary>
    /// Return true if dead
    /// </summary>
    protected bool checkHealth()
    {
        if (health <= 0)
        {
            Die();
            return true;
        }

        else return false;
    }


    private IEnumerator delayedDestroy()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }


    private Color GetHealthBarColor(float value)
    {
        return Color.Lerp(Color.red, Color.green, Mathf.Pow(value / 100f, 2));
    }
}
