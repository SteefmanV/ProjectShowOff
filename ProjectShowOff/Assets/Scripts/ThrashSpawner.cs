﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrashSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _thrashPrefabs = new GameObject[0];
    [SerializeField] private Vector2 _spawnRange = new Vector2(0,0);

    private float _timer = 0;

    void Update()
    {
        _timer += Time.deltaTime;

        if(_timer > 1)
        {
            spawnRandomThrash();
            _timer = 0;
        }
    }

    private void spawnRandomThrash()
    {
        GameObject thrash = Instantiate(
            _thrashPrefabs[Random.Range(0, _thrashPrefabs.Length)],
            new Vector3(Random.Range(_spawnRange.x, _spawnRange.y), 0, 0),
            Quaternion.identity,
            this.transform
            );
    }
}
