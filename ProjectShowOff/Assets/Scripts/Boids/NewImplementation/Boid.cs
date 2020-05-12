using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{

    BoidSettings settings;

    [Title("Boid")]
    [ReadOnly] public Vector3 position;
    [ReadOnly] public Vector3 forward;
    [ReadOnly] private Vector3 _velocity;

    [Title("Flock Information")]
    [ShowInInspector, ReadOnly] public Vector3 avgFlockHeading { get; set; }
    [ShowInInspector, ReadOnly] public Vector3 avgAvoidanceHeading { get; set; }
    [ShowInInspector, ReadOnly] public Vector3 centreOfFlockmates { get; set; }
    [ShowInInspector, ReadOnly] public int numPerceivedFlockmates { get; set; }

    private Transform _cachedTransform;
    private Transform _target;


    void Awake()
    {
        _cachedTransform = transform;
    }


    /// <summary>
    /// Set settings and target
    /// </summary>
    public void Initialize(BoidSettings settings, Transform target)
    {
        this._target = target;
        this.settings = settings;

        position = _cachedTransform.position;
        forward = _cachedTransform.forward;

        float startSpeed = (settings.minSpeed + settings.maxSpeed) / 2;
        _velocity = transform.forward * startSpeed;
    }


    /// <summary>
    /// Perform boid algorithm 
    /// </summary>
    public void UpdateBoid()
    {
        Vector3 acceleration = Vector3.zero;

        if (_target != null)
        {
            Vector3 offsetToTarget = (_target.position - position);
            acceleration = SteerTowards(offsetToTarget) * settings.targetWeight;
        }

        if (numPerceivedFlockmates != 0)
        {
            centreOfFlockmates /= numPerceivedFlockmates;

            Vector3 offsetToFlockmatesCentre = (centreOfFlockmates - position);

            acceleration += SteerTowards(avgFlockHeading) * settings.alignWeight; // allignment
            acceleration += SteerTowards(offsetToFlockmatesCentre) * settings.cohesionWeight; // Cohesion
            acceleration += SteerTowards(avgAvoidanceHeading) * settings.seperateWeight; // seperation
        }

        if (IsHeadingForCollision())
        {
            Vector3 collisionAvoidDir = ObstacleRays();
            Vector3 collisionAvoidForce = SteerTowards(collisionAvoidDir) * settings.avoidCollisionWeight;
            acceleration += collisionAvoidForce;
        }

        _velocity += acceleration * Time.deltaTime;
        float speed = _velocity.magnitude;
        Vector3 dir = _velocity / speed;
        speed = Mathf.Clamp(speed, settings.minSpeed, settings.maxSpeed);
        _velocity = dir * speed;

        _cachedTransform.position += _velocity * Time.deltaTime;
        _cachedTransform.forward = dir;
        position = _cachedTransform.position;
        forward = dir;
    }


    private bool IsHeadingForCollision()
    {
        RaycastHit hit;
        if (Physics.SphereCast(position, settings.boundsRadius, forward, out hit, settings.collisionAvoidDst, settings.obstacleMask))
        {
            return true;
        }
        return false;
    }


    private Vector3 ObstacleRays()
    {
        Vector3[] rayDirections = BoidHelper.directions;

        for (int i = 0; i < rayDirections.Length; i++)
        {
            Vector3 dir = _cachedTransform.TransformDirection(rayDirections[i]);
            Ray ray = new Ray(position, dir);
            if (!Physics.SphereCast(ray, settings.boundsRadius, settings.collisionAvoidDst, settings.obstacleMask))
            {
                return dir;
            }
        }

        return forward;
    }


    private Vector3 SteerTowards(Vector3 vector)
    {
        Vector3 vec = vector.normalized * settings.maxSpeed - _velocity;
        Vector3 clamped = Vector3.ClampMagnitude(vec, settings.maxSteerForce);
        return clamped;
    }
}