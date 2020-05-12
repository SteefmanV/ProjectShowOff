using Sirenix.OdinInspector;
using UnityEngine;

public class JellyFish : MonoBehaviour
{
    private Vector3 _velocity;
    [SerializeField] private Transform moveTarget;

    [Title("Settings")]
    [SerializeField] private float _collisionThreshold = 2;
    [SerializeField] private float _boundsRadius = 2;
    [SerializeField] private LayerMask _obstacleLayer;
    [SerializeField] private float _avoidTurnStrength = 1;
    [SerializeField] private float _targetStrength = 1;
    [SerializeField] private float _maxSpeed = 2;
    [SerializeField] private float _stopDistance = 0.2f;
    [SerializeField] private float _offSteerStrength = .1f;

    private Rigidbody _rb;


    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector3 acceleration = Vector3.zero;

        if ((transform.position - moveTarget.position).magnitude < _stopDistance) return;
        if (moveTarget != null)
        {
            acceleration = turnTowards(moveTarget.position - transform.position) * _targetStrength; // Turn towards target
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
      //  _rb.velocity = _velocity;

        Vector3 forwardVec = _velocity.normalized;
       // if (forwardVec.y < 0) forwardVec.y = 0;
        transform.forward = forwardVec;

      //  Vector3 oldRotation = transform.rotation.eulerAngles;
      //  transform.rotation = Quaternion.Euler(0, 0, oldRotation.z);
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
                float distance = (direction.normalized - _velocity.normalized ).magnitude;
                if(distance < smallestDistance)
                {
                    smallestDistance = distance;
                    smallestVector = direction;
                }

               // Debug.DrawRay(transform.position, direction * _collisionThreshold, Color.green, 1);
            }
        }

        return smallestVector;
        //return transform.forward;
    }

    private Vector3 turnTowards(Vector3 vector)
    {
        Vector3 turnedVector = vector.normalized * _maxSpeed - _velocity;
        return Vector3.ClampMagnitude(turnedVector, 10);
    }
}
