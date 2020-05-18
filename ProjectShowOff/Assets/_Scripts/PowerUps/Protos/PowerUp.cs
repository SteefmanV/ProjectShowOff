using Sirenix.OdinInspector;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField, Required, SceneObjectsOnly] private GameObject powerUpManager;

    public enum PowerUps { none, net, bubblePack, airTrap }
    public PowerUps currentlyActive = PowerUps.none;
    public PowerUps nextPowerUp = PowerUps.none;

    // powerup references
    private NetPowerUp _netPowerUp = null;
    private airTrapPowerUp _airTrapPowerUp = null;
    private BubblePackPowerUp _bubblePackPowerUp = null;

    private PlayerMovement _playerMovement = null;
    private ItemCollectionManager _itemCollection = null;


    private void Awake()
    {
        _itemCollection = FindObjectOfType<ItemCollectionManager>();

        _playerMovement = FindObjectOfType<PlayerMovement>();
        _playerMovement.startJump += OnStartJump;
        _playerMovement.endJump += OnEndJump;

        _netPowerUp = powerUpManager.GetComponent<NetPowerUp>();
        _airTrapPowerUp = powerUpManager.GetComponent<airTrapPowerUp>();
        _bubblePackPowerUp = powerUpManager.GetComponent<BubblePackPowerUp>();
    }

    public void ActivatePowerup(PowerUps pPowerup) 
    {
        nextPowerUp = pPowerup;
        Debug.Log("<color=purple><b> Activated: " + pPowerup + "</b></color>");
    }


    private void OnStartJump(object pSender, Vector3 pPosition)
    {
        if (nextPowerUp != PowerUps.none)
        {
            currentlyActive = nextPowerUp;
            nextPowerUp = PowerUps.none;
        }

        switch(currentlyActive)
        {
            case PowerUps.net:
                _netPowerUp.StartNet(pPosition);
                break;
            case PowerUps.bubblePack:
                _bubblePackPowerUp.Stop();
                break;
            case PowerUps.airTrap:
                _airTrapPowerUp.SetUp(pPosition, _playerMovement.shootDirection);
                break;
        }
    }

    private void OnEndJump(object pSender, Vector3 pPosition)
    {
        switch(currentlyActive)
        {
            case PowerUps.net:
                _netPowerUp.StopNet(pPosition);
                break;
            case PowerUps.bubblePack:
                _bubblePackPowerUp.Stop();
                break;
            case PowerUps.airTrap:
                _airTrapPowerUp.Stop();
                break;
        }

        if(currentlyActive != PowerUps.none)
        {
            _itemCollection.ResetCount();
            currentlyActive = PowerUps.none;
        }    
    }
}
