using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace clickProto
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Animator))]
    public class Fish : MonoBehaviour
    {
        private Animator _fishAnim;

        private void Awake()
        {
            float startTime = Random.Range(0, 10f);
            _fishAnim = GetComponent<Animator>();

            _fishAnim.Play("fishSwim", 0, startTime);
        }


        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("trash")) Destroy(transform.parent.gameObject);
        }
    }
}
