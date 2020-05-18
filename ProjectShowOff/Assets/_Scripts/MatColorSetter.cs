using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[ExecuteInEditMode]
public class MatColorSetter : MonoBehaviour
{
    [OnValueChanged("updateColor")]
    [SerializeField] private Color _materialColor;

    private void Awake()
    {
        updateColor();      
    }

    private void updateColor()
    {
        MeshRenderer render = GetComponent<MeshRenderer>();
        render.material.color = _materialColor;
    }
}
