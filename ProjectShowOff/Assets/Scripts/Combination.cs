using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

[CreateAssetMenu(fileName = "Powerup-Combination", order = 1)]
public class Combination : ScriptableObject
{
    public GameObject powerUpIcon;
    public bool bottle;
    public bool straw;
    public bool rings;


    public bool hasCombination(int pBottles, int pStraws, int pRingss)
    {
        int totalObjects = pBottles + pStraws + pRingss;

        if (bottle && pBottles < 1) return false;
        else if (straw && pStraws < 1) return false;
        else if (rings && pRingss < 1) return false;

        return (totalObjects >= 3);
    }
}
