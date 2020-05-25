using Sirenix.OdinInspector;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField, Required, SceneObjectsOnly] private GameObject powerUpManager;

    public enum PowerUps { none, net, bubblePack, airTrap }
    [ReadOnly, SerializeField] private PowerUps currentlyActive = PowerUps.none;
    [ReadOnly, SerializeField] private PowerUps nextPowerUp = PowerUps.none;

    [FoldoutGroup("Sounds"), SerializeField] private AudioClip _powerUpCharge = null; 
    [FoldoutGroup("Sounds"), SerializeField] private AudioClip _powerUpOver = null;
    private AudioSource _audio;

    // powerup references
    private NetPowerUp _netPowerUp = null;
    private airTrapPowerUp _airTrapPowerUp = null;
    private BubblePackPowerUp _bubblePackPowerUp = null;

    private PlayerMovement _playerMovement = null;
    private ItemCollectionManager _itemCollection = null;


    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
        _itemCollection = FindObjectOfType<ItemCollectionManager>();

        _playerMovement = FindObjectOfType<PlayerMovement>();
        _playerMovement.startJump += OnStartJump;
        _playerMovement.endJump += OnEndJump;

        _netPowerUp = powerUpManager.GetComponent<NetPowerUp>();
        _airTrapPowerUp = powerUpManager.GetComponent<airTrapPowerUp>();
        _bubblePackPowerUp = powerUpManager.GetComponent<BubblePackPowerUp>();
    }


    /// <summary>
    /// Activate a powerup
    /// </summary>
    public void ActivatePowerup(PowerUps pPowerup)
    {
        if (currentlyActive == PowerUps.none && nextPowerUp == PowerUps.none)
        {
            nextPowerUp = pPowerup;
            Debug.Log("<color=green><b> Activated: " + pPowerup + "</b></color>");
        }
    }


    private void OnStartJump(object pSender, Vector3 pPosition)
    {
        if (nextPowerUp != PowerUps.none)
        {
            currentlyActive = nextPowerUp;
            nextPowerUp = PowerUps.none;

            _audio.clip = _powerUpCharge;
            _audio.loop = true;
            _audio.Play();
        }

        switch (currentlyActive)
        {
            case PowerUps.net:
                _netPowerUp.StartNet(pPosition);
                break;
            case PowerUps.bubblePack:
                _bubblePackPowerUp.Setup();
                break;
            case PowerUps.airTrap:
                _airTrapPowerUp.Setup(pPosition, _playerMovement.shootDirection);
                break;
        }
    }


    private void OnEndJump(object pSender, Vector3 pPosition)
    {
        _audio.Stop();

        switch (currentlyActive)
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

        if (currentlyActive != PowerUps.none)
        {
            _itemCollection.ResetCount();
            currentlyActive = PowerUps.none;
            _audio.PlayOneShot(_powerUpOver);
        }
    }
}
