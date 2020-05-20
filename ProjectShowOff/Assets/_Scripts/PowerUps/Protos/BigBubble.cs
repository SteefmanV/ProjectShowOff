using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBubble : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameObject otherObject = other.gameObject;
        if (otherObject.CompareTag("fish")) otherObject.GetComponent<SingleFishBehaviour>().isProtected = true;
        if (otherObject.CompareTag("thrash")) otherObject.GetComponent<Thrash>().GetCollected();
    }
}
