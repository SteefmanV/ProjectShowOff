using Sirenix.OdinInspector;
using UnityEngine;

public class airTrapPowerUp : MonoBehaviour
{
    [SerializeField] private GameObject _airTrapPrefab = null;
    [SerializeField] private LayerMask mask;
    [SerializeField] private GameObject player;
    [SerializeField] private float buildRange = 0f;
    [ReadOnly] public bool airTrapActive = false;
    private Vector3 startPosition;
    private Vector3 calculatedEndposition;
    private Vector3 endPosition;
    
    private Vector3 spawnPosition1;
    private Vector3 spawnPosition2;
    private Vector3 spawnPosition3;

    public void setUp(Vector3 pPosition, Vector3 pDirection)
    {
        startPosition = pPosition;

        RaycastHit hit;
        if(Physics.Raycast(startPosition, pDirection, out hit, Mathf.Infinity, mask))
        {
            Debug.DrawRay(startPosition, pDirection * 1000, Color.yellow);
            Debug.Log("pew");
            calculatedEndposition = hit.point;

            //calculating center spawnpoint
            spawnPosition2 = new Vector3((startPosition.x + calculatedEndposition.x) / 2, (startPosition.y + calculatedEndposition.y) / 2, (startPosition.z + calculatedEndposition.z) / 2);
            spawnPosition1 = new Vector3((startPosition.x + spawnPosition2.x) / 2, (startPosition.y + spawnPosition2.y) / 2, (startPosition.z + spawnPosition2.z) / 2);
            spawnPosition3 = new Vector3((spawnPosition2.x + calculatedEndposition.x) / 2, (spawnPosition2.y + calculatedEndposition.y) / 2, (spawnPosition2.z + calculatedEndposition.z) / 2);
        }

        airTrapActive = true;
    }

    public void createTrap(Vector3 pSpawnPoint)
    {
        Instantiate(_airTrapPrefab, pSpawnPoint, Quaternion.identity);
    }

    public void checkPositionToBuild(Vector3 currentPosition, Vector3 targetPosition)
    {
        float distance = Vector3.Distance(currentPosition, targetPosition);

            if (distance <= buildRange)
            {
                createTrap(targetPosition);
            }
    }

    public void landing()
    {
        createTrap(spawnPosition1);
        createTrap(spawnPosition2);
        createTrap(spawnPosition3);
        airTrapActive = false;
    }
}
