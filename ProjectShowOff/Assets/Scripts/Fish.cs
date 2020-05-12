using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    private FishManager _fishManager;

    private void Awake()
    {
        _fishManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<FishManager>();
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("thrash"))
        {
            _fishManager.CheckFishCount();
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
