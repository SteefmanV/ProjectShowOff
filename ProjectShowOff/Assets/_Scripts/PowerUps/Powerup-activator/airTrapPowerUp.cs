using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class airTrapPowerUp : MonoBehaviour
{
    [ReadOnly] public bool airTrapActive = false;

    [SerializeField] private GameObject _airTrapPrefab = null;
    [SerializeField] private GameObject _player = null;
    [SerializeField] private LayerMask _rayCastMask = 0;

    private Vector3[] trapPosition = new Vector3[3];
    private Vector3 _startPosition;

    private float _totalTrapDistance = 0;
    private float _widthOffset = 0;
    private int _spawnedTraps = 0;


    private void Awake()
    {
        _widthOffset = _airTrapPrefab.GetComponent<MeshFilter>().sharedMesh.bounds.size.x + _player.GetComponent<MeshFilter>().sharedMesh.bounds.size.x; // Trap.width + player.width
    }


    private void Update()
    {
        if (airTrapActive) updateTrapSpawn();
    }


    public void Setup(Vector3 pPosition, Vector3 pDirection)
    {
        _startPosition = pPosition;

        if (Physics.Raycast(_startPosition, pDirection, out RaycastHit hit, Mathf.Infinity, _rayCastMask))
        {
            Vector3 calculatedEndposition = hit.point;
            _totalTrapDistance = (hit.point - _startPosition).magnitude;

            trapPosition[1] = (_startPosition + calculatedEndposition) / 2; // Middle trap position
            trapPosition[0] = (_startPosition + trapPosition[1]) / 2; // Left trap
            trapPosition[2] = (trapPosition[1] + calculatedEndposition) / 2; // Right trap        
        }

        airTrapActive = true;
    }


    public void Stop()
    {
        airTrapActive = false;
        _spawnedTraps = 0;
    }


    private void updateTrapSpawn()
    {
        float playerTraveledDistance = (_player.transform.position - _startPosition).magnitude;

        switch (_spawnedTraps)
        {
            case 0:
                if (playerTraveledDistance > _totalTrapDistance / 5 * 1 + _widthOffset)
                    CreateTrap(trapPosition[0]);
                break;
            case 1:
                if (playerTraveledDistance > _totalTrapDistance / 5 * 2 + _widthOffset)
                    CreateTrap(trapPosition[1]);
                break;
            case 2:
                if (playerTraveledDistance > _totalTrapDistance / 5 * 3 + _widthOffset)
                    CreateTrap(trapPosition[2]);
                break;
        }
    }


    private void CreateTrap(Vector3 pSpawnPoint)
    {
        Instantiate(_airTrapPrefab, pSpawnPoint, Quaternion.identity);
        _spawnedTraps++;
    }
}
