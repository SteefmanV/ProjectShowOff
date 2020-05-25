using Sirenix.OdinInspector;
using UnityEngine;

public class BubblePackPowerUp : MonoBehaviour
{
    [Title("Shoot Settings")]
    [InfoBox("% on top of normal speed. So 10% is: speed x 1.1")]
    [SerializeField] private float _percentageIncreament = 100;

    [FoldoutGroup("Sounds"), SerializeField] private AudioClip _activated = null;
    private AudioSource _audio;

    private PlayerMovement _playerMovement;
    private Player _player;

    //caching previous values
    private float _defaultStrength;
    private float _defaultMaxSpeed;


    private void Awake()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        _player = playerObject.GetComponent<Player>();
        _playerMovement = playerObject.GetComponent<PlayerMovement>();

        _defaultStrength = _playerMovement.shootStrength;
        _defaultMaxSpeed = _playerMovement.maximumShootSpeed;

        _audio = GetComponent<AudioSource>();
        _audio.PlayOneShot(_activated);
    }


    public void Setup()
    {
        float strengthIncreaement = (1 + _percentageIncreament * 0.01f);
        _playerMovement.shootStrength = _defaultStrength * strengthIncreaement;
        _playerMovement.maximumShootSpeed = _defaultMaxSpeed * strengthIncreaement;

        _player.SetJetpack(true);
    }


    public void Stop()
    {
        _playerMovement.shootStrength = _defaultStrength;
        _playerMovement.maximumShootSpeed = _defaultMaxSpeed;
        _player.SetJetpack(false);
    }
}
