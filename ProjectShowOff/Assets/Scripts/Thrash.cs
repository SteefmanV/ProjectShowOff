using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Thrash : MonoBehaviour
{
    [SerializeField] private float _startFallSpeed = 1;


    void Update()
    {
        Vector3 position = transform.position;
        position.y -= (_startFallSpeed * Time.deltaTime);
        transform.position = position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
