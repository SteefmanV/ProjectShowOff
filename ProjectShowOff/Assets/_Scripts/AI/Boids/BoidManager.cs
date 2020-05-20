using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class BoidManager : MonoBehaviour
{
    [SerializeField, Required] private BoidSettings _settings = null;
    [SerializeField] private Transform _target = null;
    private List<Boid> _boids = new List<Boid>();


    //void Start()
    //{
    //    _boids = getActiveBoids();
    //}


    void Update()
    {
        _boids = getActiveBoids();
        updateBoids();
    }


    /// <summary>
    /// Update flocking behaviour
    /// </summary>
    private void updateBoids()
    {
        if (_boids != null)
        {
            BoidData[] boidData = getBoidData();

            // Whatch out with this, it's exponential. If necessary implement octrees later
            for (int boidIndex = 0; boidIndex < boidData.Length; boidIndex++)
            {
                BoidData boid = boidData[boidIndex];
                for (int otherBoids = 0; otherBoids < boidData.Length; otherBoids++)
                {
                    if (boidIndex != otherBoids)
                    {
                        BoidData otherBoid = boidData[otherBoids];
                        Vector3 deltaPosition = otherBoid.position - boid.position;

                        if (deltaPosition.magnitude < _settings.visionRadius)
                        {
                            boid.flockSize += 1;
                            boid.flockDirection += otherBoid.direction;
                            boid.flockCentre += otherBoid.position;

                            if (deltaPosition.magnitude < _settings.avoidanceRadius)
                            {
                                boid.avoidDirection -= deltaPosition / (deltaPosition.magnitude  * deltaPosition.magnitude);
                            }
                        }
                    }
                }

                _boids[boidIndex].UpdateBoid(boid);
            }
        }
    }


    /// <summary>
    /// Get all activate boids in scene
    /// </summary>
    private List<Boid> getActiveBoids()
    {
        _boids = FindObjectsOfType<Boid>().ToList();

        foreach (Boid boid in _boids)
        {
            boid.Initialize(_settings, _target);
        }

        return _boids;
    }


    /// <summary>
    /// Transfer Boid[] to BoidData[]
    /// </summary>
    private BoidData[] getBoidData()
    {
        BoidData[] boidData = new BoidData[_boids.Count];

        for (int i = 0; i < _boids.Count; i++)
        {
            if (_boids[i] == null)
            {
                _boids.Remove(_boids[i]);
                continue;
            }
            boidData[i].Initialize(_boids[i].transform.position, _boids[i].transform.forward);
        }

        return boidData;
    }
}