using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Thrash : MonoBehaviour
{
    [SerializeField] private float _startFallSpeed = 1;
    [SerializeField] private Rigidbody _rb = null;
    [SerializeField] private float _minForcePercentage = 0.2f;

    [SerializeField] private Vector3 minimumForce;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();

    }


    private void Start()
    {
        _rb.AddForce(new Vector3(0, _startFallSpeed, 0), ForceMode.Force);
    }


    private void Update()
    {
        minimumForce = new Vector3(0, _startFallSpeed * _minForcePercentage, 0);

        if (_rb.velocity.magnitude < minimumForce.magnitude)
        {
            _rb.velocity = minimumForce;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
