using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Lanuage", order = 1)]
public class Translation : ScriptableObject
{
    [Title("Translation")]
    public enum Languages { Dutch, English, German }
    public Languages language = Languages.Dutch;

    [Title("Main Screen")]
    public string playButton;
    public string controlsButton;

    [Title("Controls Screen")]
    public string textBubble1;
    public string textBubble2;
    public string textBubble3;
    public string backButton;

    [Title("Tutorial")]
    public string movementExplenation;
    public string saveTheAnimals;
    public string powerupExplenation;
    public string tutorialEnd;

    [Title("PlaceHOlder")]
    public string cutSceneText;

    [Title("Leaderboard")]
    public string powerUpsUsed;
    public string fishSaved;
    public string daily;
    public string allTime;
    public string continueButtonText;
    public string fillInYourName;
    public string enterName;
}
