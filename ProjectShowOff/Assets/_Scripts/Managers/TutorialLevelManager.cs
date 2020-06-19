using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialLevelManager : MonoBehaviour
{
    public enum state { start, movement, powerupCollect, powerUpActivate, done }
    public state _tutorialState = state.start;

    [Title("Level State 1 (Start)")]
    // [SerializeField] private GameObject _uiPopup = null;
    [SerializeField] private Rigidbody _playerBody = null;
    [SerializeField] private ThrashSpawner _thrashSpawner = null;
    [SerializeField] private Vector3 _playerStartForce = new Vector3();
    //  [SerializeField] private List<Fish> _fishes = new List<Fish>();
    [SerializeField] private Fish _tutorialFirstFish = null;

    [Title("Level State 2(Movement)")]
    [SerializeField] private PlayerMovement _playerMovement = null;
    [SerializeField] private GameObject _movementExplenation = null;
    [SerializeField] private Thrash _firstThrash = null;

    [Title("Level State 3 (Power up collection)")]
    [SerializeField] private GameObject _bottle1 = null;
    [SerializeField] private GameObject _ring2 = null;
    [SerializeField] private GameObject _animalExplenation = null;
    [SerializeField] private float _freezeAfterSeconds = 5;
    [SerializeField] private GameObject _powerupExplenation = null;
    private bool _playerLanded = false;

    [Title("Level State 4 (Power up activation)")]
    [SerializeField] private Button _powerupActivationButton = null;
    [SerializeField] private GameObject _bottle2 = null;
    [SerializeField] private GameObject _tutorialEndPopup = null;


    [Title("Tutorial Texts")]
    [SerializeField] private TextMeshProUGUI _movementExplenationText;
    [SerializeField] private TextMeshProUGUI _saveTheAnimalsText;
    [SerializeField] private TextMeshProUGUI _powerupExplenationText;
    [SerializeField] private TextMeshProUGUI _tutorialEndText;
    private LanguageManager _languageManager = null;
    private bool _startedSpawning = false;

    private Vector3 playerStartPos = Vector3.zero;


    void Start()
    {
        StartTutorial();
        _playerMovement.endJump += SetStartPos;

        _languageManager = FindObjectOfType<LanguageManager>();
        if (_languageManager != null) updateTexts(_languageManager.GetTranslation());
    }


    void Update()
    {
        switch (_tutorialState)
        {
            case state.start:
                SetHealthOfAllFish(int.MaxValue);
                StartCoroutine(movementExplenation(2.5f));

                if ((playerStartPos != Vector3.zero) && Vector3.Distance(_playerMovement.transform.position, playerStartPos) > .5f)
                {
                    _tutorialState = state.movement;
                }

                //   if (_firstThrash == null) _tutorialState = state.movement;
                break;
            case state.movement:
                if (_firstThrash == null)
                {
                    Debug.Log("go go go");
                    _movementExplenation.SetActive(false);
                    // _uiPopup.SetActive(true);
                    _bottle1.SetActive(true);
                    _ring2.SetActive(true);

                    StartCoroutine(showAnimalExplenation(5));
                    StartCoroutine(delayedFreeze(_freezeAfterSeconds));
                    _playerMovement.startJump += startJump;


                    _tutorialState = state.powerupCollect;
                }
                break;
            case state.powerupCollect:
                if (_bottle1 == null && _ring2 == null)
                {
                    _playerMovement.endJump += OnPlayerLanded;

                    if (_playerLanded)
                    {
                        Time.timeScale = 0;
                        _powerupExplenation.SetActive(true);
                        _playerMovement.endJump += OnPowerup;
                        _powerupActivationButton.onClick.AddListener(powerUpButtonPressed);
                        _tutorialState = state.powerUpActivate;
                    }
                }
                break;
            case state.powerUpActivate:
                if (_bottle2 == null)
                {
                    StartCoroutine(showFinalPopup(5));
                    StartCoroutine(startSpawning(2));
                    ResetFishHealth();
                }
                break;
            case state.done:
                break;
        }
    }


    private void updateTexts(Translation pTranslation)
    {
        _movementExplenationText.text = pTranslation.movementExplenation;
        _saveTheAnimalsText.text = pTranslation.saveTheAnimals;
        _powerupExplenationText.text = pTranslation.powerupExplenation;
        _tutorialEndText.text = pTranslation.tutorialEnd;
    }


    private IEnumerator movementExplenation(float pDelay)
    {
        yield return new WaitForSeconds(pDelay);

        if (_firstThrash != null)
        {
            _tutorialState = state.movement;
            StartCoroutine(delayedFreeze(0.2f));
            _playerMovement.startJump += startJump;
            _movementExplenation.SetActive(true);
        }
    }

    private IEnumerator delayedFreeze(float pDuration)
    {
        float timer = 0;
        while ((_bottle1 == null && _ring2 == null) == false)
        {
            timer += Time.deltaTime;
            if (timer > pDuration) break;
            yield return null;
        }

        if (timer > pDuration)
        {
            if (_playerBody.velocity.magnitude < 0.1f)
            {
                Time.timeScale = 0;
            }
        }
    }


    private void startJump(object pSender, Vector3 pPosition)
    {
        if (_bottle1 == null && _ring2 == null) _playerMovement.startJump -= startJump;
        Time.timeScale = 1;
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


    private IEnumerator showAnimalExplenation(float pDuration)
    {
        _animalExplenation.SetActive(true);
        yield return new WaitForSeconds(pDuration);
        _animalExplenation.SetActive(false);
    }


    private void OnPlayerLanded(object pSender, Vector3 pPosition)
    {
        _playerLanded = true;
    }


    private void powerUpButtonPressed()
    {
        Time.timeScale = 1;
        _bottle2.SetActive(true);
        _powerupActivationButton.onClick.RemoveListener(powerUpButtonPressed);
    }


    private void OnPowerup(object pSender, Vector3 pPosition)
    {
        _powerupExplenation.SetActive(false);
    }


    private IEnumerator showFinalPopup(float pDuration)
    {
        _tutorialEndPopup.SetActive(true);
        yield return new WaitForSeconds(pDuration);
        _tutorialEndPopup.SetActive(false);
    }

    private IEnumerator startSpawning(float pDuration)
    {
        if (_startedSpawning) yield break;
        _startedSpawning = true;
        yield return new WaitForSeconds(pDuration);
        Debug.Log("Star spawning!");
        _thrashSpawner.StartSpawning();
        _tutorialState = state.done;
    }


    private void SetHealthOfAllFish(int pValue)
    {
        foreach(Fish fish in FindObjectsOfType<Fish>())
        {
            fish.health = pValue;
        }
    }


    private void ResetFishHealth()
    {
        foreach (Fish fish in FindObjectsOfType<Fish>())
        {
            fish.ResetHealth();
        }
    }
}
