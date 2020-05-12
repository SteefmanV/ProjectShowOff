using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "Wave", menuName = "SpawnSystem/Wave", order = 1)]
public class Wave : ScriptableObject
{
    [TabGroup("Object Settings"), InfoBox("Number of objects spawned this wave", InfoMessageType.None)]
    public int objectCount = 1;

    [Title("Objects in wave:")]
    [TabGroup("Object Settings"), AssetList(Path = "/Prefabs/Thrash"), Space, Tooltip("The selected objects will be used in this wave")]
    public List<GameObject> Objects = new List<GameObject>();

    [TabGroup("Object Settings"), Space, InlineEditor(InlineEditorModes.LargePreview), AssetList(Path = "/Prefabs/Thrash"), ShowIf("@this.Objects.Count > 0"), SerializeField]
    private GameObject QuickThrashObjectPreview;


    [TabGroup("Time Settings"), Tooltip("The seconds between each spawn")]
    [Title("Time settings")]
    public float timeBetweenSpawn = 5;

    [TabGroup("Time Settings"), Tooltip("A random number between the x & y get's selected and substracted from the 'Time between Spawn' every spawn"), MinMaxSlider(0, 2, true)]
    public Vector2 minMaxTimeRandomeness;
}
