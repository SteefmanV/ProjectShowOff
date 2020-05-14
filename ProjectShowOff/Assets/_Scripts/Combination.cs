using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

[CreateAssetMenu(fileName = "Powerup-Combination", order = 1)]
public class Combination : ScriptableObject
{
    public GameObject powerUpIcon;
    public bool bottle;
    public bool straw;
    public bool rings;


    public bool hasCombination(int pBottles, int pStraws, int pRings)
    {
        int totalObjects = pBottles + pStraws + pRings;

        // Check if enough items
        if (bottle && pBottles < 1) return false;
        else if (straw && pStraws < 1) return false;
        else if (rings && pRings < 1) return false;

        if (!bottle && pBottles > 0) return false;
        else if (!straw && pStraws > 0) return false;
        else if (!rings && pRings > 0) return false;

        return (totalObjects >= 3);
    }
}
