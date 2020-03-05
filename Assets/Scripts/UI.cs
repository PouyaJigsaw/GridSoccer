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
        greenPlayerScript.SetReward(0f);
        redPlayerScript.SetReward(+1f);
        greenPlayerScript.Done();
        redPlayerScript.Done();
        Reset();
    }

    public void GreenScores()
    {
        WhoScoredLastTime = PlayerColor.Green;
        greenScore++;
        greenScoreText.text = greenScore.ToString();
        greenPlayerScript.AddReward(+1f);
        redPlayerScript.SetReward(0f);
        greenPlayerScript.Done();
        redPlayerScript.Done();
        Reset();
    }

    private void TimeInvoke()
    {
        cycle += 1;
        timerText.text = "Cycle: " + cycle;
    }
}
