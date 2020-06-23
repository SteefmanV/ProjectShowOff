using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FactoryPipe : MonoBehaviour
{
    [SerializeField, FoldoutGroup("Events")] public UnityEvent OnPlayerHit;
    [SerializeField, FoldoutGroup("Events")] public UnityEvent OnPipeEnabled;

    [SerializeField] private bool _pipeEnabled = false;
    public bool pipeEnabled
    {
        get { return _pipeEnabled; }
        set
        {
            _pipeEnabled = value;
            if (_pipeEnabled) OnPipeEnabled?.Invoke();
            updatePipe();
        }
    }

    public int health = 3;

    [SerializeField] private TrashShooter _trashShooter = null;

    [Title("Different Pipe States")]
    [SerializeField] private GameObject _fullHealth;
    [SerializeField] private GameObject _1Hit;
    [SerializeField] private GameObject _2Hit;
    [SerializeField] private GameObject _broken;
    [SerializeField] private GameObject _disabled;

    private MeshRenderer _pipeMeshRenderer;
    private float _lastHittimer = 0;

    private void Awake()
    {
        _pipeMeshRenderer = GetComponent<MeshRenderer>();
        updatePipe();
    }


    private void Update()
    {
        _lastHittimer += Time.deltaTime;
    }


    public void ShootTrash(GameObject pTrashPrefab)
    {
        _trashShooter.Shoot(pTrashPrefab);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (pipeEnabled)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                if (_lastHittimer > 1)
                {
                    health--;
                    if (health >= 0) OnPlayerHit?.Invoke();
                    if (health < 0) health = 0;
                    updatePipe();
                    _lastHittimer = 0;
                }
            }
        }
    }


    private void updatePipe()
    {
        deactivateAllPipes();

        if (pipeEnabled)
        {
            switch (health)
            {
                case 3:
                    _fullHealth.SetActive(true);
                    break;
                case 2:
                    _1Hit.SetActive(true);
                    break;
                case 1:
                    _2Hit.SetActive(true);
                    break;
                case 0:
                    _broken.SetActive(true);
                    break;
            }
        }
        else
        {
            _disabled.SetActive(true);
        }
    }


    private void SetPipeColor(Color pColor)
    {
        _pipeMeshRenderer.material.color = pColor;
    }


    private void deactivateAllPipes()
    {
        if (_fullHealth.activeSelf) _fullHealth.SetActive(false);
        if (_1Hit.activeSelf) _1Hit.SetActive(false);
        if (_2Hit.activeSelf) _2Hit.SetActive(false);
        if (_broken.activeSelf) _broken.SetActive(false);
        if (_disabled.activeSelf) _disabled.SetActive(false);
    }
}
