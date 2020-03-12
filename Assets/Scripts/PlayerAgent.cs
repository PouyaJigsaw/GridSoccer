using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using UnityEngine.Experimental.PlayerLoop;

public class PlayerAgent : Agent
{
    private float cycle;
    private bool hasBall;
    public Vector3 playerPos;
    public GameObject ball;


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
    
    public enum PlayerDirection
    {
        Up,Down,Left,Right
    }
    
    public enum PlayerType
    {
        WASD, UpDownLeftRight
    }

    public PlayerType playerType;
    // Start is called before the first frame update
    void Start()
    {
        middleLine = BoardManager.instance.middleBoardEdge.transform.localPosition.z;
        leftLine = BoardManager.instance.leftEdge.transform.localPosition.z;
        rightLine = BoardManager.instance.rightEdge.transform.localPosition.z;
        upLine = BoardManager.instance.upEdge.transform.localPosition.x;
        downLine = BoardManager.instance.downEdge.transform.localPosition.x;

        goalLineDown_Green = BoardManager.instance.goalEdgeDown_Green.transform.localPosition.x;
        goalLineUp_Green = BoardManager.instance.goalEdgeUp_Green.transform.localPosition.x;
        goalLineDown_Red = BoardManager.instance.goalEdgeDown_Red.transform.localPosition.x;
        goalLineUp_Red = BoardManager.instance.goalEdgeUp_Red.transform.localPosition.x;

    
        
        flagGoal = false;
        flagMoved = false;
        cycle = BoardManager.instance.cycleTime;

        if (BoardManager.instance.whoHasBall.Equals(playerColor)) { hasBall = true; }else {hasBall = false;}
        if (hasBall) { ball.SetActive(true); }    else     { ball.SetActive(false); }
        playerPos = gameObject.transform.localPosition;
        
        if (gameObject.CompareTag("Green Player"))
        {
            opponent = GameObject.FindGameObjectWithTag("Red Player");
            opponentGoal = BoardManager.instance.redGoal;
            goal = BoardManager.instance.greenGoal;
        }
        else
        {
            opponent = GameObject.FindGameObjectWithTag("Green Player");
            opponentGoal = BoardManager.instance.greenGoal;
            goal = BoardManager.instance.redGoal;
        }
        
        
        InvokeRepeating("Move", 0.5f, 0.5f);

    }

    // Update is called once per frame
    void Update()
    {
        if (BoardManager.instance.whoHasBall.Equals(playerColor))
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
                                                if (TheyCollideInTheSameBlock())
                                                {
                                                    if(hasBall) {AddReward(-0.5f);} else {AddReward(0.5f);}
                                                    SetNewPosition(PlayerDirection.Down);
                                                    BoardManager.instance.ChangeBallOwner();
                                                }
                                    }
                                    break;
                    case 1:
                                    CheckIfScore_Red(); //Because Red Goes from right to "left"
                                    if (NotOutOfBoard_WestSide() && !flagGoal)
                                    {
                                            SetNewPosition(PlayerDirection.Left);
                                                if (TheyCollideInTheSameBlock())
                                                {
                                                    if(hasBall) {AddReward(-0.5f);} else {AddReward(0.5f);}
                                                    SetNewPosition(PlayerDirection.Right);
                                                    BoardManager.instance.ChangeBallOwner();
                                                }
                                    }
                                    break;
                    case 2:
                                    if (NotOutOfBoard_SouthSide() && !flagGoal)
                                    {
                                            SetNewPosition(PlayerDirection.Down);
                                                if (TheyCollideInTheSameBlock())
                                                {
                                                    if(hasBall) {AddReward(-0.5f);} else {AddReward(0.5f);}
                                                    SetNewPosition(PlayerDirection.Up);
                                                    BoardManager.instance.ChangeBallOwner();
                                                }
                                    }
                                    break;
                    case 3:
                                    CheckIfScore_Green(); //Because Green Goes from left to "right"
                                    
