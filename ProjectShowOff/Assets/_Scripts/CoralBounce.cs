using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CoralBounce : MonoBehaviour
{
    public UnityEvent OnBounce;

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
                OnBounce?.Invoke();
                _timer = 0;
            }
        }
    }
}
