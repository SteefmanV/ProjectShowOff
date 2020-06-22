using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoralBounce : MonoBehaviour
{
    [SerializeField] private float _timeBetweenBounces = 0;
    private Animator _anim;

    private float _timer = 0;

    private void Start()
    {
        _anim = GetComponent<Animator>();
    }


    private void Update()
    {
        _timer += Time.deltaTime;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (_timer > _timeBetweenBounces)
            {
                _anim.SetTrigger("bounce");
                _timer = 0;
            }
        }
    }
}
