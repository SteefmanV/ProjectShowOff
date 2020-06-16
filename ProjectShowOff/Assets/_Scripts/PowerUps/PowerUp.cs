using Sirenix.OdinInspector;
using System;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField, Required, SceneObjectsOnly] private GameObject powerUpManager;

    public enum PowerUps { none, net, bubblePack, airTrap }
    public bool powerUpActivated { get; set; } = false;

    [ReadOnly, SerializeField] private PowerUps currentlyActive = PowerUps.none;
    [ReadOnly, SerializeField] private PowerUps nextPowerUp = PowerUps.none;

    [FoldoutGroup("Sounds"), SerializeField] private AudioSource _audio;
    [FoldoutGroup("Sounds"), SerializeField] private AudioClip _powerUpCharge = null;
    [FoldoutGroup("Sounds"), SerializeField] private AudioClip _powerUpOver = null;
    [SerializeField] private ParticleSystem _powerUpEffect = null;


    // powerup references
    private NetPowerUp _netPowerUp = null;
    private airTrapPowerUp _airTrapPowerUp = null;
    private BubblePackPowerUp _bubblePackPowerUp = null;

    private PlayerMovement _playerMovement = null;
    private ItemCollectionManager _itemCollection = null;


    private void Awake()
    {
        //_audio = GetComponent<AudioSource>();
        _itemCollection = FindObjectOfType<ItemCollectionManager>();

        _playerMovement = FindObjectOfType<PlayerMovement>();
        _playerMovement.startJump += OnStartJump;
        _playerMovement.endJump += OnEndJump;
        _playerMovement.startDrag += OnStartDrag;
        _playerMovement.endDrag += OnEndDrag;

        _netPowerUp = powerUpManager.GetComponent<NetPowerUp>();
        _airTrapPowerUp = powerUpManager.GetComponent<airTrapPowerUp>();
        _bubblePackPowerUp = powerUpManager.GetComponent<BubblePackPowerUp>();
    }


    private void Update()
    {
        if (powerUpActivated && (_playerMovement.isDragging || _playerMovement.isMoving))
        {
            if (!_powerUpEffect.isPlaying) _powerUpEffect.Play();
        }
        else
        {
            if (_powerUpEffect.isPlaying) _powerUpEffect.Stop();
        }
    }


    /// <summary>
    /// Activate a powerup
    /// </summary>
    public void PreparePowerup(PowerUps pPowerup)
    {
        if (currentlyActive == PowerUps.none && nextPowerUp == PowerUps.none)
        {
            nextPowerUp = pPowerup;
            Debug.Log("<color=green><b> next: " + pPowerup + "</b></color>");
        }
    }


    public void ActivatePowerup()
    {
        if (nextPowerUp != PowerUps.none)
        {
            Debug.Log("Activate");
            powerUpActivated = true;
            _itemCollection.ActivatePowerupEffect();
        }
    }


    private void OnStartJump(object pSender, Vector3 pPosition)
    {
        if (powerUpActivated)
        {
            if (nextPowerUp != PowerUps.none)
            {
                currentlyActive = nextPowerUp;
                nextPowerUp = PowerUps.none;

                powerUpActivated = false;
            }

            switch (currentlyActive)
            {
                case PowerUps.net:
                    _netPowerUp.StartNet(_playerMovement.transform.position);
                    break;
                case PowerUps.bubblePack:
                    _bubblePackPowerUp.Setup();
                    break;
                case PowerUps.airTrap:
                    _airTrapPowerUp.Setup(_playerMovement.transform.position, -_playerMovement.shootDirection);
                    break;
            }
        }
    }


    private void OnEndJump(object pSender, Vector3 pPosition)
    {
       // _audio.Stop();

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
        }
    }


    private void OnStartDrag(object pSender, EventArgs pE)
    {
        if (powerUpActivated)
        {
            Debug.Log("Start drag powerup");
            _audio.clip = _powerUpCharge;
            _audio.loop = true;
            _audio.Play();
        }
    }


    private void OnEndDrag(object pSender, EventArgs pE)
    {
        if (nextPowerUp != PowerUps.none)
        {
            Debug.Log("<color=red>End drag powerup</color>");
            _audio.Stop();
            _audio.PlayOneShot(_powerUpOver);
        }
    }


    private void OnDestroy()
    {
        _playerMovement.startJump -= OnStartJump;
        _playerMovement.endJump -= OnEndJump;
    }
}
