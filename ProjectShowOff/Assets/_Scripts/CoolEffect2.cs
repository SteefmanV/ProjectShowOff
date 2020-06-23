using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolEffect2 : MonoBehaviour
{
    [SerializeField] private GameObject _nemo = null;
    [SerializeField] private Transform _playerHolder = null;
    private string[] nemoCode = new string[] { "n", "e", "m", "o" };
    private int index = 0;



    void Update()
    {
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(nemoCode[index])) index++;
            else index = 0;
        }

        if (index == nemoCode.Length) nemoAction();
    }


    private void nemoAction()
    {
        index = 0;
        Instantiate(_nemo, _playerHolder);
    }
}
