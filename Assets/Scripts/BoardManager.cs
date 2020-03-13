using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour
{
    
    
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

    private PlayerAgent greenPlayerScript;
    private PlayerAgent redPlayerScript;
    private float changeNum;

    public PlayerColor whoHasBall;

    [HideInInspector] public PlayerColor whoStartedLastTime;
    
    [HideInInspector] public float cycleTime;



   
    public enum PlayerColor
    {
        Red, Green
    }


    private void Awake()
    {
        whoStartedLastTime = whoHasBall;

    }

    // Start is called before the first frame update
    void Start()
    {
      
        cycleTime = Time.fixedDeltaTime * 2;

        greenPlayerScript = greenPlayer.GetComponent<PlayerAgent>();
        redPlayerScript = redPlayer.GetComponent<PlayerAgent>();
        
        initPos_greenPlayer = greenPlayer.transform.localPosition;
        initPos_redPlayer = redPlayer.transform.localPosition;
        
        InvokeRepeating("ZeroTheChangeBallOwnerNum", 0.5f, 0.6f);
    }

    // Update is called once per frame
    void Update()
    {
        if (greenPlayerScript.playerPos == redPlayerScript.playerPos)
        {
            ChangeBothPos();
            if(greenPlayerScript.hasBall)
            { greenPlayerScript.AddReward(-0.5f);}
            else
            {
                greenPlayerScript.AddReward(-0.5f);
            }
            
            if(redPlayerScript.hasBall)
            { redPlayerScript.AddReward(-0.5f);}
            else
            {
                redPlayerScript.AddReward(-0.5f);
            }
            
            ChangeBallOwner();
        }
        /*  if (TheyCollideInTheSameBlock())
  {
      if(hasBall) {AddReward(-0.5f);} else {AddReward(0.5f);}
      SetNewPosition(PlayerDirection.Left);
      localBoardManager.ChangeBallOwner();
  }
 */
        
        
    }

    private void ChangeBothPos()
    {
        if (greenPlayerScript._PlayerDirection == PlayerAgent.PlayerDirection.Down)
        {
            greenPlayerScript.SetNewPosition(PlayerAgent.PlayerDirection.Up);
        }
        else if (greenPlayerScript._PlayerDirection == PlayerAgent.PlayerDirection.Up)
        {
            greenPlayerScript.SetNewPosition(PlayerAgent.PlayerDirection.Down);
        }
        else if (greenPlayerScript._PlayerDirection == PlayerAgent.PlayerDirection.Left)
        {
            greenPlayerScript.SetNewPosition(PlayerAgent.PlayerDirection.Right);
        }
        else if (greenPlayerScript._PlayerDirection == PlayerAgent.PlayerDirection.Right)
        {
            greenPlayerScript.SetNewPosition(PlayerAgent.PlayerDirection.Left);
        }

        if (redPlayerScript._PlayerDirection == PlayerAgent.PlayerDirection.Down)
        {
            redPlayerScript.SetNewPosition(PlayerAgent.PlayerDirection.Up);
        }
        else if (redPlayerScript._PlayerDirection == PlayerAgent.PlayerDirection.Up)
        {
            redPlayerScript.SetNewPosition(PlayerAgent.PlayerDirection.Down);
        }
        else if (redPlayerScript._PlayerDirection == PlayerAgent.PlayerDirection.Left)
        {
            redPlayerScript.SetNewPosition(PlayerAgent.PlayerDirection.Right);
        }
        else if (redPlayerScript._PlayerDirection == PlayerAgent.PlayerDirection.Right)
        {
            redPlayerScript.SetNewPosition(PlayerAgent.PlayerDirection.Left);
        }
    }

    void ResetPlayerPosition()
    {
        greenPlayerScript.playerPos = initPos_greenPlayer;
        redPlayerScript.playerPos = initPos_redPlayer;
        greenPlayer.transform.localPosition = initPos_greenPlayer;
        redPlayer.transform.localPosition = initPos_redPlayer;
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
        if (whoStartedLastTime == PlayerColor.Green)
        {
            whoStartedLastTime = PlayerColor.Red;
            whoHasBall = PlayerColor.Red;
        }
        else
        {
            whoStartedLastTime = PlayerColor.Green;
            whoHasBall = PlayerColor.Green;
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
        
       
        UIManager.instance.RedScorePlus();
        redPlayerScript.SetReward(1f);
        greenPlayerScript.Done();
        redPlayerScript.Done();
        Reset();
    }

    public void GreenScores()
    {

        
        UIManager.instance.greenScorePlus();
        greenPlayerScript.SetReward(1f);
        greenPlayerScript.Done();
        redPlayerScript.Done();
        Reset();
    }

   public  bool SameDirection()
    {
        if (greenPlayerScript._PlayerDirection == redPlayerScript._PlayerDirection)
        {
            
            return true;
        }
        else
        {
            return false;
        }
    }
    
}
