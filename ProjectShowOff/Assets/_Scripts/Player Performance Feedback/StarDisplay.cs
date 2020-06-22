using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarDisplay : MonoBehaviour
{
    [SerializeField] private UIStar _star1 = null;
    [SerializeField] private UIStar _star2 = null;
    [SerializeField] private UIStar _star3 = null;

    [Title("Settings")]
    [SerializeField] private int _pointsFor1Star = 50;
    [SerializeField] private int _pointsFor2Star = 100;
    [SerializeField] private int _pointsFor3Star = 150;

    public void SetPoints(int pPoints)
    {
        _star1.on = pPoints > _pointsFor1Star;
        _star2.on = pPoints > _pointsFor2Star;
        _star3.on = pPoints > _pointsFor3Star;
    }

}
