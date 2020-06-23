using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class BossShootManager : MonoBehaviour
{
    public enum BossState { firstPipe, secondPipe, lastPipes, completed };

    public BossState bossState = BossState.firstPipe;
    public UnityEvent OnStartNewWave;
    public UnityEvent OnBossFightNextState;

    [SerializeField] private LevelManager _levelManager = null;
    
    [SerializeField] private FactoryPipe _pipe1;
    [SerializeField] private FactoryPipe _pipe2;
    [SerializeField] private FactoryPipe _pipe3;
    [SerializeField] private FactoryPipe _pipe4;

    [Tooltip("When final wave is played spawner starts over")]
    [SerializeField] private List<Wave> waves = new List<Wave>();
    int currentWaveIndex = 0;


    [Title("Spawn settings")]
    [SerializeField] private float _secondsBetweenWaves = 1;


    [Title("spawn info")]
    [SerializeField, ReadOnly] private float _spawnTimer = 0;

    [SerializeField, ReadOnly] private float _currentSpawningObject;

    [SerializeField, ReadOnly] private Wave currentWave;

    private void Start()
    {
        currentWave = waves[0];
        _pipe1.pipeEnabled = true;
    }

    private void Update()
    {
        updateSpawner();
        updateState();
    }


    private void NextWave()
    {
        currentWaveIndex++;
        _currentSpawningObject = 0;
        if (currentWaveIndex > waves.Count - 1) currentWaveIndex = 0;
        currentWave = waves[currentWaveIndex];
        OnStartNewWave?.Invoke();
    }


    private void updateSpawner()
    {
        _spawnTimer += Time.deltaTime;
        if(_spawnTimer > currentWave.timeBetweenSpawn)
        {
            _spawnTimer = 0;
            shootActivePipe();
            _currentSpawningObject++;

            if (_currentSpawningObject > currentWave.objectCount) NextWave();
        }
    }


    private void updateState()
    {
        switch(bossState)
        {
            case BossState.firstPipe:
                if (_pipe1.health <= 0) setBossState(BossState.secondPipe);
                break;
            case BossState.secondPipe:
                if (_pipe3.health <= 0) setBossState(BossState.lastPipes);
                break;
            case BossState.lastPipes:
                if (_pipe2.health <= 0 && _pipe4.health <= 0) setBossState(BossState.completed);
                break;
        }
    }


    private void setBossState(BossState pState)
    {
        if (pState == BossState.secondPipe)
        {
            _pipe3.pipeEnabled = true;
        }
        else if (pState == BossState.lastPipes)
        {
            _pipe2.pipeEnabled = true;
            _pipe4.pipeEnabled = true;
        }
        else if (pState == BossState.completed)
        {
            ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
            if (scoreManager != null) scoreManager.UpdateScore();

            _levelManager.LoadLevel();
            Debug.Log("BOSS FIGHT COMPLETED");
        }

        bossState = pState;
        OnBossFightNextState?.Invoke();
    }


    private void shootActivePipe()
    {
        switch(bossState)
        {
            case BossState.firstPipe:
                _pipe1.ShootTrash(currentWave.Objects[Random.Range(0, currentWave.Objects.Count - 1)]);
                break;

            case BossState.secondPipe:
                _pipe3.ShootTrash(currentWave.Objects[Random.Range(0, currentWave.Objects.Count - 1)]);
                break;

            case BossState.lastPipes:
                if (_pipe2.health <= 0)
                {
                    _pipe4.ShootTrash(currentWave.Objects[Random.Range(0, currentWave.Objects.Count - 1)]);
                }
                else if (_pipe4.health <= 0)
                {
                    _pipe2.ShootTrash(currentWave.Objects[Random.Range(0, currentWave.Objects.Count - 1)]);
                }


                if (Random.Range(0, 2) == 0) _pipe2.ShootTrash(currentWave.Objects[Random.Range(0, currentWave.Objects.Count - 1)]);
                else _pipe4.ShootTrash(currentWave.Objects[Random.Range(0, currentWave.Objects.Count - 1)]);
                break;
        }
    }
}
