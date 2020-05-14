using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace clickProto
{
    public class ThrashSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject[] _trashPrefabs = new GameObject[0];
        [SerializeField] private Vector2 _spawnRange = new Vector2(0, 0);

        [SerializeField] private float _spawnTime = 2;
        [SerializeField] private float _speedupPerSpawn = 0.01f;

        private float _timer = 0;

        void Update()
        {
            _timer += Time.deltaTime;

            if (_timer > _spawnTime)
            {
                spawnRandomThrash();
                _timer = 0;

                if (_spawnTime < 0.1f) _spawnTime = 0.1f;
            }
        }

        private void spawnRandomThrash()
        {
            GameObject trash = Instantiate(
                _trashPrefabs[Random.Range(0, _trashPrefabs.Length)],
                new Vector3(Random.Range(_spawnRange.x, _spawnRange.y), 0, 0),
                Quaternion.identity,
                this.transform
             );

            _spawnTime -= _speedupPerSpawn;
        }
    }
}
