using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStar : MonoBehaviour
{
    private bool _on;

    public bool on
    {
        get { return _on; }
        set
        {
            _on = value;
            _starOn.SetActive(_on);
            _starOff.SetActive(!_on);
        }
    }

    [SerializeField] private GameObject _starOn = null;
    [SerializeField] private GameObject _starOff = null;
}
