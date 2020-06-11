using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScreen : MonoBehaviour
{
    private LanguageManager _languageManager = null;

    [Title("Main screen")]
    [SerializeField] private TextMeshProUGUI _playText = null;
    [SerializeField] private TextMeshProUGUI _controlsText = null;

    [Title("Controls screen")]
    [SerializeField] private TextMeshProUGUI _textBubble1 = null;
    [SerializeField] private TextMeshProUGUI _textBubble2 = null;
    [SerializeField] private TextMeshProUGUI _textBubble3 = null;
    [SerializeField] private TextMeshProUGUI _backButton = null;

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


    public void LoadScene(string pSceneToLoad)
    {
        SceneManager.LoadScene(pSceneToLoad);
    }


    public void SetLanguage(string pLanguage)
    {
        switch (pLanguage)
        {
            case "Dutch":
                _languageManager.SetLanguage(Translation.Languages.Dutch);
                break;
            case "English":
                _languageManager.SetLanguage(Translation.Languages.English);
                break;
            case "German":
                _languageManager.SetLanguage(Translation.Languages.German);
                break;
        }
    }


    public void ResetSessionScore()
    {
        FindObjectOfType<PlaySession>().StartNewSession();
    }


    private void languageChanged(object pSender, Translation pTranslation)
    {
        updateTexts(pTranslation);
    }


    private void updateTexts(Translation pTranslation)
    {
        // Main Screen
        _playText.text = pTranslation.playButton;
        _controlsText.text = pTranslation.controlsButton;

        // Controls screen
        _textBubble1.text = pTranslation.textBubble1;
        _textBubble2.text = pTranslation.textBubble2;
        _textBubble3.text = pTranslation.textBubble3;
        _backButton.text = pTranslation.backButton;
    }
}
