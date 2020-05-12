using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FibonacciSphere
{
    const int pointCount = 300;
    public static readonly Vector3[] pointsAroundSphere;

    static FibonacciSphere()
    {
        pointsAroundSphere = new Vector3[pointCount];

        float goldenRatio = (1 + Mathf.Sqrt(5)) / 2; // Golden ratio formula 
        float angleIncrement = Mathf.PI * 2 * goldenRatio;

        for (int i = 0; i < pointCount; i++)
        {
            float t = (float)i / pointCount; // time 0-1
            float inclination = Mathf.Acos(1 - 2 * t);
            float azimuth = angleIncrement * i;

            float x = Mathf.Sin(inclination) * Mathf.Cos(azimuth);
            float y = Mathf.Sin(inclination) * Mathf.Sin(azimuth);
            float z = Mathf.Cos(inclination);
            pointsAroundSphere[i] = new Vector3(x, y, z);
        }
    }
}