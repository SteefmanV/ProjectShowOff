using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirTrap : MonoBehaviour
{
    [SerializeField] private float _trapDuration = 5f;
    [SerializeField] private GameObject holdPosition;
    private float _timer = 0f;
    private bool hasTrash = false;
    public GameObject trashObject = null;
    private float maxHeight = 6.5f;

    private void Update()
    {
        if (hasTrash)
        {
            _timer += Time.deltaTime;

            if (_timer > _trapDuration)
            {
                destroyAndReset();
            }

            transform.Translate(Vector3.up * Time.deltaTime, Space.World);

            if (transform.position.y >= 6.5f)
            {
                destroyAndReset();
            }
        }

        if (transform.position.y >= maxHeight)
        {
            destroyAndReset();
        } 
    }

    // Update is called once per frame
    public void OnCollisionEnter(Collision collision)
    {
        if (!hasTrash)
        {
            if (collision.gameObject.CompareTag("thrash"))
            {
                hasTrash = true;
                collision.gameObject.transform.parent = holdPosition.transform;
                trashObject = collision.gameObject;

                trashObject.GetComponent<BoxCollider>().enabled = false;
                trashObject.GetComponent<Rigidbody>().isKinematic = true;

                trashObject.transform.position = holdPosition.transform.position;
                trashObject.GetComponent<Thrash>().enabled = false;

                GetComponent<BoxCollider>().enabled = false;
            }
        }
        /*else
        {
            if (hasTrash)
            {
                if (collision.gameObject.CompareTag("Player"))
                {
                    destroyAndReset();
                }
            }
        }*/

    }

    public void destroyAndReset()
    {
        if (hasTrash)
        {
            trashObject.transform.parent = null;
            trashObject.GetComponent<Thrash>().enabled = true;
            trashObject.GetComponent<BoxCollider>().enabled = true;
            trashObject.GetComponent<Rigidbody>().isKinematic = false;
        }

        Destroy(gameObject);
    }
}
