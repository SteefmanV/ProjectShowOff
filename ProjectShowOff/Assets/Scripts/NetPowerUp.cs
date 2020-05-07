using Sirenix.OdinInspector;
using UnityEngine;

public class NetPowerUp : MonoBehaviour
{
    [Required("Please add a Net prefab")]
    [SerializeField] private GameObject _netPrefab = null;
    [ReadOnly] public bool netActive = false;
    private Vector3 netStartPos = new Vector3();
    private Vector3 netStopPos = new Vector3();


    public void startNet(Vector3 pPosition)
    {
        Debug.Log("<color=purple> Start Net at: " + netStartPos + " </color>");
        netActive = true;
        netStartPos = pPosition;
    }


    public void stopNet(Vector3 pPosition)
    {
        if (netActive)
        {
            netStopPos = pPosition;

            generateNet();
            Debug.Log("<color=purple> Generate net from: " + netStartPos + " to: " + netStopPos + "</color>");
            netActive = false;
        }
    }


    private void generateNet()
    {
        GameObject netObject = Instantiate(_netPrefab, netStartPos, Quaternion.identity, transform);

        Vector3 direction = netStopPos - netStartPos;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Debug.Log("NetAngle should be: " + angle);

        Vector3 netRotation = netObject.transform.rotation.eulerAngles;
        netRotation.z = angle;
        netObject.transform.rotation = Quaternion.Euler(netRotation);

        netObject.transform.localScale = new Vector3(direction.magnitude, .2f, .2f);
    }
}
