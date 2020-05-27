using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CameraView
{
    public static bool isInCameraView(Vector3 pPosition)
    {
        Vector3 viewportPoint = Camera.main.WorldToViewportPoint(pPosition);
        return (new Rect(0, 0, 1, 1).Contains(viewportPoint));
    }
}
