using UnityEngine;

public partial class GameManager //GameBoardInsantiator
{
    public GameObject rightGoal;
    public GameObject leftGoal;
    //left goal and right goal positions
    private Vector3 rightGoalPos; // x = -0.6      z = -7.1
    private Vector3 leftGoalPos; // x = -0.6      z = 5.9
    //game block prefab
    public GameObject block;
    //length of the edge of the block
    private float length;
    public GameObject edge;

    [SerializeField] private float numOfGameBoards;
    //inital Pos of red player
    
    //initial pos of green player
    
    //green and red player seriazable game objects (prefab)
    private void InstantiateGameBoard()
    {
        
        GameObject edgeSample = Instantiate(edge, Vector3.zero, Quaternion.identity);
        length = edgeSample.GetComponent<BoxCollider>().bounds.size.x;
        Destroy(edgeSample);
        
        goals[0] = Instantiate(leftGoal, leftGoalPos, Quaternion.identity); //Left Goal
        goals[1] = Instantiate(rightGoal, rightGoalPos, Quaternion.identity); // Right Goal

        for (int i = -3, ii = 0; i <= 3 && ii < 6; i++, ii++)
        {
            for (int j = -5, jj = 0; j <= 5 && jj < 10; j++, jj++)
            {
                Vector3 gbPos = new Vector3(i * length, 0, j * length);
                gameBoard[ii, jj] = Instantiate(block, gbPos, Quaternion.identity);
                playerBoard[ii, jj] = gbPos + new Vector3(0, 0.6f, 0);
            }
        }


        redPlayerPosInit = playerBoard[2, 0];
        greenPlayerPosInit = playerBoard[2, 9];


        players[0] = Instantiate(redPlayer, redPlayerPosInit, Quaternion.identity);
        players[1] = Instantiate(greenPlayer, greenPlayerPosInit, Quaternion.identity);
    }
}