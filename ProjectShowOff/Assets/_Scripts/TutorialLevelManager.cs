using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialLevelManager : MonoBehaviour
{
    public enum state { start, movement, powerupCollect, powerUpActivate, done }
    public state _tutorialState = state.start;

    [Title("Level State 1")]
    [SerializeField] private GameObject _uiPopup = null;
    [SerializeField] private Rigidbody _playerBody = null;
    [SerializeField] private ThrashSpawner _thrashSpawner = null;
    [SerializeField] private Vector3 _playerStartForce = new Vector3();
    [SerializeField] private List<Fish> _fishes = new List<Fish>();

    [Title("Level State 2")]
    [SerializeField] private PlayerMovement _playerMovement = null;
    [SerializeField] private GameObject _movementExplenation = null;
    [SerializeField] private Thrash _firstThrash = null;

    [Title("Level State 3")]
    [SerializeField] private GameObject _bottle1 = null;
    [SerializeField] private GameObject _ring2 = null;

    private Vector3 playerStartPos = Vector3.zero;


    void Start()
    {
        StartTutorial();
        _playerMovement.endJump += SetStartPos;
    }


    void Update()
    {
        switch(_tutorialState)
        {
            case state.start:
                foreach(Fish fish in _fishes)
                {
                    if (fish.health < 100)
                    {
                        _tutorialState = state.movement;
                        StartCoroutine(delayedFreeze(0.5f));
                        _playerMovement.startJump += startJump;
                        _movementExplenation.SetActive(true);
                    }

                    if ((playerStartPos != Vector3.zero) && Vector3.Distance(_playerMovement.transform.position, playerStartPos) > .5f)
                    {
                        _tutorialState = state.movement;
                    }
                }

                if (_firstThrash == null) _tutorialState = state.movement;
                break;
            case state.movement:
                if(_firstThrash.health <= 0 || _firstThrash == null)
                {
                    _movementExplenation.SetActive(false);
                    _uiPopup.SetActive(true);
                    _bottle1.SetActive(true);
                    _ring2.SetActive(true);
                    _tutorialState = state.powerupCollect;
                }
                break;
            case state.powerupCollect:
                if(_bottle1 == null && _ring2 == null)
                {
                   // _tutorialState = state.powerUpActivate;
                    _thrashSpawner.StartSpawning();
                    _tutorialState = state.done;
                }
                break;
            case state.powerUpActivate:
                // if powerup activated:
                //_thrashSpawner.StartSpawning();
                // 
                break;
            case state.done:
                break;
        }
    }


    private IEnumerator delayedFreeze(float pTime)
    {
        yield return new WaitForSeconds(pTime);
        Time.timeScale = 0;
    }


    private void startJump(object pSender, Vector3 pPosition)
    {
        Time.timeScale = 1;
        _playerMovement.startJump -= startJump;
    }

    private void SetStartPos(object pSender, Vector3 pPosition)
    {
        playerStartPos = _playerMovement.transform.position;
        _playerMovement.endJump -= SetStartPos;
    }


    private void StartTutorial()
    {
        _playerBody.AddForce(_playerStartForce, ForceMode.Impulse);
    }
}
