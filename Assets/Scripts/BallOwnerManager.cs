using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameManager
{
    [HideInInspector] public PlayerColor whoHasBall;

    
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

    private void ChangeBallOwnerForEveryScore()
    {
        if (WhoScoredLastTime == PlayerColor.Green)
        {
            whoHasBall = PlayerColor.Green;
        }
        else
        {
            whoHasBall = PlayerColor.Red;
        }
    }

    private void ZeroTheChangeBallOwnerNum()
    {
        if (changeNum > 0)
        {
            changeNum = 0;
        }
    }
}

