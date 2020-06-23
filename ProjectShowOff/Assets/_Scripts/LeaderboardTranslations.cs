using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderboardTranslations : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _fillInName = null;
    [SerializeField] private TextMeshProUGUI _enterName = null;
    [SerializeField] private TextMeshProUGUI _powerupsUsed = null;
    [SerializeField] private TextMeshProUGUI _fishSaved = null;
    [SerializeField] private TextMeshProUGUI _daily = null;
    [SerializeField] private TextMeshProUGUI _allTime = null;
    [SerializeField] private TextMeshProUGUI _continueButtonText = null;


    void Start()
    {
        LanguageManager languageManager = FindObjectOfType<LanguageManager>();

        if (languageManager != null)
        {
            Translation translation = languageManager.GetTranslation();
            _fillInName.text = translation.fillInYourName;
            _enterName.text = translation.enterName;
            _powerupsUsed.text = translation.powerUpsUsed;
            _fishSaved.text = translation.fishSaved;
            _daily.text = translation.daily;
            _allTime.text = translation.allTime;
            _continueButtonText.text = translation.continueButtonText;
        }
    }
}
