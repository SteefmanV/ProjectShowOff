using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


[ExecuteInEditMode]
public class Tentacle : MonoBehaviour
{
    [SerializeField] private GameObject _prefab = null;


    [SerializeField] private float rotation = 0;
    [SerializeField] private float sineStrength = 0;
    [SerializeField] private int sineLength = 10;
    [SerializeField] private List<Transform> children = new List<Transform>();
    [SerializeField] private float sineOffset = 1;

    [OnValueChanged("generateObjects")]
    [SerializeField] private int objectCount = 10;
    [SerializeField] private float offset = 1;


    void Start()
    {
        generateObjects();
    }


    // Update is called once per frame
    void Update()
    {
        rotateChildren();
    }


    private void rotateChildren()
    {
        for (int i = 5; i < children.Count; i++)
        {
            float rot = rotation * (Mathf.Sin((i + (sineOffset * 0.1f)) * sineLength)) * sineStrength;
            if (i == 6)
            {
                children[i].transform.localRotation = Quaternion.Euler(0, 0, -rot);
            }
            else
            { 
                children[i].transform.localRotation = Quaternion.Euler(0, 0, rot);
            }

        }
    }


    private void generateObjects()
    {
        clearChildren();
        Transform previousTransform = transform;
        Vector3 pos = Vector3.zero;
        for (int i = 0; i < objectCount; i++)
        {
           GameObject newObject = Instantiate(_prefab, Vector3.zero, Quaternion.identity, previousTransform);
            newObject.transform.localPosition = new Vector3(0, offset, 0);
            children.Add(newObject.transform);
            previousTransform = newObject.transform;
            pos.y += offset;
        }
    }

    private void clearChildren()
    {
        if(children.Count > 0) DestroyImmediate(transform.GetChild(0).gameObject);
        children.Clear();
    }
}
