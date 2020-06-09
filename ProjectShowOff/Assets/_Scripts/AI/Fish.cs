﻿using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public event EventHandler OnDeath;

    //================= Health Settings =================
    [ProgressBar(0, 100, ColorMember = "GetHealthBarColor")]
    [SerializeField] public float health = 100;
    [SerializeField] protected float decreaseHpPerSecEating = 0;


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



    //================= Misc =================
    protected FishManager fishManager;
    protected bool dead = false;



    // Start is called before the first frame update
    void Awake()
    {
        fishManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<FishManager>();
        _audio = GetComponent<AudioSource>();
    }


    protected void Die()
    {
        if (!dead)
        {         
            dead = true;
            fishManager.CheckFishCount();
            _audio.PlayOneShot(_die);
            Instantiate(_dieParticlePrefab, transform.position, Quaternion.identity, transform);
            OnDeath?.Invoke(this, EventArgs.Empty);
            StartCoroutine(delayedDestroy());
        }
    }


    protected Thrash checkFortrash()
    {
        Transform trash = nearbytrash();
        if (trash != null)
        {
            if (!CameraView.isInCameraView(trash.transform.position)) return null;

            targetThrash = trash.GetComponentInChildren<Thrash>();
            return targetThrash;
        }

        return null;
    }


    protected Transform nearbytrash()
    {
        Collider[] trashColliders = Physics.OverlapSphere(transform.position, _trashDetectionRadius, _trashLayer);

        if (trashColliders.Length == 0) return null;

        Transform closestThrash = null;
        float closestDistance = float.MaxValue;
        foreach (Collider trash in trashColliders)
        {
            float distance = (trash.transform.position - transform.position).magnitude;
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestThrash = trash.transform;
            }
        }

        return closestThrash;
    }


    protected void checkHealth()
    {
        if (health <= 0)
        {
            Die();
        }
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
