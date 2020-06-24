using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TrashGravityManager : MonoBehaviour
{
    [SerializeField] private Thrash trash = null;
    private Rigidbody _rb = null;

    private Transform _waterSurface;
    private bool gravityEnabled = false;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _waterSurface = GameObject.FindGameObjectWithTag("WaterSurface").transform;
    }

    private void Update()
    {
        if(transform.position.y > _waterSurface.position.y) // Above water
        {
            if(!gravityEnabled)
            {
                enableGravity(true);
            }
        }
        else // In water
        {
            if(gravityEnabled)
            {
                enableGravity(false);
            }
        }
    }


    private void enableGravity(bool pActive)
    {
        _rb.useGravity = pActive;
        trash.enabled = !pActive;
        gravityEnabled = pActive;

        if(!pActive)
        {
            _rb.velocity = Vector3.zero;
        }
    }
}
