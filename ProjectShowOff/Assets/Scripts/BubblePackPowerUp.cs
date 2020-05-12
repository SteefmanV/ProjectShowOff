using Sirenix.OdinInspector;
using UnityEngine;

public class BubblePackPowerUp : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [ReadOnly] public bool bbPackActive = false;
    private PlayerMovement _playerMovement;

    //saving previous values
    public float startShootStrength;
    public float startMaximumShootSpeed;

    //new values
    private float shootStrengthIncreasement = 20;
    private float maximumShootSpeedIncreasement = 40;

    public void setUp(PlayerMovement newPlayerMovement)
    {
        bbPackActive = true;

        //saving
        _playerMovement = newPlayerMovement;
        startShootStrength = _playerMovement._shootStrength;
        startMaximumShootSpeed = _playerMovement._maximumShootSpeed;

        //changing
        _playerMovement._shootStrength += shootStrengthIncreasement;
        _playerMovement._maximumShootSpeed += maximumShootSpeedIncreasement;

    }

    public void landing()
    {
        if (bbPackActive)
        {
            _playerMovement._shootStrength = startShootStrength;
            _playerMovement._maximumShootSpeed = startMaximumShootSpeed;
            bbPackActive = false;
        }
    }
}
