using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject _jetPack = null;
    [SerializeField] private GameObject _characterObject = null;
    [SerializeField] private GameObject _arrowIndicator = null;

    private PlayerMovement _playerMovement = null;
    private Rigidbody _rb;

    [Title("Player Models")]
    [SerializeField] private GameObject _playerIdle = null;
    [SerializeField] private GameObject _playerMoving = null;
    [SerializeField] private GameObject _playerCharging = null;

    [Title("Colliders")]
    [SerializeField] private Collider _movingCollider = null;

    [Title("Settings")]
    [SerializeField] private float _arrowScaleFactor = 1;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _playerMovement = GetComponent<PlayerMovement>();
    }


    private void Update()
    {
        if (_rb.velocity.magnitude > .1f)
        {
            setObjectActive(_playerMoving);
            _movingCollider.enabled = true;

            Vector3 target = _playerMovement.transform.position + _rb.velocity;
            target.z = transform.position.z;
            transform.LookAt(target, Vector3.up);
        }
        else if (_playerMovement.mouseDrag.magnitude > _playerMovement.minimumDragLength)
        {
            setObjectActive(_playerCharging);
            _arrowIndicator.SetActive(true);


            float angle = Mathf.Atan2(_playerMovement.mouseDrag.y, _playerMovement.mouseDrag.x);
            _arrowIndicator.transform.rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);

            float arrowScale = _playerMovement.shootForce * _arrowScaleFactor;
            _arrowIndicator.transform.localScale = new Vector3(arrowScale, 1, 1);
        }
        else
        {
            setObjectActive(_playerIdle);
            _movingCollider.enabled = false;
            _rb.velocity = Vector3.zero;
        }
    }


    private void setObjectActive(GameObject pObject)
    {
        if (!pObject.activeSelf)
        {
            _arrowIndicator.SetActive(false);
            _playerMoving.SetActive(false);
            _playerCharging.SetActive(false);
            _playerIdle.SetActive(false);
            _movingCollider.enabled = false;

            pObject.SetActive(true);
        }

    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("sticky"))
        {
            _movingCollider.enabled = false;
        }
    }


            public void SetJetpack(bool pActive)
    {
        _jetPack.SetActive(pActive);
    }
}
