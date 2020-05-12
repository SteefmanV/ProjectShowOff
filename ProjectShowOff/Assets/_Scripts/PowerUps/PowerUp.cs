using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [Required, SceneObjectsOnly]
    [SerializeField] private NetPowerUp _netPowerUp = null;
    [SerializeField] private airTrapPowerUp _airTrapPowerUp = null;
    [SerializeField] private BubblePackPowerUp _bubblePackPowerUp = null;

    private PlayerMovement _playerMovement = null;
    private ItemCollectionManager _itemCollection = null;
    private bool activateNetNextJump = false;
    private bool activateBubblePackNextJump = false;
    private bool activateAirTrapNextJump = false;


    private void Awake()
    {
        _itemCollection = FindObjectOfType<ItemCollectionManager>();

        _playerMovement = FindObjectOfType<PlayerMovement>();
        _playerMovement.startJump += OnStartJump;
        _playerMovement.endJump += OnEndJump;
    }

    public void ActivateNet()
    {
        activateNetNextJump = true;
        Debug.Log("ACTIVATE NET");
    }


    public void ActivateBubblePack()
    {
        activateBubblePackNextJump = true;
        Debug.Log("bubble pack activated");
    }

    public void ActivateAirTrap()
    {
        activateAirTrapNextJump = true;
        Debug.Log("air traps activated");
    }


    private void OnStartJump(object pSender, Vector3 pPosition)
    {
        if (activateNetNextJump) _netPowerUp.StartNet(pPosition);
        if (activateAirTrapNextJump) _airTrapPowerUp.SetUp(pPosition, _playerMovement.shootDirection);
        if (_bubblePackPowerUp.bbPackActive)
        {
            _bubblePackPowerUp.Land();
        }
    }

    private void OnEndJump(object pSender, Vector3 pPosition)
    {
        Debug.Log("NET = " + _netPowerUp);
        if (_netPowerUp.netActive)
        {
            _netPowerUp.StopNet(pPosition);
            activateNetNextJump = false;
            _itemCollection.ResetCount();
        }
        else if (_airTrapPowerUp.airTrapActive)
        {
            _airTrapPowerUp.Land();
            activateAirTrapNextJump = false;
            _itemCollection.ResetCount();
        }
        if (activateBubblePackNextJump)
        {
            _bubblePackPowerUp.SetUp(_playerMovement);
            activateBubblePackNextJump = false;
            _itemCollection.ResetCount();
        }
    }
}
