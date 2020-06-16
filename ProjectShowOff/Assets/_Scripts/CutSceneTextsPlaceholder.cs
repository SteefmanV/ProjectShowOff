using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CutSceneTextsPlaceholder : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _placeHolderText;
    void Start()
    {
        _placeHolderText.text = FindObjectOfType<LanguageManager>().GetTranslation().cutSceneText;
    }
}
