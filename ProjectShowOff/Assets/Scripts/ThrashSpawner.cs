using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrashSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] thrashObjects = new GameObject[0];

    [Header("Spawn Settings")]
    [SerializeField] private float _timeBetweenSpawns = 2;
    [SerializeField] private float _timeDecreasementPerSpawn = 0.1f;

    [Header("Spawn Position")]
    [SerializeField] private Vector2 minMaxX = new Vector2(0,0);
    [SerializeField] private float spawnHeight = 0;

    private float _timer;

    private void Update()
    {
        _timer += Time.deltaTime;

        if(_timer > _timeBetweenSpawns)
        {
            SpawnRandomThrash();
            _timer = 0;
            _timeBetweenSpawns -= _timeDecreasementPerSpawn;
            if (_timeBetweenSpawns < .2f) _timeBetweenSpawns = .2f;
        }
    }

    private void SpawnRandomThrash()
    {
        GameObject randomObject = thrashObjects[Random.Range(0, thrashObjects.Length)];
        Vector3 randomPosition = new Vector3(Random.Range(minMaxX.x, minMaxX.y), spawnHeight, 0);
        Instantiate(randomObject, randomPosition, Quaternion.identity, transform);
    }
}
