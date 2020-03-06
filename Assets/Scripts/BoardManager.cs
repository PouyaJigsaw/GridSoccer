using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{

    public GameObject greenPlayer;

    public GameObject redPlayer;

    public GameObject redGoal;

    public GameObject greenGoal;


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
    
    private float cycleTime;
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
    
}
