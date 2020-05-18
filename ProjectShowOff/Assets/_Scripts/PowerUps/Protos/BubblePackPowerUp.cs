using Sirenix.OdinInspector;
using UnityEngine;

public class BubblePackPowerUp : MonoBehaviour
{
    [ReadOnly] public bool bbPackActive = false;
    [SerializeField, Required] private GameObject _player;

    [Title("Shoot Settings")]
    [SerializeField] private float _shootStrengthIncreasement = 20;
    [SerializeField] private float _maximumShootSpeedIncreasement = 40;

    private PlayerMovement _playerMovement;

    //caching previous values
    private float _startShootStrength;
    private float _startMaximumShootSpeed;

    public void SetUp(PlayerMovement newPlayerMovement)
    {
        bbPackActive = true;

        //saving
        _playerMovement = newPlayerMovement;
        _startShootStrength = _playerMovement.shootStrength;
        _startMaximumShootSpeed = _playerMovement.maximumShootSpeed;

        //changing
        _playerMovement.shootStrength += _shootStrengthIncreasement;
        _playerMovement.maximumShootSpeed += _maximumShootSpeedIncreasement;

    }


    public void Stop()
    {
        if (bbPackActive)
        {
            _playerMovement.shootStrength = _startShootStrength;
            _playerMovement.maximumShootSpeed = _startMaximumShootSpeed;
            bbPackActive = false;
        }
    }
}
