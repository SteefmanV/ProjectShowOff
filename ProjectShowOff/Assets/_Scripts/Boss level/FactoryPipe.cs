using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryPipe : MonoBehaviour
{
    [SerializeField] private bool _pipeEnabled = false;
    public bool pipeEnabled
    {
        get { return _pipeEnabled; }
        set {
            _pipeEnabled = value;
            updatePipeColor();
        }
    }

    public int health = 3;

    [SerializeField] private TrashShooter _trashShooter = null;

    [SerializeField] private Color _fullHealth;
    [SerializeField] private Color _1Hit;
    [SerializeField] private Color _2Hit;
    [SerializeField] private Color _broken;
    [SerializeField] private Color _disabled;

    private MeshRenderer _pipeMeshRenderer;
    private float _lastHittimer = 0;

    private void Awake()
    {
        _pipeMeshRenderer = GetComponent<MeshRenderer>();
        updatePipeColor();
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
                    if (health < 0) health = 0;
                    updatePipeColor();
                    _lastHittimer = 0;
                }
            }
        }
    }


    private void updatePipeColor()
    {
        if (pipeEnabled)
        {
            switch (health)
            {
                case 3:
                    SetPipeColor(_fullHealth);
                    break;
                case 2:
                    SetPipeColor(_1Hit);
                    break;
                case 1:
                    SetPipeColor(_2Hit);
                    break;
                case 0:
                    SetPipeColor(_broken);
                    break;
            }
        }
        else
        {
            SetPipeColor(_disabled);
        }
    }


    private void SetPipeColor(Color pColor)
    {
        _pipeMeshRenderer.material.color = pColor;
    }
}
