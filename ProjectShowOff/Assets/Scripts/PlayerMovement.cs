using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _shootSpeed = 1;
    private Rigidbody _rb = null;

    [SerializeField] private Vector2 minPower;
    [SerializeField] private Vector2 maxPower;
    [SerializeField] private LineRenderer _lineRender;

    private Vector3 startPosition;
    private Vector3 endPosition;
    private Vector2 force;
    private Camera cam;
    private bool isDragging = false;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        //arrowPos();
        dragAndShoot();

        Debug.Log("Is mouse over object? " + isMouseOverObject());
    }


    //private void arrowPos()
    //{
    //    float degAngle = Mathf.Rad2Deg * angleBetweenMouseAndObject();
    //    Vector3 currentRotation = _arrowObject.transform.rotation.eulerAngles;
    //    _arrowObject.transform.rotation = Quaternion.Euler(currentRotation.x, currentRotation.y, degAngle);
    //}


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


    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collision");
        if (collision.gameObject.CompareTag("sticky"))
        {
            _rb.velocity = Vector3.zero;
        }
    }


    private void clickAndShoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (_rb.velocity.magnitude < 0.01f)
            {
                _rb.AddForce(direction() * _shootSpeed, ForceMode.Force);
            }
        }
    }


    private void dragAndShoot()
    {
        // Start drag
        if (Input.GetMouseButtonDown(0) && isMouseOverObject())
        {
            if (_rb.velocity.magnitude < 0.01f)
            {
                isDragging = true;
                startPosition = transform.position;
                startPosition.z = -1;
                _lineRender.positionCount = 2;
                _lineRender.SetPosition(0, startPosition);
            }
        }

        if (isDragging)
        {
            // Update drag line
            if (Input.GetMouseButton(0))
            {
                Vector3 mouse = cam.ScreenToWorldPoint(Input.mousePosition);
                mouse.z = -1;
                _lineRender.SetPosition(1, mouse);
                _lineRender.SetPosition(0, startPosition);
            }

            // Drag stop
            if (Input.GetMouseButtonUp(0))
            {
                if (!isMouseOverObject())
                {
                    endPosition = cam.ScreenToWorldPoint(Input.mousePosition);
                    force = new Vector2(Mathf.Clamp(startPosition.x - endPosition.x, minPower.x, maxPower.x), Mathf.Clamp(startPosition.y - endPosition.y, minPower.y, maxPower.y));
                    _rb.AddForce(force * _shootSpeed, ForceMode.Impulse);
                }

                _lineRender.positionCount = 0;

                isDragging = false;
            }
        }
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
}
