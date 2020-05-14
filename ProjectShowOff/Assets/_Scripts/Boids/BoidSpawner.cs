//using Sirenix.OdinInspector;
using UnityEngine;

public class BoidSpawner : MonoBehaviour {

    [SerializeField] private GameObject _boidPrefab = null;
    [SerializeField] private float _spawnRadius = 10;
    [SerializeField] private int _boidCount = 10;


    void Awake () {
        SpawnBoids(_boidCount);
    }


    /// <summary>
    /// Spawn boids in a random position and direction
    /// </summary>
    /// <param name="pBoidCount"> Number of boids to spawn </param>
    private void SpawnBoids(int pBoidCount)
    {
        for (int i = 0; i < pBoidCount; i++)
        {
            Vector3 randomPosition = transform.position + Random.insideUnitSphere * _spawnRadius;
            GameObject boid = Instantiate(_boidPrefab);
            boid.transform.position = randomPosition;
            boid.transform.forward = Random.insideUnitSphere;
        }
    }
}