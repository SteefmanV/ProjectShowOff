using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(AudioSource))]
public class PlayerMovement : MonoBehaviour
{
    public event EventHandler<Vector3> startJump;
    public event EventHandler<Vector3> endJump;

    //Added for air trap raycast
    public Vector2 shootDirection { get; private set; }
    public Vector3 mouseDrag { get; private set; }
    public float shootForce { get; private set; }

    //Made public for Bubblepack
    [Title("Shoot settings")]
    public float shootStrength = 1;
    public float maximumShootSpeed = 10;
    [SerializeField] private LayerMask _playerLayer = 0;

    [Title("Drag / Trail settings")]
    [SerializeField] public float minimumDragLength = 1;
    [SerializeField] private float _dragTrailOffsetStrength = 0.1f;
    [SerializeField] private float _dragThresholdSpeed = 0.1f; // If the player moves slower than this speed, allow to shoot

    [TitleGroup("Movement Information")]
    [SerializeField, ReadOnly] private float _movingSpeed = 0;
    [SerializeField, ReadOnly] private bool _isDragging = false;

    [SerializeField, FoldoutGroup("Sounds")] private AudioClip _select = null;
    [SerializeField, FoldoutGroup("Sounds")] private AudioClip _charge = null;
    [SerializeField, FoldoutGroup("Sounds")] private AudioClip _jump = null;
    [SerializeField, FoldoutGroup("Sounds")] private AudioClip _land = null;
    private AudioSource _audio;

    private Rigidbody _rb = null;
    private LineRenderer _lineRender;
    private Camera _camera;

    private float _timer = 0;


    void Awake()
    {
        _audio = GetComponent<AudioSource>();
        _rb = GetComponent<Rigidbody>();
        _lineRender = GetComponent<LineRenderer>();
        _camera = Camera.main;
    }


    void Update()
    {
        dragAndShoot();

        _timer += Time.deltaTime;
    }


    /// <summary>
    /// Handle Player collision
    /// </summary>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("sticky"))
        {
            if (_timer < 0.1f) return;

            _rb.velocity = Vector3.zero;
            endJump?.Invoke(this, transform.position);
            if(_land != null) _audio.PlayOneShot(_land);

            Vector3 n = collision.GetContact(0).normal;
            Vector3 target = transform.position + n * 10;
            transform.LookAt(target, Vector3.up);

            if(transform.rotation.eulerAngles.y == 0)
            {
                Vector3 rotEuler = transform.rotation.eulerAngles;
                rotEuler.y = 90;
                transform.rotation = Quaternion.Euler(rotEuler);
            }
        }
    }


    /// <summary>
    /// Update drag and shoot mechanic
    /// </summary>
    private void dragAndShoot()
    {
        _movingSpeed = _rb.velocity.magnitude;

        if (Input.GetMouseButtonDown(0) && isMouseOverObject()) // Drag start
        {
            if (_rb.velocity.magnitude < _dragThresholdSpeed)
            {
                _isDragging = true;
                dragStart();
            }
        }

        if (_isDragging)
        {
            if (Input.GetMouseButton(0)) // Drag stay
            {
                dragUpdate();
            }

            if (Input.GetMouseButtonUp(0)) // Drag Stop
            {
                dragStop();
                _isDragging = false;
            }
        }
    }


    /// <summary>
    /// Setup startPosition and Linerender 
    /// </summary>
    private void dragStart()
    {
        Vector3 startPos = transform.position;
        startPos.z = -1;

        _lineRender.positionCount = 2;
        _lineRender.SetPosition(0, startPos);
        _audio.PlayOneShot(_select);

        // Start charge loop
        _audio.clip = _charge;
        _audio.loop = true;
        _audio.Play();
    }


    /// <summary>
    /// Update drag linerender
    /// </summary>
    private void dragUpdate()
    {
        Vector3 startPos = transform.position;
        Vector3 mouse = _camera.ScreenToWorldPoint(Input.mousePosition);
        startPos.z = -1;
        mouse.z = -1;

        mouseDrag = mouse - startPos;
        shootForce = shootStrength * mouseDrag.magnitude;
        if (shootForce > maximumShootSpeed) shootForce = maximumShootSpeed;

        if (mouseDrag.magnitude > minimumDragLength)
        {
            _lineRender.positionCount = 2;
            _lineRender.SetPosition(0, startPos + mouseDrag.normalized * _dragTrailOffsetStrength);
            _lineRender.SetPosition(1, mouse);
        }
        else _lineRender.positionCount = 0;
    }


    /// <summary>
    /// Drag stop, shoot player (if able) and clear linerender
    /// </summary>
    private void dragStop()
    {
        if (!isMouseOverObject())
        {
            Vector3 mouse = _camera.ScreenToWorldPoint(Input.mousePosition);
            mouse.z = -1;
            Vector3 delta = mouse - transform.position;

            if (delta.magnitude > minimumDragLength)
            {
                Vector3 endPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
                shootDirection = transform.position - endPosition;

                _audio.Stop(); // stop charge loop
                startJump?.Invoke(this, transform.position);
                _audio.PlayOneShot(_jump);

                Vector3 force = shootStrength * shootDirection;

                if (force.magnitude > maximumShootSpeed)
                {
                    force = force.normalized * maximumShootSpeed; // Limit shoot speed
                }

                _rb.AddForce(-force, ForceMode.Impulse);
                _timer = 0;
            }
        }

        _lineRender.positionCount = 0;
        mouseDrag = Vector3.zero;
    }


    /// <summary>
    /// Return true if the mouse is hovering over this gameObject
    /// </summary>
    private bool isMouseOverObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, _playerLayer))
        {
            if (hit.collider.gameObject == gameObject) return true;
        }

        return false;
    }



    #region ClickAndShoot
    //Leggacy, not used anymore
    private void clickAndShoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (_rb.velocity.magnitude < 0.01f)
            {
                _rb.AddForce(direction() * shootStrength, ForceMode.Force);
            }
        }
    }

    private void arrowPos()
    {
        float degAngle = Mathf.Rad2Deg * angleBetweenMouseAndObject();
        //Vector3 currentRotation = _arrowObject.transform.rotation.eulerAngles;
        // _arrowObject.transform.rotation = Quaternion.Euler(currentRotation.x, currentRotation.y, degAngle);
    }


    private float angleBetweenMouseAndObject()
    {
        Vector2 objPos = Camera.main.WorldToScreenPoint(transform.position);
        Vector2 mousePos = Input.mousePosition;
        return Mathf.Atan2(mousePos.y - objPos.y, mousePos.x - objPos.x);
    }


    private Vector2 direction()
    {
        Vector2 deltaPos = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        return deltaPos.normalized;
    }
    #endregion
}
