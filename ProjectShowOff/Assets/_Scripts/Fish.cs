using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public enum Behaviour { idle, patrolling, chasingThrash, eating }
    //================= References =================
    [SerializeField, Required] private FishAgent _agent = null;

    //================= Health Settings =================
    [ProgressBar(0, 100, ColorMember = "GetHealthBarColor")]
    [SerializeField] private float health = 100;
    [SerializeField] private float decreaseHpPerSecEating = 0;

    //================= Thrash Searching =================
    [Title("Thrash Searching Settings")]
    [SerializeField] private float _trashDetectionRadius = 0;
    [SerializeField] private LayerMask _trashLayer = 1;
    [SerializeField] private float _trashEatRadius = 0;

    //================= Behaviour Randomness =================
    [Title("Behaviour Randomness")]
    [SerializeField] private Vector2 minMaxTimeIdle = new Vector2();
    [SerializeField] private int perctangeToGoIdleAfterPatrol = 20;

    //================= FISH FINITE STATE =================
    [Title("Agent State")]
    public Behaviour currentState = Behaviour.patrolling;

    //*** Patrolling state fields ***
    [ReadOnly, SerializeField, BoxGroup("Patrolling"), ShowIf("currentState", Behaviour.patrolling)] private Vector3 _targetPosition;
    [ReadOnly, SerializeField, BoxGroup("Patrolling"), ShowIf("currentState", Behaviour.patrolling)]
    [ProgressBar(0, "totalTravelDistanceToTarget")] private float targetDistance = 0;

    private float totalTravelDistanceToTarget = 0;


    //*** Idle state fields ***
    [SerializeField, BoxGroup("Idle State"), ShowIf("currentState", Behaviour.idle)]
    [ProgressBar(0, "_timeIdle")]
    private float _timer = 0;
    private float _timeIdle = 0;

    //*** Chase state field ***
    [SerializeField, BoxGroup("Chase trash"), ShowIf("currentState", Behaviour.chasingThrash)]
    private Thrash targetThrash = null;


    [Title("Protection Bubble")]
    private bool _isProtected = false;
    public bool isProtected {
        get { 
            return _isProtected;
        }
        set
        {
            _isProtected = value;
            _protectionBubble.SetActive(value);
        }
    }
    [SerializeField] private GameObject _protectionBubble;


    private EnvironmentTargetGenerator _targetGenerator;
    private FishManager _fishManager;

    private float _oldCollisionThreshhold = 0;


    private void Awake()
    {
        _fishManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<FishManager>();
        _targetGenerator = FindObjectOfType<EnvironmentTargetGenerator>();

        _agent.ClearTarget();
        _oldCollisionThreshhold = _agent.collisionThreshold;
    }


    void Update()
    {
        switch (currentState)
        {
            case Behaviour.idle:            // - Idle -                                                            
                idle();
                break;
            case Behaviour.patrolling:      // - Patrolling -                                                  
                patrolling();
                break;
            case Behaviour.chasingThrash:   // - Chasing Thrash -
                chaseThrash();
                break;
            case Behaviour.eating:          // - Eating Thrash -
                eat();
                break;
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("thrash"))
        {
            if (isProtected)
            {
                isProtected = false;
                Destroy(collision.gameObject);
            }
            else
            {
                _fishManager.CheckFishCount();
                Destroy(gameObject);
            }
        }
    }



    private void idle()
    {
        _agent.moving = false;
        _timer += Time.deltaTime;
        if (_timer > _timeIdle)
        {
            currentState = Behaviour.patrolling;
            _timer = 0;
        }
        // animate   

        checkFortrash();
    }


    private void patrolling()
    {
        _agent.moving = true;
        _agent.collisionThreshold = _oldCollisionThreshhold;
        // Target reached,  TODO: implement deacceleration instead of instant stop
        targetDistance = (_agent.targetPosition - _agent.transform.position).magnitude;
        if (targetDistance < _agent.stopDistance)
        {
            _agent.ClearTarget();

            if (Random.Range(0, 100) > perctangeToGoIdleAfterPatrol)
            {
                _timeIdle = Random.Range(minMaxTimeIdle.x, minMaxTimeIdle.y);
                currentState = Behaviour.idle; // Go Idle
                return;
            }
        }

        if (!_agent.hasTarget)
        {
            _agent.SetTarget(_targetGenerator.GetValidPosition(_agent.obstacleLayer));
            totalTravelDistanceToTarget = (_agent.targetPosition - _agent.transform.position).magnitude;
        }

        _targetPosition = _agent.targetPosition;

        checkFortrash();
    }


    private void chaseThrash()
    {
        if (targetThrash == null)
        {
            currentState = Behaviour.patrolling;
            return;
        }

        _agent.moving = true;
        _agent.collisionThreshold = 0.3f;
        _agent.SetTarget(targetThrash.transform.position);

        if((transform.position - targetThrash.transform.position).magnitude < _trashEatRadius * 0.8f)
        {
            currentState = Behaviour.eating;
        }
    }


    private void eat()
    {
        if (targetThrash == null) currentState = Behaviour.patrolling;

        _agent.moving = false;
        health -= (Time.deltaTime * decreaseHpPerSecEating);
        targetThrash.health -= (Time.deltaTime * decreaseHpPerSecEating);
        currentState = Behaviour.patrolling;
        checkHealth();
    }


    private void Die()
    {
        _fishManager.CheckFishCount();
        Destroy(gameObject);
    }


    private void checkFortrash()
    {
        Transform trash = nearbytrash();
        if(trash != null)
        {
            targetThrash = trash.GetComponent<Thrash>();
            currentState = Behaviour.chasingThrash;
        }
    }


    private Transform nearbytrash()
    {
        Collider[] trashColliders = Physics.OverlapSphere(transform.position, _trashDetectionRadius, _trashLayer);

        if (trashColliders.Length == 0) return null;

        Transform closestThrash = null;
        float closestDistance = float.MaxValue;
        foreach(Collider trash in trashColliders)
        {
            float distance = (trash.transform.position - transform.position).magnitude;
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestThrash = trash.transform;
            }           
        }

        return closestThrash;
    }


    private void checkHealth()
    {
        if (health <= 0) Die();
    }


    private Color GetHealthBarColor(float value)
    {
        return Color.Lerp(Color.red, Color.green, Mathf.Pow(value / 100f, 2));
    }
}
