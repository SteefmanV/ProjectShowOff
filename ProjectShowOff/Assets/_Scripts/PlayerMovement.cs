using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(LineRenderer))]
public class PlayerMovement : MonoBehaviour
{
    public event EventHandler<Vector3> startJump;
    public event EventHandler<Vector3> endJump;

    //Added for air trap raycast
    public Vector2 shootDirection { get; set; }

    //Made public for Bubblepack
    [Title("Shoot settings")]
    public float shootStrength = 1;
    public float maximumShootSpeed = 10;
    [SerializeField] private LayerMask _playerLayer = 0;

    [Title("Drag / Trail settings")]
    [SerializeField] private float _minimumDragLength = 1;
    [SerializeField] private float _dragTrailOffsetStrength = 0.1f;
    [SerializeField] private float _dragThresholdSpeed = 0.1f; // If the player moves slower than this speed, allow to shoot

    [TitleGroup("Movement Information")]
    [SerializeField, ReadOnly] private float _movingSpeed = 0;

    [SerializeField, ReadOnly] private bool _isDragging = false;
    private Rigidbody _rb = null;
    private LineRenderer _lineRender;
    private Camera _camera;


    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _lineRender = GetComponent<LineRenderer>();
        _camera = Camera.main;
    }


    void Update()
    {
        dragAndShoot();
    }


    /// <summary>
    /// Handle Player collision
    /// </summary>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("sticky"))
        {
            _rb.velocity = Vector3.zero;
            endJump?.Invoke(this, transform.position);
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

        Vector3 delta = mouse - startPos;

        if (delta.magnitude > _minimumDragLength)
        {
            _lineRender.positionCount = 2;
            _lineRender.SetPosition(0, startPos + delta.normalized * _dragTrailOffsetStrength);
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

            if (delta.magnitude > _minimumDragLength)
            {
                Vector3 endPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
                shootDirection = transform.position - endPosition;

                startJump?.Invoke(this, transform.position);

                Vector3 shootForce = shootStrength * shootDirection;

                if (shootForce.magnitude > maximumShootSpeed)
                {
                    shootForce = shootForce.normalized * maximumShootSpeed; // Limit shoot speed
                }

                Debug.Log("shoot speed = " + shootForce.magnitude);

                _rb.AddForce(shootForce, ForceMode.Impulse);               
            }
        }

        _lineRender.positionCount = 0;
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
