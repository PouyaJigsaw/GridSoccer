using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using MLAgents;
using UnityEngine.Experimental.PlayerLoop;
using Random = System.Random;

public class PlayerAgent : Agent
{
    private float cycle;
    public bool hasBall;
    public Vector3 playerPos;
    public GameObject ball;

    private int hasBall_int;
    [SerializeField] private BoardManager localBoardManager;
private GameObject opponent;

    private GameObject opponentGoal;

    private GameObject goal;

    public BoardManager.PlayerColor playerColor;

    private bool flagGoal;
    private bool flagMoved;
    private int NWSE;
    private int direction;
    private Vector2 DistanceToGoal;
    private float blockPace = 1.2f;
    
    
    private float middleLine;
    private float leftLine;
    private float rightLine;
    private float upLine;
    private float downLine;

    private float goalLineDown_Green;
    private float goalLineUp_Green;
    private float goalLineDown_Red;
    private float goalLineUp_Red;

    public PlayerDirection _PlayerDirection;
    public enum PlayerDirection
    {
        Up,Down,Left,Right,Stay
    }
    
    public enum PlayerType
    {
        WASD, UpDownLeftRight, Random
    }

    public PlayerType playerType;
    // Start is called before the first frame update
    void Start()
    {
        middleLine = localBoardManager.middleBoardEdge.transform.localPosition.z;
        leftLine = localBoardManager.leftEdge.transform.localPosition.z;
        rightLine = localBoardManager.rightEdge.transform.localPosition.z;
        upLine = localBoardManager.upEdge.transform.localPosition.x;
        downLine = localBoardManager.downEdge.transform.localPosition.x;

        goalLineDown_Green = localBoardManager.goalEdgeDown_Green.transform.localPosition.x;
        goalLineUp_Green = localBoardManager.goalEdgeUp_Green.transform.localPosition.x;
        goalLineDown_Red = localBoardManager.goalEdgeDown_Red.transform.localPosition.x;
        goalLineUp_Red = localBoardManager.goalEdgeUp_Red.transform.localPosition.x;

    
        
        flagGoal = false;
        flagMoved = false;
        cycle = localBoardManager.cycleTime;

        if (localBoardManager.whoHasBall.Equals(playerColor)) { hasBall = true; }else {hasBall = false;}
        if (hasBall) { ball.SetActive(true); }    else     { ball.SetActive(false); }
        playerPos = gameObject.transform.localPosition;
        
        if (gameObject.CompareTag("Green Player"))
        {
            opponent = GameObject.FindGameObjectWithTag("Red Player");
            opponentGoal = localBoardManager.redGoal;
            goal = localBoardManager.greenGoal;
        }
        else
        {
            opponent = GameObject.FindGameObjectWithTag("Green Player");
            opponentGoal = localBoardManager.greenGoal;
            goal = localBoardManager.redGoal;
        }
        
        
        InvokeRepeating("Move", 0.5f, 0.5f);

    }

    // Update is called once per frame
    void Update()
    {
        if (localBoardManager.whoHasBall.Equals(playerColor))
        {
            hasBall = true;
        }
        else
            hasBall = false;
        
        
        if (hasBall)
        {
            ball.SetActive(true);
        }
        else
        {
            ball.SetActive(false);
        }


        CheckInput();
    }
    
        void Move()
    {
        MovementReward();

        flagGoal = false;
        if(flagMoved)
        {
            switch (NWSE)
                {
                    case 0:
                                    if (NotOutOfBoard_NorthSide() && !flagGoal)
                                    {
                                            SetNewPosition(PlayerDirection.Up);

                                    }
                                    break;
                    case 1:
                                    CheckIfScore_Red(); //Because Red Goes from right to "left"
                                    if (NotOutOfBoard_WestSide() && !flagGoal)
                                    {
                                            SetNewPosition(PlayerDirection.Left);

                                    }
                                    break;
                    case 2:
                                    if (NotOutOfBoard_SouthSide() && !flagGoal)
                                    {
                                        SetNewPosition(PlayerDirection.Down);
                                    }

                                    break;
                    case 3:
                                    CheckIfScore_Green(); //Because Green Goes from left to "right"
                                    
                                    if (NotOutOfBoard_EastSide() && !flagGoal)
                                    {
                                            SetNewPosition(PlayerDirection.Right);

                                    }
                                    break;
                  
                }
            
            flagMoved = false;
        }
        else
        {
            SetNewPosition(PlayerDirection.Stay);
        }
    }

