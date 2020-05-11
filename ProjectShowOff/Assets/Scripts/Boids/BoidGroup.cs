using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Dynamic;
using UnityEngine;

public class BoidGroup : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;

    public List<Boid> boids { get; private set; } = new List<Boid>();
    public Vector3 targetPosition { get; private set; } = new Vector3();
    public int tankSize { get; private set; } = 6;
    public bool randomTarget = true;


    [Title("Fish Settings")]
    [ShowInInspector] public float rotationSpeed { get; private set; } = 4.0f;
    [ShowInInspector] public float neighbourDistanceThrashhold { get; private set; } = 3;
    [ShowInInspector] public Vector2 minMaxFishSpeed { get; private set; } = new Vector3(0.5f, 1);


    private void Awake()
    {
        foreach (Transform boid in transform)
        {
            boids.Add(boid.gameObject.GetComponent<Boid>());
        }
    }


    public void Update()
    {
        setRandomTargetPosition();
        targetPosition = targetTransform.position;
    }


    private void setRandomTargetPosition()
    {
        if (randomTarget)
        {
            if (Random.Range(0, 10000) < 50)
            {
                targetPosition = new Vector3(Random.Range(-tankSize, tankSize), Random.Range(-tankSize, tankSize), Random.Range(-tankSize, tankSize));
                targetTransform.position = targetPosition;
            }
        }
    }
}
