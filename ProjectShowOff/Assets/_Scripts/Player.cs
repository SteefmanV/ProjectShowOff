using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject _jetPack = null;
    
    public void SetJetpack(bool pActive)
    {
        _jetPack.SetActive(pActive);
    }
}
