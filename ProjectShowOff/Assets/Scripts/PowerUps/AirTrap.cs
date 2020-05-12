using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class AirTrap : MonoBehaviour
{
    public GameObject trashObject = null;

    [SerializeField] private float _trapDurationSec = 5f;
    [SerializeField, Required] private GameObject _holdPosition = null;

    private float _timer = 0f;
    private bool _hasTrash = false;
    private float _maxHeight = 6.5f;


    private void Update()
    {
        if (_hasTrash)
        {
            _timer += Time.deltaTime;
            if (_timer > _trapDurationSec) destroyAndReset();

            transform.Translate(Vector3.up * Time.deltaTime, Space.World);
        }

        if (transform.position.y >= _maxHeight) destroyAndReset();
    }


    // Update is called once per frame
    public void OnCollisionEnter(Collision collision)
    {
        if (!_hasTrash)
        {
            if (collision.gameObject.CompareTag("thrash"))
            {
                _hasTrash = true;

                trashObject = collision.gameObject;
                trashObject.transform.parent = _holdPosition.transform;

                trashObject.GetComponent<BoxCollider>().enabled = false;
                trashObject.GetComponent<Rigidbody>().isKinematic = true;

                trashObject.transform.position = _holdPosition.transform.position;
                trashObject.GetComponent<Thrash>().enabled = false;

                GetComponent<BoxCollider>().enabled = false;
            }
        }
    }


    public void destroyAndReset()
    {
        if (_hasTrash)
        {
            trashObject.transform.parent = null;
            trashObject.GetComponent<Thrash>().enabled = true;
            trashObject.GetComponent<BoxCollider>().enabled = true;
            trashObject.GetComponent<Rigidbody>().isKinematic = false;
        }

        Destroy(gameObject);
    }
}
