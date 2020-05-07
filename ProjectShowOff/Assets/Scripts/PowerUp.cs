using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private NetPowerUp _netPowerUp = null;

    private PlayerMovement _playerMovement;
    private bool activateNetNextJump = false;


    private void Awake()
    {
        _playerMovement = FindObjectOfType<PlayerMovement>();
        _playerMovement.startJump += OnStartJump;
        _playerMovement.endJump += OnEndJump;
    }


    public void ActivateNet()
    {
        activateNetNextJump = true;
        Debug.Log("ACTIVATE NET");
    }


    private void OnStartJump(object pSender, Vector3 pPosition)
    {
        if (activateNetNextJump) _netPowerUp.startNet(pPosition);
    }


    private void OnEndJump(object pSender, Vector3 pPosition)
    {
        Debug.Log("NET = " + _netPowerUp);
        if (_netPowerUp.netActive)
        {
            _netPowerUp.stopNet(pPosition);
            activateNetNextJump = false;
        }
    }



}
