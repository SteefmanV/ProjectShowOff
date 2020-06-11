using System;
using System.Collections.Generic;


[Serializable]
public class Highscores
{
    public List<Score> scores = new List<Score>();

    public Highscores()
    {
        for (int i = 0; i < 25; i++)
        {
            scores.Add(new Score(0, ""));
        }
    }

    public Highscores(List<Score> pHighscores)
    {
        scores = pHighscores;
    }
}
