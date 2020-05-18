using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Boid Settings", menuName = "Boids", order = 1)]
public class BoidSettings : ScriptableObject {
    [Title("Boid Settings:")]
    public float minSpeed = 2;
    public float maxSpeed = 5;
    public float visionRadius = 2.5f;
    public float avoidanceRadius = 1;
    public float maxTurnForce = 3;

    [Title("Rule strength:")]
    public float alignStrength = 1;
    public float cohesionStrength = 1;
    public float seperationStrength = 1;
    public float targetStrength = 1;

    [Title ("Collisions:")]
    public LayerMask obstacleLayer;
    public float boundsRadius = .27f;
    public float avoidCollisionStrength = 10;
    public float collisionThreshold = 5;

    [Title("Debug")]
    public bool _debugColission = false;

}