                                    if (NotOutOfBoard_EastSide() && !flagGoal)
                                    {
                                            SetNewPosition(PlayerDirection.Right);
                                                if (TheyCollideInTheSameBlock())
                                                {
                                                    if(hasBall) {AddReward(-0.5f);} else {AddReward(0.5f);}
                                                    SetNewPosition(PlayerDirection.Left);
                                                    BoardManager.instance.ChangeBallOwner();
                                                }
                                    }
                                    break;
                }
            flagMoved = false;
        }
    }

    private void MovementReward()
    {
        if (hasBall)
        {
            AddReward(-0.01f);

            if (this.CompareTag("Green Player"))
            {
               if (transform.localPosition.z < middleLine)
                {
                    AddReward(0.01f);
                }
                else
                {
                    AddReward(-0.01f);
                }
            }
            else
            {
                if (transform.localPosition.z > middleLine)
                {
                    AddReward(0.01f);
                }
                else
                {
                    AddReward(-0.01f);
                }
            }
        }
        else
        {
            AddReward(-0.02f);

            if (this.CompareTag("Green Player"))
            {
                if (transform.localPosition.z > middleLine)
                {
                    AddReward(0.01f);
                }
                else
                {
                    AddReward(-0.01f);
                }
            }
            else
            {
                if (transform.localPosition.z < middleLine)
                {
                    AddReward(0.01f);
                }
                else
                {
                    AddReward(-0.01f);
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
                BoardManager.instance.GreenScores();
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
                BoardManager.instance.RedScores();
                
            }
        }
    }
    
    
    private static bool TheyCollideInTheSameBlock()
    {
        if (BoardManager.instance.greenPlayer.transform.localPosition == BoardManager.instance.redPlayer.transform.localPosition)
        {
            
        }
        return BoardManager.instance.redPlayer.transform.localPosition == BoardManager.instance.greenPlayer.transform.localPosition;
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
        }
        else
        {
            DistanceToGoal = ExtensionMethods.ConvertToVector2(transform.localPosition - goal.transform.localPosition).normalized;
        }


        AddVectorObs(DistanceToGoal);
        AddVectorObs(ExtensionMethods.ConvertToVector2(transform.localPosition - opponent.transform.localPosition).normalized);
   
 
    
    }
    
    public void SetNewPosition(Enum newDirection)
    {
        switch (newDirection)
        { 
            case PlayerDirection.Up: {       playerPos.x += blockPace;                     gameObject.transform.localPosition = playerPos; break; }
            case PlayerDirection.Down: {     playerPos.x -= blockPace;                     gameObject.transform.localPosition = playerPos; break; }
            case PlayerDirection.Left: {     playerPos.z += blockPace;                     gameObject.transform.localPosition = playerPos; break; }
            case PlayerDirection.Right: {    playerPos.z -= blockPace;                     gameObject.transform.localPosition = playerPos; break; }
        }
    }


    void CheckInput()
    {
        if (playerType == PlayerType.WASD)
        
        {
            if (Input.GetKeyDown(KeyCode.W))           { NWSE = 0; flagMoved = true;}
            else if (Input.GetKeyDown(KeyCode.A))      { NWSE = 1; flagMoved = true; }
            else if (Input.GetKeyDown(KeyCode.S))      { NWSE = 2; flagMoved = true; }
            else if (Input.GetKeyDown(KeyCode.D))      { NWSE = 3; flagMoved = true; }
        }
        
        else if (playerType == PlayerType.UpDownLeftRight)
        
        {
            if      (Input.GetKeyDown(KeyCode.UpArrow))     { NWSE = 0; flagMoved = true;}
            else if (Input.GetKeyDown(KeyCode.LeftArrow))   { NWSE = 1; flagMoved = true;}
            else if (Input.GetKeyDown(KeyCode.DownArrow))   { NWSE = 2; flagMoved = true;}
            else if (Input.GetKeyDown(KeyCode.RightArrow))  { NWSE = 3; flagMoved = true;}
        }
    }
}
