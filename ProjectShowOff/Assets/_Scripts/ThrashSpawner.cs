﻿using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class ThrashSpawner : MonoBehaviour
{
    public enum SpawnerState { idle, waitingForNextWave, spawningObjects }
    [SerializeField] private bool _log = false;
    [SerializeField] private List<Wave> _spawnWaves = new List<Wave>();
    private Wave currentWave;

    [Header("Spawn Settings")]
    [SerializeField] private float _timeBetweenWaves = 2;
    public bool _autoStartSpawner = false;

    [Header("Spawn Position")]
    [SerializeField] private Vector2 minMaxX = new Vector2(0, 0);
    [SerializeField] private float spawnHeight = 0;

    [FoldoutGroup("Spawner Information"), SerializeField, ReadOnly]
    public SpawnerState state = SpawnerState.idle;

    [FoldoutGroup("Spawner Information"), SerializeField, ReadOnly]
    private int currentWaveNumber = 0;

    [FoldoutGroup("Spawner Information"), SerializeField, ReadOnly]
    private float _timer = 0;

    [FoldoutGroup("Spawner Information"), SerializeField, ReadOnly]
    private float _timeToCurrentSpawn = 0;

    [FoldoutGroup("Spawner Information"), SerializeField, ReadOnly, ProgressBar(0, "objectsThisWave")]
    private int currentObjectCount = 0;

    [BoxGroup("Dynamic Range")]
    private int objectsThisWave;

    private GameManager _gameManager;

    private void Awake()
    {
        _gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        if (_autoStartSpawner) StartSpawning();
    }


    private void Update()
    {
        updateState();
    }


    /// <summary>
    /// Start spawn system
    /// </summary>
    public void StartSpawning()
    {
        if (_log) Debug.Log("<color=green><b> Spawn system enabled! </b></color>");

        if (!prepareNextWave()) StopSpawning();
        else state = SpawnerState.spawningObjects;
    }


    /// <summary>
    /// Stop spawn system
    /// </summary>
    public void StopSpawning()
    {
        if (_log) Debug.Log("<color=red><b> Stopped spawn system... </b></color>");
        state = SpawnerState.idle;
    }


    /// <summary>
    /// Update Finite state
    /// </summary>
    private void updateState()
    {
        _timer += Time.deltaTime;
        switch (state)
        {
            case SpawnerState.idle:
                // do nothing
                break;
            case SpawnerState.spawningObjects:
                if (_timer > _timeToCurrentSpawn)
                {
                    if (currentObjectCount < currentWave.objectCount)
                    {
                        SpawnRandomThrash(currentWave.Objects);
                        prepareNextSpawn();
                    }
                    else
                    {
                        if (!prepareNextWave()) StopSpawning();
                    }
                }
                break;
            case SpawnerState.waitingForNextWave:
                if (_timer > _timeBetweenWaves)
                {
                    startNextWave();
                }
                break;
        }
    }


    /// <summary>
    /// Spawns an random object
    /// </summary>
    private void SpawnRandomThrash(List<GameObject> objects)
    {
        GameObject randomObject = objects[Random.Range(0, objects.Count)];

        Vector3 randomPosition = new Vector3(Random.Range(minMaxX.x, minMaxX.y), spawnHeight, 0);
        if (_log) Debug.Log("<color=orange><b> Spawn: " + randomObject.name + "</b></color>");
        Instantiate(randomObject, randomPosition, randomObject.transform.rotation, transform);
    }


    /// <summary>
    /// Setup settings for the next spawn
    /// </summary>
    private void prepareNextSpawn()
    {
        _timeToCurrentSpawn = currentWave.timeBetweenSpawn - Random.Range(currentWave.minMaxTimeRandomeness.x, currentWave.minMaxTimeRandomeness.y);
        _timer = 0;
        currentObjectCount++;

        if (_log) Debug.Log("<color=orange><b> Next object in: " + _timeToCurrentSpawn + " (" + currentObjectCount + " / " + currentWave.objectCount + ") </b></color>");
    }

    /// <summary>
    /// Prepare Next Wave 
    /// </summary>
    /// <returns> false if no more waves are available </returns>
    private bool prepareNextWave()
    {
        currentWaveNumber += 1;
        if (currentWaveNumber > _spawnWaves.Count)
        {
            if (_log) Debug.Log("<color=red><b> No more waves available... </b></color>");
            return false;
        }

        currentObjectCount = 0;
        _timer = 0;
        currentObjectCount = 0;
        state = SpawnerState.waitingForNextWave;
        currentWave = _spawnWaves[currentWaveNumber - 1];
        objectsThisWave = currentWave.objectCount;

        if (_log) Debug.Log("<color=orange><b> Spawning wave completed, wait for next wave </b></color>");

        return true;
    }


    /// <summary>
    /// Start Next Wave 
    /// </summary>
    /// <returns> false if no more waves are available </returns>
    private void startNextWave()
    {
        currentWave = _spawnWaves[currentWaveNumber - 1];
        if(_log) Debug.Log("<color=orange><b>==========| -=< NEW WAVE( " + currentWaveNumber.ToString() + " ) >=- |==========</b></color>");
        prepareNextSpawn();
        state = SpawnerState.spawningObjects;
    }
}
