using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Net : MonoBehaviour
{
    [SerializeField] private float _netDuration = 5;
    private float _timer = 0;

    private void Update()
    {
        _timer += Time.deltaTime;

        if(_timer > _netDuration)
        {
            Destroy(gameObject);
        }
    }


    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("thrash"))
        {
            Destroy(collision.gameObject);
        }
    }
}
