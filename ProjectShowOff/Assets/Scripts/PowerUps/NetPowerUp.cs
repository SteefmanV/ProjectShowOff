using Sirenix.OdinInspector;
using UnityEngine;

public class NetPowerUp : MonoBehaviour
{
    [ReadOnly] public bool netActive = false;

    [Required("Please add a Net prefab")]
    [SerializeField] private GameObject _netPrefab = null; 

    private Vector3 _netStartPos = new Vector3();
    private Vector3 _netStopPos = new Vector3();


    public void StartNet(Vector3 pPosition)
    {
        Debug.Log("<color=purple> Start Net at: " + _netStartPos + " </color>");
        netActive = true;
        _netStartPos = pPosition;
    }


    public void StopNet(Vector3 pPosition)
    {
        if (netActive)
        {
            _netStopPos = pPosition;

            GenerateNet();
            Debug.Log("<color=purple> Generate net from: " + _netStartPos + " to: " + _netStopPos + "</color>");
            netActive = false;
        }
    }


    private void GenerateNet()
    {
        GameObject netObject = Instantiate(_netPrefab, _netStartPos, Quaternion.identity, transform);

        Vector3 direction = _netStopPos - _netStartPos;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Debug.Log("NetAngle should be: " + angle);

        Vector3 netRotation = netObject.transform.rotation.eulerAngles;
        netRotation.z = angle;
        netObject.transform.rotation = Quaternion.Euler(netRotation);

        netObject.transform.localScale = new Vector3(direction.magnitude, .2f, .2f);
    }
}
