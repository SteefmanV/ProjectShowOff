using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thrash : MonoBehaviour
{
    [SerializeField] private float _startFallSpeed = 1;
    private GameManager _gameManager = null;

    private void Awake()
    {
        GameObject managerHolder = GameObject.FindGameObjectWithTag("GameManager");
        if (managerHolder == null)
        {
            Debug.LogWarning("GameManager gameobject does not exist");
            return;
        }

        _gameManager = managerHolder.GetComponent<GameManager>();
    }


    void Update()
    {
        Vector3 position = transform.position;
        position.y -= (_startFallSpeed * Time.deltaTime);
        transform.position = position;
    }


    private void OnMouseDown()
    {
        _gameManager.ThrashDestroyed();
        Destroy(gameObject);
    }
}
