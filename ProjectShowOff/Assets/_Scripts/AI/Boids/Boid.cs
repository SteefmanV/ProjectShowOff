using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : Fish
{
    private BoidSettings _settings;
    [SerializeField] private Transform _target;
    private Vector3 _velocity;
    [SerializeField] private Animator _anim;


    /// <summary>
    /// Set settings and target
    /// </summary>
    public void Initialize(BoidSettings pBoidSettings, Transform pTarget)
    {
        _settings = pBoidSettings;
        _target = pTarget;
        _velocity = transform.forward * ((_settings.minSpeed + _settings.maxSpeed) / 2);
    }


    /// <summary>
    /// Perform boid algorithm 
    /// </summary>
    public void UpdateBoid(BoidData pBoidData)
    {
        Vector3 acceleration = Vector3.zero;

        Thrash targetThrash = checkFortrash();
        if (targetThrash != null)
        {
            _target = targetThrash.transform;
            acceleration = turnTowards(_target.position - transform.position) * _settings.targetStrength; // Turn towards target

            if ((transform.position - _target.position).magnitude < _trashEatRadius * 0.8f)
            {
                eat();
            }
            else _anim.SetBool("hurt", false);
        }
        else _anim.SetBool("hurt", false);

        if (pBoidData.flockSize != 0)
        {
            pBoidData.flockCentre /= pBoidData.flockSize;

            acceleration += turnTowards(pBoidData.flockDirection) * _settings.alignStrength; // allignment
            acceleration += turnTowards(pBoidData.flockCentre - transform.position) * _settings.cohesionStrength; // Cohesion
            acceleration += turnTowards(pBoidData.avoidDirection) * _settings.seperationStrength; // seperation
        }

        if (isCloseToCollision()) // First perform low cost check
        {
            Vector3 collisionAvoidDir = getObstacleDirection(); // More expensive high precission check
            Vector3 collisionAvoidForce = turnTowards(collisionAvoidDir) * _settings.avoidCollisionStrength;
            acceleration += collisionAvoidForce;
        }

        _velocity += acceleration * Time.deltaTime;
        _velocity = _velocity.normalized * Mathf.Clamp(_velocity.magnitude, _settings.minSpeed, _settings.maxSpeed); // move direction * clamped speed
        transform.forward = _velocity.normalized;

        transform.position += _velocity * Time.deltaTime;
    }



    private Vector3 turnTowards(Vector3 vector)
    {
        Vector3 turnedVector = vector.normalized * _settings.maxSpeed - _velocity;
        return Vector3.ClampMagnitude(turnedVector, _settings.maxTurnForce);
    }


    private bool isCloseToCollision()
    {
        return (Physics.SphereCast(transform.position, _settings.boundsRadius, transform.forward, out RaycastHit hit, _settings.collisionThreshold, _settings.obstacleLayer));
    }


    /// <summary>
    /// Return obstacle direction
    /// </summary>
    private Vector3 getObstacleDirection()
    {
        Vector3[] rayDirections = FibonacciSphere.pointsAroundSphere;

        for (int i = 0; i < rayDirections.Length; i++)
        {
            Vector3 direction = transform.TransformDirection(rayDirections[i]);
            if (_settings._debugColission) Debug.DrawRay(transform.position, direction, Color.red, _settings.collisionThreshold);
            Ray ray = new Ray(transform.position, direction);
            if (!Physics.SphereCast(ray, _settings.boundsRadius, _settings.collisionThreshold, _settings.obstacleLayer)) // If ray hit obstacle
            {
                if (_settings._debugColission) Debug.DrawRay(transform.position, direction, Color.green, _settings.collisionThreshold);
                return direction;
            }
        }

        return transform.forward;
    }
    private void eat()
    {
        if (targetThrash == null) return;
        health -= (Time.deltaTime * decreaseHpPerSecEating);
        targetThrash.health -= (Time.deltaTime * decreaseHpPerSecEating);
        if (health <= 0 && !dead)
        {
            _anim.SetTrigger("die");
        }
        else _anim.SetTrigger("hurt");

        checkHealth();

        //if(_audio.clip != _eating)
        //{
        //    _audio.clip = _eating;
        //    _audio.loop = true;
        //    _audio.Play();
        //}
    }
}