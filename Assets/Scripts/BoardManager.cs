using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{

    public static BoardManager instance;
    
    public GameObject greenPlayer;

    public GameObject redPlayer;

    public GameObject redGoal;

    public GameObject greenGoal;

    public GameObject middleBoardEdge;
    public GameObject rightEdge;
    public GameObject leftEdge;
    public GameObject upEdge;
    public GameObject downEdge;
    public GameObject goalEdgeUp_Green;
    public GameObject goalEdgeDown_Green;
    public GameObject goalEdgeUp_Red;
    public GameObject goalEdgeDown_Red;
    private Vector3 initPos_greenPlayer;
    private Vector3 initPos_redPlayer;

    private Player greenPlayerScript;
    private Player redPlayerScript;
    private float changeNum;

    [HideInInspector] public PlayerColor whoHasBall;

    [HideInInspector] public PlayerColor whoScoredLastTime;
    public enum PlayerColor
    {
        Red, Green
    }


    private void Awake()
    {
        whoHasBall = PlayerColor.Green;
        
        if (instance == null)
            instance = this;


    }

    public float cycleTime;
    // Start is called before the first frame update
    void Start()
    {
        cycleTime = Time.fixedDeltaTime;

        greenPlayerScript = greenPlayer.GetComponent<Player>();
        redPlayerScript = redPlayer.GetComponent<Player>();
        
        initPos_greenPlayer = greenPlayer.transform.position;
        initPos_redPlayer = redPlayer.transform.position;
        
        InvokeRepeating("ZeroTheChangeBallOwnerNum", 0.5f, cycleTime + cycleTime/2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ResetPlayerPosition()
    {
        greenPlayerScript.playerPos = initPos_greenPlayer;
        redPlayerScript.playerPos = initPos_redPlayer;
    }
    private void ZeroTheChangeBallOwnerNum()
    {
        if (changeNum > 0)
        {
            changeNum = 0;
        }
    }
    
    private void ChangeBallOwnerForEveryScore()
    {
        if (whoScoredLastTime == PlayerColor.Green)
        {
            whoHasBall = PlayerColor.Green;
        }
        else
        {
            whoHasBall = PlayerColor.Red;
        }
    }

    void Reset()
    {
        ResetPlayerPosition();
        ChangeBallOwnerForEveryScore();
    }
    
    public void ChangeBallOwner()
    {
        if (changeNum < 1)
        {
            if (whoHasBall == PlayerColor.Green)
            {
                whoHasBall = PlayerColor.Red;
            }
            else
            {
                whoHasBall = PlayerColor.Green;
            }
        }

        changeNum++;
    }



    public void RedScores()
    {
        whoScoredLastTime = PlayerColor.Red; 
    }

    public void GreenScores()
    {
        whoScoredLastTime = PlayerColor.Green;  
    }
}
