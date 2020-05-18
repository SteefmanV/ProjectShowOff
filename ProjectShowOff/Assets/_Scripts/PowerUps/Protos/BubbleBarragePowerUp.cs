using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BubbleBarragePowerUp : MonoBehaviour
{
    [ReadOnly] public bool bubbleBarrageActive = false;

    [SerializeField] private GameObject _bubbleShooterPrefab = null;
    [SerializeField] private GameObject _player = null;
    [SerializeField] private LayerMask _rayCastMask;

    private Vector3[] bubbleShooterPositions = new Vector3[3];
    private Vector3 _startPosition;
    private Vector3 _shootDirection;

    private float _totalBubbleShooterDistance = 0;
    private float _widthOffset = 0;
    private int _spawnedBubbleShooter = 0;
    

    private void Awake()
    {
        _widthOffset = _bubbleShooterPrefab.GetComponent<MeshFilter>().sharedMesh.bounds.size.x + _player.GetComponent<MeshFilter>().sharedMesh.bounds.size.x; // Trap.width + player.width
    }


    private void Update()
    {
        if (bubbleBarrageActive) updateBubbleSpawn();
    }

    public void setUp(Vector3 pPosition, Vector3 pDirection)
    {
        _startPosition = pPosition;
        _shootDirection = pDirection;

        if (Physics.Raycast(_startPosition, _shootDirection, out RaycastHit hit, Mathf.Infinity, _rayCastMask))
        {
            Vector3 calculatedEndposition = hit.point;
            _totalBubbleShooterDistance = (hit.point - _startPosition).magnitude;

            bubbleShooterPositions[1] = (_startPosition + calculatedEndposition) / 2; // Middle trap position
            bubbleShooterPositions[0] = (_startPosition + bubbleShooterPositions[1]) / 2; // Left trap
            bubbleShooterPositions[2] = (bubbleShooterPositions[1] + calculatedEndposition) / 2; // Right trap        
        }
        bubbleBarrageActive = true;
    }

    public void Land()
    {
        bubbleBarrageActive = false;
        _spawnedBubbleShooter = 0;
    }

    private void updateBubbleSpawn()
    {
        float playerTraveledDistance = (_player.transform.position - _startPosition).magnitude;

        switch (_spawnedBubbleShooter)
        {
            case 0:
                if (playerTraveledDistance > _totalBubbleShooterDistance / 5 * 1 + _widthOffset)
                    CreateTrap(bubbleShooterPositions[0]);
                break;
            case 1:
                if (playerTraveledDistance > _totalBubbleShooterDistance / 5 * 2 + _widthOffset)
                    CreateTrap(bubbleShooterPositions[1]);
                break;
            case 2:
                if (playerTraveledDistance > _totalBubbleShooterDistance / 5 * 3 + _widthOffset)
                    CreateTrap(bubbleShooterPositions[2]);
                break;
        }
    }

    private void CreateTrap(Vector3 pSpawnPoint)
    {
        GameObject barrage = Instantiate(_bubbleShooterPrefab, pSpawnPoint, Quaternion.Euler(_shootDirection));
        StartCoroutine(delayedDelete(barrage));
        _spawnedBubbleShooter++;
    }

    private IEnumerator delayedDelete(GameObject pDelete)
    {
        yield return new WaitForSeconds(1f);
        Destroy(pDelete);
    }
}
