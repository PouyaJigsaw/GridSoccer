
using System;
using MLAgents;
using UnityEngine;

public class Player : Agent
{
    public enum PlayerType
    {
        WASD, UpDownLeftRight, HeuristicAi, ReinforcementLearningAi
    }

    public enum PlayerDirection
    {
        Up,Down,Left,Right
    }
    public PlayerType playerType;
    public Vector3 playerPos;

    [SerializeField]
    float blockPace = 1.2f;
    public bool hasBall;
    public GameObject ball;


    private GameObject opponent;
    private int NWSE;
    private float cycleTime;

    private bool flagMoved;

    private Vector2 thisPosition;
    private Vector2 opponentPosition;
    private bool flagGoal;
    public GameManager.PlayerColor playerColor;

    private int direction;
    // Start is called before the first frame update
    
    void Start()
    {
        flagGoal = false;
        flagMoved = false;
        cycleTime = GameManager.instance.cycleTime;

        if (GameManager.instance.whoHasBall.Equals(playerColor)) { hasBall = true; }else {hasBall = false;}
        if (hasBall) { ball.SetActive(true); }    else     { ball.SetActive(false); }
        playerPos = gameObject.transform.position;
        if (this.gameObject.CompareTag("Green Player")) { opponent = GameObject.FindGameObjectWithTag("Red Player"); }      else      { opponent = GameObject.FindGameObjectWithTag("Green Player"); }
        
        
        InvokeRepeating("Move", 0.5f, cycleTime);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.whoHasBall.Equals(playerColor))
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
        flagGoal = false;
        if(flagMoved)
        {
            SetReward(-0.01f);
            switch (NWSE)
                {
                    case 0:
                                    if (NotOutOfBoard_NorthSide() && !flagGoal)
                                    {
                                            SetNewPosition(PlayerDirection.Up);
                                                if (TheyCollideInTheSameBlock())
                                                {
                                                    SetNewPosition(PlayerDirection.Down);
                                                    GameManager.instance.ChangeBallOwner();
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
                                                    SetNewPosition(PlayerDirection.Right);
                                                    GameManager.instance.ChangeBallOwner();
                                                }
                                    }
                                    break;
                    case 2:
                                    if (NotOutOfBoard_SouthSide() && !flagGoal)
                                    {
                                            SetNewPosition(PlayerDirection.Down);
                                                if (TheyCollideInTheSameBlock())
                                                {
                                                    SetNewPosition(PlayerDirection.Up);
                                                    GameManager.instance.ChangeBallOwner();
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
                                                    if(hasBall) {SetReward(-0.5f);} else {SetReward(0.5f);}
                                                    SetNewPosition(PlayerDirection.Left);
                                                    GameManager.instance.ChangeBallOwner();
                                                }
                                    }
                                    break;
                }
            flagMoved = false;
        }
    }
    #region Check if Player Score
    private void CheckIfScore_Green()
    {
        if (playerPos.z - 1.1f < -7)
        {
            if (gameObject.CompareTag("Green Player") && hasBall)
            {
                SetReward(1f);
                flagGoal = true;
                GameManager.instance.GreenScores();
            }
        }
    }
    private void CheckIfScore_Red()
    {
        if (playerPos.z + 1.1f > 5.7f)
        {
            if (gameObject.CompareTag("Red Player") && hasBall)
            {
                SetReward(1f);
                flagGoal = true;
                GameManager.instance.RedScores();
                
            }
        }
    }
    #endregion
    private static bool TheyCollideInTheSameBlock()
    {
        
        return GameManager.instance.players[0].transform.position == GameManager.instance.players[1].transform.position;
    }
    #region Not out of board
    private bool NotOutOfBoard_EastSide()
    {
        return playerPos.z - 1.2f >= -6.1f;
    }

    private bool NotOutOfBoard_SouthSide()
    {
        return playerPos.x - 1.2f >= -3.7f;
    }

    private bool NotOutOfBoard_WestSide()
    {
        return playerPos.z + 1.2f <= 4.8f;
    }

    private bool NotOutOfBoard_NorthSide()
    {
        return playerPos.x + 1.2f <= 2.4f;
    }
#endregion
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
void SetNewPosition(Enum newDirection)
{
    switch (newDirection)
    { 
        case PlayerDirection.Up: {       playerPos.x += blockPace;                     gameObject.transform.position = playerPos; break; }
        case PlayerDirection.Down: {     playerPos.x -= blockPace;                     gameObject.transform.position = playerPos; break; }
        case PlayerDirection.Left: {     playerPos.z += blockPace;                     gameObject.transform.position = playerPos; break; }
        case PlayerDirection.Right: {    playerPos.z -= blockPace;                     gameObject.transform.position = playerPos; break; }
    }
}


public override void CollectObservations()
{

    
    AddVectorObs(transform.position);
    AddVectorObs(opponent.transform.position);
    AddVectorObs(GameManager.instance.leftGoal.transform.position);
    AddVectorObs(GameManager.instance.rightGoal.transform.position);
    AddVectorObs(hasBall);
    
}


public override void AgentAction(float[] vectorAction, string textAction)
{
    direction = (int) vectorAction[0];
    Debug.Log(direction);
    switch (direction)
    {
        case 0:   { NWSE = 0; flagMoved = true; break;}
        case 1:   { NWSE = 1; flagMoved = true; break;}
        case 2:   { NWSE = 2; flagMoved = true; break;}
        case 3:   { NWSE = 3; flagMoved = true; break;}
        
    }
    
    
}
}
