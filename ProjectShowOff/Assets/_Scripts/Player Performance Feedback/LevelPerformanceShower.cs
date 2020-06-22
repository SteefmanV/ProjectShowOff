using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelPerformanceShower : MonoBehaviour
{
    [Title("UI Elements")]
    [SerializeField] private StarDisplay _averageDislay = null;
    [SerializeField] private StarDisplay _displayLevel1 = null;
    [SerializeField] private StarDisplay _displayLevel2 = null;
    [SerializeField] private StarDisplay _displayLevel3 = null;
    [SerializeField] private StarDisplay _displayLevel4 = null;


    private PlaySession _session;

    void Start()
    {
        _session = FindObjectOfType<PlaySession>();

        int[] scores = _session.GetScores();

        _displayLevel1.SetPoints(scores[0]);
        _displayLevel2.SetPoints(scores[1]);
        _displayLevel3.SetPoints(scores[2]);
        _displayLevel4.SetPoints(scores[3]);
        _averageDislay.SetPoints(scores.Sum());
    }
}
