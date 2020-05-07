﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("thrash"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
