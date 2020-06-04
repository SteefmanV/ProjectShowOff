using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    private static LanguageManager _instance;
    public static LanguageManager instance { get { return _instance; } }

    public event EventHandler<Translation> OnLanguageChanged;

    [SerializeField] private Translation.Languages currentLanaguage;

    [Title("Languages")]
    [SerializeField] private Translation _dutch = null;
    [SerializeField] private Translation _english = null;
    [SerializeField] private Translation _german = null;


    private void Awake()
    {
        // Singleton
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }


    public Translation GetTranslation()
    {
        Translation translation = null;

        switch (currentLanaguage)
        {
            case Translation.Languages.Dutch:
                translation = _dutch;
                break;
            case Translation.Languages.English:
                translation = _english;
                break;
            case Translation.Languages.German:
                translation = _german;
                break;
        }

        return translation;
    }


    public void SetLanguage(Translation.Languages pLanguage)
    {
        currentLanaguage = pLanguage;
        OnLanguageChanged?.Invoke(this, GetTranslation());
    }
}
