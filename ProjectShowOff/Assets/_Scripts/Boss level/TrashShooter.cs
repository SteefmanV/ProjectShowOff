using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashShooter : MonoBehaviour
{
    [SerializeField] private Vector2 _minMaxShootForce = new Vector2();
    [SerializeField] private Transform _trashHolder = null;


    public void Shoot(GameObject trashPrefab)
    {
        GameObject trashObject = Instantiate(trashPrefab, transform.position, trashPrefab.transform.rotation, _trashHolder);
        Rigidbody trashRB = trashObject.GetComponent<Rigidbody>();
        float shootSpeed = Random.Range(_minMaxShootForce.x, _minMaxShootForce.y);
        Vector3 force = transform.forward * shootSpeed; // direction to force
        trashRB.AddForce(force);
    }
}
