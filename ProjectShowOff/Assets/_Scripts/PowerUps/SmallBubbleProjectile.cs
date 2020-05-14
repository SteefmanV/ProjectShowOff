using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class SmallBubbleProjectile : MonoBehaviour
{
    [ReadOnly] public GameObject target = null;

    [SerializeField] private float speed = 1f;


    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("fish"))
        {
            if (other.gameObject.GetComponent<Fish>().isProtected == false)
            {
                other.gameObject.GetComponent<Fish>().isProtected = true;
            }

            Destroy(gameObject);
        }
    }
}
