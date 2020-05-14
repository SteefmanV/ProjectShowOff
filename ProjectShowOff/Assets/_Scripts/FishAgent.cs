using Sirenix.OdinInspector;
using UnityEngine;

public class FishAgent : MonoBehaviour
{
    [ShowInInspector] public Vector3 targetPosition { get; set; }
    public bool hasTarget { get; private set; } = false;
    public bool moving { get; set; } = false;


    [Title("Collision")]
    [TabGroup("Collision")] [SerializeField] public LayerMask obstacleLayer = 0;
    [TabGroup("Collision")] [SerializeField] public float collisionThreshold = 2;
    [TabGroup("Collision")] [SerializeField] private float _boundsRadius = 2;

    [Title("Movement Settings")]
    [TabGroup("Movement Settings")] [SerializeField] private float _maxSpeed = 2;
    [TabGroup("Movement Settings")] [SerializeField] private float _avoidTurnStrength = 1;
    [TabGroup("Movement Settings")] [SerializeField] private float _targetStrength = 1;
    [TabGroup("Movement Settings")] public float stopDistance = 0.2f;

    [Title("Debug")]
    [SerializeField] private bool _debugColission = false;
    private Vector3 _velocity;

    private void Update()
    {
        if(moving) Move();
    }


    public void ClearTarget()
    {
        targetPosition = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
        hasTarget = false;
    }


    public void SetTarget(Vector3 pTarget)
    {
        targetPosition = pTarget;
        hasTarget = true;
    }


    private void Move()
    {
        Vector3 acceleration = Vector3.zero;

        if (hasTarget)
        {
            acceleration = turnTowards(targetPosition - transform.position) * _targetStrength; // Turn towards target
        }

        if (isCloseToCollision())
        {
            Vector3 collisionAvoidDir = getObstacleDirection(); // More expensive high precission check
            Vector3 collisionAvoidForce = turnTowards(collisionAvoidDir) * _avoidTurnStrength;
            acceleration += collisionAvoidForce;
        }

        _velocity += acceleration * Time.deltaTime;
        _velocity = _velocity.normalized * Mathf.Clamp(_velocity.magnitude, 0.1f, _maxSpeed);
        transform.position += _velocity * Time.deltaTime;

        Vector3 forwardVec = _velocity.normalized;
        transform.forward = forwardVec;
    }


    private bool isCloseToCollision()
    {
        return (Physics.SphereCast(transform.position, _boundsRadius, transform.forward, out RaycastHit hit, collisionThreshold, obstacleLayer));
    }


    /// <summary>
    /// Return obstacle direction
    /// </summary>
    private Vector3 getObstacleDirection()
    {
        Vector3[] rayDirections = FibonacciSphere.pointsAroundSphere;

        Vector3 smallestVector = transform.forward;
        float smallestDistance = float.MaxValue;

        for (int i = 0; i < rayDirections.Length; i++)
        {
            Vector3 direction = transform.TransformDirection(rayDirections[i]);
            Ray ray = new Ray(transform.position, direction);

            if (!Physics.SphereCast(ray, _boundsRadius, out RaycastHit hit, collisionThreshold, obstacleLayer)) // If ray hit obstacle
            {
                float distance = (direction.normalized - _velocity.normalized).magnitude;
                if (distance < smallestDistance)
                {
                    smallestDistance = distance;
                    smallestVector = direction;
                }

                if (_debugColission) Debug.DrawRay(transform.position, direction * collisionThreshold, Color.green, 1);
            }
            else
            {
                if (_debugColission) Debug.DrawRay(transform.position, direction * collisionThreshold, Color.red, 1);
            }
        }

        return smallestVector;
    }


    private Vector3 turnTowards(Vector3 vector)
    {
        Vector3 turnedVector = vector.normalized * _maxSpeed - _velocity;
        return Vector3.ClampMagnitude(turnedVector, 10);
    }
}
