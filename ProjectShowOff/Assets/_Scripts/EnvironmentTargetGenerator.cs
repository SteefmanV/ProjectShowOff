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
    [SerializeField] private LayerMask _environmentLayers;

    [Title("Position Settings")]
    [SerializeField] private Vector2 _minMaxX;
    [SerializeField] private Vector2 _minMaxY;
    [SerializeField] private Vector2 _minMaxZ;

    [Title("Test to check the _minRadius")]
    [SerializeField] private GameObject testPrefab = null;
    private const int MAX_TRIES = 100;
    private bool generateSpheres = false;


    public Vector3 GetValidPosition()
    {
        Vector3 randomPos = getRandomPosition();

        int tries = 0;
        while (!isPositionValid(randomPos))
        {
            randomPos = getRandomPosition();
            if (tries > MAX_TRIES) break;
        }

        return randomPos;
    }


    private bool isPositionValid(Vector3 pPosition)
    {
        return (Physics.OverlapSphere(pPosition, _minRadiusFromEnvironment, _environmentLayers).Length <= 0);
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
            Instantiate(testPrefab, GetValidPosition(), Quaternion.identity, transform);
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
