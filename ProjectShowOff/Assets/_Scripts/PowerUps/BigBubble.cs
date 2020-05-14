using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBubble : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("fish"))
        {
            if (other.gameObject.GetComponent<Fish>().isProtected == false)
            {
                other.gameObject.GetComponent<Fish>().isProtected = true;
            }
        }
        else if (other.gameObject.CompareTag("thrash"))
        {
            other.gameObject.GetComponent<Thrash>().GetCollected();
        }
    }
}
