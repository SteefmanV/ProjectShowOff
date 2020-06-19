using Sirenix.OdinInspector;
using UnityEngine;

public class NetPowerUp : MonoBehaviour
{
    [ReadOnly] public bool netActive = false;

    [Required("Please add a Net prefab")]
    [SerializeField] private GameObject _netPrefab = null;
    [SerializeField] private float _offset = 2;

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
        Vector3 direction = _netStopPos - _netStartPos;
        Vector3 startPos = _netStartPos -= direction.normalized * _offset;


        GameObject netObject = Instantiate(_netPrefab, startPos, Quaternion.identity, transform);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Debug.Log("NetAngle should be: " + angle);

        Net net = netObject.GetComponentInChildren<Net>();
        Transform actualNet = net.transform;

        Vector3 netRotation = actualNet.rotation.eulerAngles;
        netRotation.z = angle;
        netObject.transform.rotation = Quaternion.Euler(netRotation);

        actualNet.localScale = new Vector3(direction.magnitude + _offset * 2, .2f, .2f);
        actualNet.transform.localPosition = actualNet.localScale / 2;

        net._netEnd.transform.position = _netStopPos;
        net._netStart.transform.position = _netStartPos;
    }
}
