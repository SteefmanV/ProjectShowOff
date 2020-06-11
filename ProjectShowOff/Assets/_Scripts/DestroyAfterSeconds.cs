using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour
{
    [SerializeField] private float _duration = 1;

    private void Start()
    {
        StartCoroutine(delayedDestroy(_duration));
    }


    private IEnumerator delayedDestroy(float pDuration)
    {
        yield return new WaitForSeconds(pDuration);
        Destroy(gameObject);
    } 
}
