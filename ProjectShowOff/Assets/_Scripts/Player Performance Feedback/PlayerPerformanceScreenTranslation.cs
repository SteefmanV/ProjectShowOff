using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerPerformanceScreenTranslation : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _continueButton = null;


    void Start()
    {
        LanguageManager languageManager = FindObjectOfType<LanguageManager>();
        Translation translation = languageManager.GetTranslation();

        _continueButton.text = translation.continueButtonText;
    }
}
