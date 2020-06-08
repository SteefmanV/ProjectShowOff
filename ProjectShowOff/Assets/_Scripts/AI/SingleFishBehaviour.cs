using Sirenix.OdinInspector;
using System;
using UnityEngine;

public class SingleFishBehaviour : Fish
{
    public enum Behaviour { idle, patrolling, chasingThrash, eating }

    //================= References =================
    [SerializeField, Required] private FishAgent _agent = null;
    [SerializeField, Required] private Animator _anim = null;


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


    //================= Misc =================
    private EnvironmentTargetGenerator _targetGenerator;
    private float _oldCollisionThreshhold = 0;



    private void Start()
    {
        _targetGenerator = FindObjectOfType<EnvironmentTargetGenerator>();
        OnDeath += die;

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


    private void OnDestroy()
    {
        OnDeath -= die;
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
                fishManager.CheckFishCount();
                //Destroy(gameObject);
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

        if(checkFortrash() != null)
        {
            currentState = Behaviour.chasingThrash;
        }
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

            if (UnityEngine.Random.Range(0, 100) > perctangeToGoIdleAfterPatrol)
            {
                _timeIdle = UnityEngine.Random.Range(minMaxTimeIdle.x, minMaxTimeIdle.y);
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

        if (checkFortrash() != null)
        {
            currentState = Behaviour.chasingThrash;
        };
    }


    private void chaseThrash()
    {
        if (targetThrash == null)
        {
            currentState = Behaviour.patrolling;
            if (_audio.clip == _eating) _audio.Stop();
            return;
        }

        _agent.moving = true;
        _agent.collisionThreshold = 0.2f;
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

        _anim.SetTrigger("hurt");

        if (_audio.clip != _eating)
        {
            _audio.clip = _eating;
            _audio.loop = true;
            _audio.Play();
        }
    }


    private void die(object pSender, EventArgs pE)
    {
        _anim.SetTrigger("die");
    }
}
