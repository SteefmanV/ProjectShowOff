using Sirenix.OdinInspector;
using UnityEngine;

public class FishMovement : MonoBehaviour
{
    public enum MoveState { idle, patrolling, chasingThrash, eating }




    [Title("Collision")]
    [TabGroup("Collision")] [SerializeField] private LayerMask _obstacleLayer = 0;
    [TabGroup("Collision")] [SerializeField] private float _collisionThreshold = 2;
    [TabGroup("Collision")] [SerializeField] private float _boundsRadius = 2;

    [Title("Movement Settings")]
    [TabGroup("Movement Settings")] [SerializeField] private float _maxSpeed = 2;
    [TabGroup("Movement Settings")] [SerializeField] private float _avoidTurnStrength = 1;
    [TabGroup("Movement Settings")] [SerializeField] private float _targetStrength = 1;
    [TabGroup("Movement Settings")] [SerializeField] private float _stopDistance = 0.2f;

    [Title("Behaviour Randomness")]
    [TabGroup("Behaviour Randomness")] [SerializeField] private Vector2 minMaxTimeIdle = new Vector2();
    [InfoBox("0-100%")]
    [TabGroup("Behaviour Randomness")] [SerializeField] private int perctangeToGoIdleAfterPatrol = 20;


    [Title("Debug")]
    [SerializeField] private bool _debugColission = false;

    [Title("Agent State")]
    public MoveState currentState = MoveState.patrolling;


    // Patrolling state fields
    [SerializeField, BoxGroup("Patrolling"), ShowIf("currentState", MoveState.patrolling)]
    private Vector3 targetPosition;
    [SerializeField, BoxGroup("Patrolling"), ShowIf("currentState", MoveState.patrolling)]
    [ProgressBar(0, "totalTravelDistanceToTarget")]
    private float targetDistance = 0;
    private float totalTravelDistanceToTarget = 0;


    // Idle state fields
    [SerializeField, BoxGroup("Idle State"), ShowIf("currentState", MoveState.idle)]
    [ProgressBar(0, "_timeIdle")]
    private float _timer = 0;
    private float _timeIdle = 0;


    private EnvironmentTargetGenerator _targetGenerator;
    private Vector3 _velocity;
    private Vector3 NULL_VEC = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);


    private void Awake()
    {
        _targetGenerator = FindObjectOfType<EnvironmentTargetGenerator>();
        targetPosition = NULL_VEC;
    }


    void Update()
    {
        switch (currentState)
        {
            case MoveState.idle:            // - Idle -                                                            
                idle();      
                break;
            case MoveState.patrolling:      // - Patrolling -                                                  
                patrolling();
                break;
            case MoveState.chasingThrash:   // - Chasing Thrash -
                chaseThrash();
                break;
            case MoveState.eating:          // - Eating Thrash -
                eat();
                break;
        }
    }


    private void idle()
    {
        _timer += Time.deltaTime;
        if (_timer > _timeIdle)
        {
            currentState = MoveState.patrolling;
            _timer = 0;
        }


        // animate     
    }


    private void patrolling()
    {
        // Target reached,  TODO: implement deacceleration instead of instant stop
        targetDistance = (transform.position - targetPosition).magnitude;
        if (targetDistance < _stopDistance)
        {
            targetPosition = NULL_VEC;

            if (Random.Range(0, 100) > perctangeToGoIdleAfterPatrol)
            {
                _timeIdle = Random.Range(minMaxTimeIdle.x, minMaxTimeIdle.y);
                currentState = MoveState.idle; // Go Idle
            }
        }

        if (targetPosition == NULL_VEC)
        {
            targetPosition = _targetGenerator.GetValidPosition(_obstacleLayer);
            totalTravelDistanceToTarget = (transform.position - targetPosition).magnitude;
        }

        Move();
    }

    private void chaseThrash()
    {
        // Follow thrash
        // if(in range) > eat
    }


    private void eat()
    {
        // if thrash within range > eat thrash
        // else if(thrash gone) > patrolling
        // else chase thrash
    }


    private void Move()
    {
        Vector3 acceleration = Vector3.zero;

        if (targetPosition != null)
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
        return (Physics.SphereCast(transform.position, _boundsRadius, transform.forward, out RaycastHit hit, _collisionThreshold, _obstacleLayer));
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

            if (!Physics.SphereCast(ray, _boundsRadius, out RaycastHit hit, _collisionThreshold, _obstacleLayer)) // If ray hit obstacle
            {
                float distance = (direction.normalized - _velocity.normalized).magnitude;
                if (distance < smallestDistance)
                {
                    smallestDistance = distance;
                    smallestVector = direction;
                }

                if (_debugColission) Debug.DrawRay(transform.position, direction * _collisionThreshold, Color.green, 1);
            }
            else
            {
                if (_debugColission) Debug.DrawRay(transform.position, direction * _collisionThreshold, Color.red, 1);
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
