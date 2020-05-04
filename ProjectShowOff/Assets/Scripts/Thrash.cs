using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thrash : MonoBehaviour
{
    [SerializeField] private float _startFallSpeed = 1;


    void Update()
    {
        Vector3 position = transform.position;
        position.y -= (_startFallSpeed * Time.deltaTime);
        transform.position = position;
    }


    private void OnMouseDown()
    {
        Destroy(gameObject);
    }
}
