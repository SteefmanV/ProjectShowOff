using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(LineRenderer))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Shoot settings")]
    [SerializeField] private float _shootStrength = 1;
    [SerializeField] private float _maximumShootSpeed = 10;

    [Header("Drag / Trail settings")]
    [SerializeField] private float _minimumDragLength = 1;
    [SerializeField] private float _dragTrailOffsetStrength = 0.1f;

    private Rigidbody _rb = null;
    private LineRenderer _lineRender;
    private Camera _camera;
    private bool _isDragging = false;


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
        }
    }


    /// <summary>
    /// Update drag and shoot mechanic
    /// </summary>
    private void dragAndShoot()
    {
        if (Input.GetMouseButtonDown(0) && isMouseOverObject()) // Drag start
        {
            if (_rb.velocity.magnitude < 0.01f)
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
            Vector3 endPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 shootForce = _shootStrength * new Vector2(transform.position.x - endPosition.x, transform.position.y - endPosition.y);

            Vector3 mouse = _camera.ScreenToWorldPoint(Input.mousePosition);
            mouse.z = -1;
            Vector3 delta = mouse - transform.position;

            if (delta.magnitude > _minimumDragLength)
            {
                if (shootForce.magnitude > _maximumShootSpeed)
                {
                    shootForce = shootForce.normalized * _maximumShootSpeed; // Limit shoot speed
                }

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
        if (Physics.Raycast(ray, out hit))
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
                _rb.AddForce(direction() * _shootStrength, ForceMode.Force);
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
