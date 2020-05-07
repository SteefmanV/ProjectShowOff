using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave", menuName = "SpawnSystem/Wave", order = 1)]
public class Wave : ScriptableObject
{
    [Tooltip("The objects used in this wave")]
    public GameObject[] Objects = new GameObject[0];

    [Tooltip("Number of objects spawned this wave")]
    public int objectCount = 1;

    [Tooltip("The seconds between each spawn")]
    public float timeBetweenSpawn = 5;

    [Tooltip("A random number between the x & y get's selected and substracted from the 'Time between Spawn' every spawn")]
    public Vector2 minMaxTimeRandomeness;
}
