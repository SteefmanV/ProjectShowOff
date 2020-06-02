using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolEffect : MonoBehaviour
{
    [SerializeField] private Material sebasMat = null;
    private string[] sebasCode = new string[] { "s", "c", "r", "u", "m" };
    private int index = 0;

  //  private Material[] oldMats;
    Dictionary<MeshRenderer, Material> oldMats = new Dictionary<MeshRenderer, Material>();


    void Update()
    {
        if(Input.anyKeyDown)
        {
            if(Input.GetKeyDown(sebasCode[index])) index++;
            else index = 0;
        }

        if(index == sebasCode.Length) sebasAction();
    }


    private void sebasAction()
    {
        Debug.Log("Sebastian");
        index = 0;

        MeshRenderer[] filters = FindObjectsOfType<MeshRenderer>();
        oldMats.Clear();
        for (int i = 0; i < filters.Length; i++)
        {
            oldMats.Add(filters[i], filters[i].material);
            filters[i].material = sebasMat;
        }

        StartCoroutine(resetMaterial());
    }


    private IEnumerator resetMaterial()
    {
        yield return new WaitForSeconds(5);
        MeshRenderer[] filters = FindObjectsOfType<MeshRenderer>();
        for (int i = 0; i < filters.Length; i++)
        {
            if(oldMats.ContainsKey(filters[i])) filters[i].material = oldMats[filters[i]];
        }
    }
}
