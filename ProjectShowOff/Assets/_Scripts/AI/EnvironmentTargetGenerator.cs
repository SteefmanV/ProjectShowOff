using JetBrains.Annotations;
using Sirenix.OdinInspector;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

[ExecuteInEditMode]
public class EnvironmentTargetGenerator : MonoBehaviour
{
    [Title("AI Settings")]
    [SerializeField] private float _minRadiusFromEnvironment = .5f;

    [Title("Position Settings")]
    [SerializeField] private Vector2 _minMaxX = new Vector2();
    [SerializeField] private Vector2 _minMaxY = new Vector2();
    [SerializeField] private Vector2 _minMaxZ = new Vector2();

    [Title("Test to check the _minRadius")]
    [SerializeField] private GameObject testPrefab = null;
    [SerializeField] private LayerMask testEnvironmentLayers = 0;

    private const int MAX_TRIES = 100;

    private bool generateSpheres = false;


    public Vector3 GetValidPosition(LayerMask pEnvironmentLayers)
    {
        Vector3 randomPos = getRandomPosition();

        int tries = 0;
        while (!isPositionValid(randomPos, pEnvironmentLayers))
        {
            randomPos = getRandomPosition();
            if (tries > MAX_TRIES) break;
        }

        return randomPos;
    }


    private bool isPositionValid(Vector3 pPosition, LayerMask pEnvironmentLayers)
    {
        return (Physics.OverlapSphere(pPosition, _minRadiusFromEnvironment, pEnvironmentLayers).Length <= 0);
    }


    private Vector3 getRandomPosition()
    {
        return new Vector3(
            Random.Range(_minMaxX.x, _minMaxX.y),
            Random.Range(_minMaxY.x, _minMaxY.y),
            Random.Range(_minMaxZ.x, _minMaxZ.y)
        );
    }


    [Button("Generate 100 random Spheres")]
    private void generate()
    {
        for (int i = 0; i < 100; i++)
        {
            Instantiate(testPrefab, GetValidPosition(testEnvironmentLayers), Quaternion.identity, transform);
        }

        generateSpheres = true;
    }


    [ShowIf("generateSpheres")]
    [Button("Clean Test")]
    private void clean()
    {
        for (int i = this.transform.childCount; i > 0; --i)
            DestroyImmediate(this.transform.GetChild(0).gameObject);

        generateSpheres = false;
    }
}
