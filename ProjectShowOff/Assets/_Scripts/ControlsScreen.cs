using TMPro;
using UnityEngine;

public class ControlsScreen : MonoBehaviour
{


    private LanguageManager _languageManager = null;


    private void Start()
    {
        _languageManager = FindObjectOfType<LanguageManager>();
        updateTexts(_languageManager.GetTranslation());
        _languageManager.OnLanguageChanged += languageChanged;
    }


    private void OnDestroy()
    {
        _languageManager.OnLanguageChanged -= languageChanged;
    }


    private void languageChanged(object pSender, Translation pTranslation)
    {
        updateTexts(pTranslation);
    }


    private void updateTexts(Translation pTranslation)
    {

    }

}
