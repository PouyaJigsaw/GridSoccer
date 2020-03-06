using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
public class PlayerAgent : Agent
{
    
    
    // Start is called before the first frame update
    void Start()
    {
        middleLine = -0.6f;
        flagGoal = false;
        flagMoved = false;
        cycleTime = BoardManager.instance.cycleTime;

        if (.instance.whoHasBall.Equals(playerColor)) { hasBall = true; }else {hasBall = false;}
        if (hasBall) { ball.SetActive(true); }    else     { ball.SetActive(false); }
        playerPos = gameObject.transform.position;
        if (gameObject.CompareTag("Green Player"))
        {
            opponent = GameObject.FindGameObjectWithTag("Red Player");
            opponentGoal = GameManager.instance.leftGoal;
            goal = GameManager.instance.rightGoal;
        }
        else
        {
            opponent = GameObject.FindGameObjectWithTag("Green Player");
            opponentGoal = GameManager.instance.rightGoal;
            goal = GameManager.instance.leftGoal;
        }
        
        
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
                                                    if(hasBall) {AddReward(-0.5f);} else {AddReward(0.5f);}
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
                                                    if(hasBall) {AddReward(-0.5f);} else {AddReward(0.5f);}
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
                                                    if(hasBall) {AddReward(-0.5f);} else {AddReward(0.5f);}
                                                    SetNewPosition(PlayerDirection.Left);
                                                    GameManager.instance.ChangeBallOwner();
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
                if (transform.position.z < middleLine)
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
                if (transform.position.z > middleLine)
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
                if (transform.position.z > middleLine)
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
                if (transform.position.z < middleLine)
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
        if (playerPos.z - 1.1f < -7 && playerPos.x < 0.6f && playerPos.x > -1.8f )
        {
            if (gameObject.CompareTag("Green Player") && hasBall)
            {
                
                flagGoal = true;
                GameManager.instance.GreenScores();
            }
            
        }
    }
    private void CheckIfScore_Red()
    {
        if (playerPos.z + 1.1f > 5.7f && playerPos.x < 0.6f && playerPos.x > -1.8f)
        {
            if (gameObject.CompareTag("Red Player") && hasBall)
            {
               
                flagGoal = true;
                GameManager.instance.RedScores();
                
            }
        }
    }
    
    
    private static bool TheyCollideInTheSameBlock()
    {
        //BOARDMANAGER
        return GameManager.instance.players[0].transform.position == GameManager.instance.players[1].transform.position;
    }
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
            DistanceToGoal = ExtensionMethods.ConvertToVector2(transform.position - opponentGoal.transform.position);
        }
        else
        {
            DistanceToGoal = ExtensionMethods.ConvertToVector2(transform.position - goal.transform.position);
        }


        AddVectorObs(DistanceToGoal);
        AddVectorObs(ExtensionMethods.ConvertToVector2(transform.position - opponent.transform.position));
   
 
    
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


}
