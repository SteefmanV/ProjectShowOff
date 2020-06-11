using System;

[Serializable]
public class Score
{
    public int score = 0;
    public string name = "";

    public Score() { }

    public Score(int pScore, string pName)
    {
        score = pScore;
        name = pName;
    }
}
