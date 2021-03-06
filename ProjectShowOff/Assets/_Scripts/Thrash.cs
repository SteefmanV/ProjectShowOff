﻿using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Thrash : MonoBehaviour
{
    public bool disabled { get; set; } = false;

    // public float startHealth = 100;
    public Fish fishTargetedThisTrash = null;

    //  [ProgressBar(0, "startHealth", ColorMember = "GetHealthBarColor")]
    // public float health = 100;

    [Header("Thrash Settings")]
    [SerializeField] private ItemCollectionManager.Item trashType = ItemCollectionManager.Item.bottle;
    [SerializeField] private float _startFallSpeed = 1;

    [SerializeField, Tooltip("This is minimum movespeed,  % of the _startFallSpeed ")]
    private float _minForcePercentage = 0.2f;

    [SerializeField] private GameObject _mainObject = null;

    [SerializeField] private Rigidbody _rb = null;
    private ItemCollectionManager _powerUpManager;
    private ScoreManager _scoreManager;


    private void Awake()
    {
        _rb = _mainObject.GetComponent<Rigidbody>();
        GameObject gameManager = GameObject.FindGameObjectWithTag("GameManager");
        _powerUpManager = gameManager.GetComponent<ItemCollectionManager>();
        _scoreManager = gameManager.GetComponent<ScoreManager>();
        //   health = startHealth;
    }


    private void Start()
    {
        _rb.AddForce(new Vector3(0, _startFallSpeed, 0), ForceMode.Force);
    }


    private void Update()
    {
        if (!disabled)
        {
            Vector3 minimumForce = new Vector3(0, _startFallSpeed * _minForcePercentage, 0);

            if (_rb.velocity.magnitude < minimumForce.magnitude)
            {
                _rb.velocity = minimumForce;
            }
        }

        Vector3 pos = transform.parent.position;
        pos.z = 0;
        transform.parent.position = pos;
    }


    public void GetCollected()
    {
        _powerUpManager.CollectedItem(trashType);
        _scoreManager.ThrashDestroyed();
        Delete();
    }


    public void SetDisabled(bool pActive)
    {
        disabled = pActive;

        if (disabled)
        {
            GetComponentInParent<BoxCollider>().enabled = false;
            _rb.isKinematic = true;
            _rb.velocity = Vector3.zero;
        }
        else
        {
            GetComponentInParent<BoxCollider>().enabled = true;
            _rb.isKinematic = false;
            _rb.AddForce(new Vector3(0, _startFallSpeed, 0), ForceMode.Force);
        }
    }


    public void Delete()
    {
        Destroy(_mainObject);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (disabled) return;

        if (other.gameObject.tag == "Player")
        {
            if (enabled)
            {
                GetCollected();
            }
        }
    }


    private Color GetHealthBarColor(float value)
    {
        return Color.Lerp(Color.red, Color.green, Mathf.Pow(value / 100f, 2));
    }
}
