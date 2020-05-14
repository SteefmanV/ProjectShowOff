using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField, Required, SceneObjectsOnly] private GameObject powerUpManager;

    // powerup references
    private NetPowerUp _netPowerUp = null;
    private airTrapPowerUp _airTrapPowerUp = null;
    private BubblePackPowerUp _bubblePackPowerUp = null;
    private SmallBubbleGunPowerUp _smallBubbleGunPowerUp = null;
    private BigBubblePowerUp _bigBubblePowerUp = null;
    private BubbleBarragePowerUp _bubbleBarragePowerUp = null;

    private PlayerMovement _playerMovement = null;
    private ItemCollectionManager _itemCollection = null;

    // powerup mess that needs to be changed
    private bool activateNetNextJump = false;
    private bool activateBubblePackNextJump = false;
    private bool activateAirTrapNextJump = false;
    private bool activateSmallBubbleGun = false;
    private bool activateBigBubble = false;
    private bool activateFrontNet = false;
    private bool activateBubbleBarrage = false;


    private void Awake()
    {
        _itemCollection = FindObjectOfType<ItemCollectionManager>();

        _playerMovement = FindObjectOfType<PlayerMovement>();
        _playerMovement.startJump += OnStartJump;
        _playerMovement.endJump += OnEndJump;

        _netPowerUp = powerUpManager.GetComponent<NetPowerUp>();
        _airTrapPowerUp = powerUpManager.GetComponent<airTrapPowerUp>();
        _bubblePackPowerUp = powerUpManager.GetComponent<BubblePackPowerUp>();
        _smallBubbleGunPowerUp = powerUpManager.GetComponent<SmallBubbleGunPowerUp>();
        _bigBubblePowerUp = powerUpManager.GetComponent<BigBubblePowerUp>();
        _bubbleBarragePowerUp = powerUpManager.GetComponent<BubbleBarragePowerUp>();
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

    public void ActivateSmallBubbleGun()
    {
        activateSmallBubbleGun = true;
        Debug.Log("small bubble gun activated");
    }

    public void ActivateBigBubble()
    {
        activateBigBubble = true;
        Debug.Log("Big bubble activated");
    }

    public void ActivateBubbleBarrage()
    {
        activateBubbleBarrage = true;
        Debug.Log("BUBBLE BARRAGE!");
    }


    private void OnStartJump(object pSender, Vector3 pPosition)
    {
        if (activateNetNextJump) _netPowerUp.StartNet(pPosition);
        if (activateAirTrapNextJump) _airTrapPowerUp.SetUp(pPosition, _playerMovement.shootDirection);
        if (_bubblePackPowerUp.bbPackActive) _bubblePackPowerUp.Land();
        if (activateSmallBubbleGun) _smallBubbleGunPowerUp.SetUp();
        if (activateBigBubble) _bigBubblePowerUp.setUp();
        if (activateBubbleBarrage) _bubbleBarragePowerUp.setUp();
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
        else if (_smallBubbleGunPowerUp.smallBubbleGunPowerUpActive)
        {
            _smallBubbleGunPowerUp.Land();
            activateSmallBubbleGun = false;
            _itemCollection.ResetCount();
        }
        else if (_bigBubblePowerUp.bigBubbleActive)
        {
            _bigBubblePowerUp.Land();
            activateBigBubble = false;
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
