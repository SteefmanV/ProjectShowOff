using UnityEngine;

public struct BoidData
{
    public Vector3 position;
    public Vector3 direction;

    public Vector3 flockDirection;
    public Vector3 flockCentre;
    public Vector3 avoidDirection;
    public int flockSize;


    public void Initialize(Vector3 pPosition, Vector3 pDirection)
    {
        position = pPosition;
        direction = pDirection;
    }
}
