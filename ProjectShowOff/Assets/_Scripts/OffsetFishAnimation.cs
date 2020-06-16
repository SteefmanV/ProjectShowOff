using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetFishAnimation : MonoBehaviour
{
    [SerializeField] private string _animationName = "";
    [SerializeField] private Vector2 _minMaxOffset = new Vector2();
    private Animator _anim;


    void Start()
    {
        _anim = GetComponent<Animator>();
        float randomIdleStart = Random.Range(_minMaxOffset.x, _minMaxOffset.y); //Set a random part of the animation to start from
        _anim.Play(_animationName, 0, randomIdleStart);
    }
}
