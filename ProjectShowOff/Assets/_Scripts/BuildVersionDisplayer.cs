using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuildVersionDisplayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _versionText = null;
    void Start()
    {
        _versionText.text = Application.version;
    }
}
