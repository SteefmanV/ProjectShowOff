using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetFishAnimation : MonoBehaviour
{
    private Animator _anim;
    void Start()
    {
        _anim = GetComponent<Animator>();
        float randomIdleStart = Random.Range(0.00f, 2.00f); //Set a random part of the animation to start from
        _anim.Play("fishAnimation", 0, randomIdleStart);
    }
}
