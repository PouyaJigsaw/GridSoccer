using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using MLAgents;




public partial class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public  GameObject[,] gameBoard = new GameObject [6 , 10];
    public  Vector3[,] playerBoard = new Vector3[6 , 10];
    public GameObject[] goals = new GameObject[2]; // 0 ==> Left ----- 1 ==> Right
    
    
    

    public GameObject greenPlayer;
    public GameObject redPlayer;
    
    public GameObject[] players = new GameObject[2]; // 0 ==> Red ------- 1 ==> Green
     private Vector3 greenPlayerPosInit;
     private Vector3 redPlayerPosInit;
     
     
    private Vector3 ballPos;
    private GameObject ball;

    public Text timerText;
    public float cycleTime;

    private Player greenPlayerScript;
    private Player redPlayerScript;
    private int changeNum = 0;

    
    [HideInInspector]
    public PlayerColor WhoScoredLastTime;
    #region Constants
    
    private const float rightGoalPosX = -0.6f;
    private const float rightGoalPosY = 0;
    private const float rightGoalPosZ = -7.1f;
    
    private const float leftGoalPosX = -0.6f;
    private const float leftGoalPosY = 0;
    private const float leftGoalPosZ = 5.9f;
    
    #endregion
    
    #region Enums
    public enum PlayerColor
    {
        Red, Green
    }
    #endregion
    
    private void Awake()
    {
        rightGoalPos = new Vector3(rightGoalPosX, rightGoalPosY, rightGoalPosZ);
        leftGoalPos = new Vector3(leftGoalPosX, leftGoalPosY, leftGoalPosZ);

        whoHasBall = PlayerColor.Green;
        
        if (instance == null)
            instance = this;
        
        
    }
    
    void Start()
    {
        InvokeRepeating("TimeInvoke",0.5f, cycleTime);
        InvokeRepeating("ZeroTheChangeBallOwnerNum", 0.5f, cycleTime + Time.deltaTime);
        InstantiateGameBoard();
        Debug.Log("Green Player Pos Init:" + greenPlayerPosInit);
        greenPlayerScript = players[1].GetComponent<Player>();
        redPlayerScript = players[0].GetComponent<Player>();
        
    }

    void Reset()
    {
        ResetCycleTime();
        ResetPlayerPosition();
        ChangeBallOwnerForEveryScore();
        StartCoroutine(PauseGameAfterScore());
        
    }

    private void ResetCycleTime()
    {
        cycle = 0;
        timerText.text = "Cycle: " + 0;
    }

    private void ResetPlayerPosition()
    {   Debug.Log("Green Player Pos Init:" + greenPlayerPosInit);
        players[1].transform.position = greenPlayerPosInit;
        players[0].transform.position = redPlayerPosInit;
        greenPlayerScript.playerPos =  greenPlayerPosInit;
        redPlayerScript.playerPos =  redPlayerPosInit;
    }

    IEnumerator PauseGameAfterScore()
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 1;
        
    }



}
