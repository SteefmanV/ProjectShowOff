using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashPauseAnimation : MonoBehaviour
{
    [SerializeField] private float _keepPausingSeconds = 0.5f;
    private Animator _anim;
    private float _timer = 0;

    private bool _paused = true;

    private void Start()
    {
        _anim = GetComponent<Animator>();
    }


    private void Update()
    {
        _timer += Time.deltaTime;

        if (_paused && _timer > _keepPausingSeconds)
        {
            _anim.SetBool("pause", false);
            _paused = false;
        }

        if (!_paused && _timer < _keepPausingSeconds)
        {
            _anim.SetBool("pause", true);
            _paused = true;
        }
    }


    private void OnCollisionStay(Collision collision)
    {
        _timer = 0;
    }
}
