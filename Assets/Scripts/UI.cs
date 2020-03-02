using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class GameManager // UI Manager
{
    private float cycle;
    public Text greenScoreText;
    public Text redScoreText;
    private int greenScore = 0;
    private int redScore = 0;

    public void RedScores()
    {
        WhoScoredLastTime = PlayerColor.Red;
        redScore++;
        redScoreText.text = redScore.ToString();
        Reset();
    }

    public void GreenScores()
    {
        WhoScoredLastTime = PlayerColor.Green;
        greenScore++;
        greenScoreText.text = greenScore.ToString();
        Reset();
    }

    private void TimeInvoke()
    {
        cycle += 1;
        timerText.text = "Cycle: " + cycle;
    }
}
