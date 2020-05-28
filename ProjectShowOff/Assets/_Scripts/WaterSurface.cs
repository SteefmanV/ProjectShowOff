using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSurface : MonoBehaviour
{
    [SerializeField] private Rigidbody _player = null;

    // Update is called once per frame
    void Update()
    {
        if(_player.transform.position.y > transform.position.y)
        {
            _player.useGravity = true;
        }
        else
        {
            _player.useGravity = false;
        }
    }
}