    private void MovementReward()
    {
        if (hasBall)
        {
            AddReward(-1f/3000f);

            if (this.CompareTag("Green Player"))
            {
               if (transform.localPosition.z < middleLine)
                {
                    AddReward(1f/3000f);
                }
                else
                {
                    AddReward(-1/3000f);
                }
            }
            else
            {
                if (transform.localPosition.z > middleLine)
                {
                    AddReward(1f/3000f);
                }
                else
                {
                    AddReward(-1f/3000f);
                }
            }
        }
        else
        {
            AddReward(-2f/3000f);

            if (this.CompareTag("Green Player"))
            {
                if (transform.localPosition.z > middleLine)
                {
                    AddReward(1f/3000f);
                }
                else
                {
                    AddReward(-1f/3000f);
                }
            }
            else
            {
                if (transform.localPosition.z < middleLine)
                {
                    AddReward(1f/3000f);
                }
                else
                {
                    AddReward(-1f/3000f);
                }
            }
        }
    }
    private void CheckIfScore_Green()
    {
        if (playerPos.z - blockPace < rightLine && playerPos.x < goalLineUp_Red && playerPos.x > goalLineDown_Red )
        {
            if (gameObject.CompareTag("Green Player") && hasBall)
            {
                
                flagGoal = true;
                localBoardManager.GreenScores();
            }
            
        }
    }
    private void CheckIfScore_Red()
    {
        if (playerPos.z + blockPace > leftLine && playerPos.x < goalLineUp_Green && playerPos.x > goalLineDown_Green)
        {
            if (gameObject.CompareTag("Red Player") && hasBall)
            {
             
                flagGoal = true;
                localBoardManager.RedScores();
                
            }
        }
    }
    
    
    private  bool TheyCollideInTheSameBlock()
    {
        if (localBoardManager.redPlayer.transform.localPosition ==
            localBoardManager.greenPlayer.transform.localPosition)
        {
            if (localBoardManager.SameDirection())
            {
                return false;
            }
            else
            {
               return true;
            }
        }
        else
        {
            return false;
        }
       
    }
    private bool NotOutOfBoard_EastSide()
    {
        return playerPos.z - blockPace >= rightLine;
    }

    private bool NotOutOfBoard_SouthSide()
    {
        return playerPos.x - blockPace >= downLine;
    }

    private bool NotOutOfBoard_WestSide()
    {
        return playerPos.z + blockPace <= leftLine;
    }

    private bool NotOutOfBoard_NorthSide()
    {
        return playerPos.x + blockPace <= upLine;
    }
    
    public override void AgentAction(float[] vectorAction, string textAction)
    {
        
        direction = (int) vectorAction[0];
        switch (direction)
        {
            case 0:   { NWSE = 0; flagMoved = true; break;}
            case 1:   { NWSE = 1; flagMoved = true; break;}
            case 2:   { NWSE = 2; flagMoved = true; break;}
            case 3:   { NWSE = 3; flagMoved = true; break;}
        }
    
    
    }

    
    
    public override void CollectObservations()
    {
        if (hasBall)
        {
            DistanceToGoal = ExtensionMethods.ConvertToVector2(transform.localPosition - opponentGoal.transform.localPosition).normalized;
            hasBall_int = 1;
        }
        else
        {
            DistanceToGoal = ExtensionMethods.ConvertToVector2(opponent.transform.localPosition - goal.transform.localPosition).normalized;
            hasBall_int = 0;
        }

        AddVectorObs(hasBall_int);
        AddVectorObs(DistanceToGoal);
        AddVectorObs(ExtensionMethods.ConvertToVector2(transform.localPosition - opponent.transform.localPosition).normalized);
   
 
    
    }
    
    public void SetNewPosition(Enum newDirection)
    {
        switch (newDirection)
        { 
            case PlayerDirection.Up: {       playerPos.x += blockPace;   _PlayerDirection = PlayerDirection.Up;            gameObject.transform.localPosition = playerPos; break; }
            case PlayerDirection.Down: {     playerPos.x -= blockPace;   _PlayerDirection = PlayerDirection.Down;                   gameObject.transform.localPosition = playerPos; break; }
            case PlayerDirection.Left: {     playerPos.z += blockPace;   _PlayerDirection = PlayerDirection.Left;                  gameObject.transform.localPosition = playerPos; break; }
            case PlayerDirection.Right: {    playerPos.z -= blockPace;   _PlayerDirection = PlayerDirection.Right;                  gameObject.transform.localPosition = playerPos; break; }
            case PlayerDirection.Stay: { _PlayerDirection = PlayerDirection.Stay; break; }
        }
    }


    void CheckInput()
    {
        if (playerType == PlayerType.WASD)
        
        {
            if (Input.GetKeyDown(KeyCode.W))           { NWSE = 0; flagMoved = true;}
            else if (Input.GetKeyDown(KeyCode.A))      { NWSE = 1; flagMoved = true; }
            else if (Input.GetKeyDown(KeyCode.S))      { NWSE = 2; flagMoved = true; }
            else if (Input.GetKeyDown(KeyCode.D)) { NWSE = 3; flagMoved = true; }
            

        }
        
        else if (playerType == PlayerType.UpDownLeftRight)
        
        {
            if      (Input.GetKeyDown(KeyCode.UpArrow))     { NWSE = 0; flagMoved = true;}
            else if (Input.GetKeyDown(KeyCode.LeftArrow))   { NWSE = 1; flagMoved = true;}
            else if (Input.GetKeyDown(KeyCode.DownArrow))   { NWSE = 2; flagMoved = true;}
            else if (Input.GetKeyDown(KeyCode.RightArrow))  { NWSE = 3; flagMoved = true;}
       
            
        }
        else if (playerType == PlayerType.Random)
        {
           NWSE =  UnityEngine.Random.Range(0, 4);
           flagMoved = true;
        }
    }
}
