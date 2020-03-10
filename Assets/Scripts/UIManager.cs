using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text redText;
    public Text GreenText;
    private int greenScore;
    private int redScore;

    public static UIManager instance;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
        greenScore = 0;
        redScore = 0;
    }

    // Update is called once per frame
    public void RedScorePlus()
    {
        redScore++;
        redText.text = redScore.ToString();
    }

    public void greenScorePlus()
    {
        greenScore++;
        GreenText.text = greenScore.ToString();
    }
}
