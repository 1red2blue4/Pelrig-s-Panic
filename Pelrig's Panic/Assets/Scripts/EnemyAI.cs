using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {

    float time;
	// Use this for initialization
	void Start () {
        time = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;
		if (time > 3.0f)
        {
            time = 0.0f;
            MoveAndCheckUnitCollision();
            //For now, heuristic movement to closest unit
            CheckCoinDestroy();
        }
    }

    //To verify
    private void CheckCoinDestroy()
    {
        for (int i = 0; i < Board.currentNumCoins; i++)
        {
            //check for a hit
            if (transform.GetComponent<Piece>().rowPosition == Board.allCoins[i].rowPosition && 
                transform.GetComponent<Piece>().colPosition == Board.allCoins[i].colPosition)
            {
                Board.allCoins[i].GetPiece().transform.position = new Vector3(10000, 10000, 0.0f);
                Board.allCoins[i].rowPosition = -5;
                Board.allCoins[i].colPosition = -5;
                for (int j = i; j < Board.allCoins.Length - 2; j++)
                {
                    Board.allCoins[j] = Board.allCoins[j + 1];
                }
                //in every situation where the number of coins increases or decreases, adjust the timeToWait
                Board.currentNumCoins--;
                Board.timeToWait /= Board.approxGoldenRatio;
            }
        }
    }

    private void MoveAndCheckUnitCollision()
    {
        int shortestDistance = 10000;
        int targetRowPosition = 0;
        int currentRowPosition = transform.GetComponent<Piece>().rowPosition;
        int targetColumnPosition = 0;
        int hi = 0;
        int currentColumnPosition = transform.GetComponent<Piece>().colPosition;

        //See if they in play
        if (currentColumnPosition != 10000 && currentRowPosition != 10000)
        {
            for (int i = 0; i < Board.possibleMoveableChars.Length; i++)
            {
                int distance = Mathf.Abs(Board.possibleMoveableChars[i].rowPosition - currentRowPosition) +
                    Mathf.Abs(Board.possibleMoveableChars[i].colPosition - currentColumnPosition);
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    targetRowPosition = Board.possibleMoveableChars[i].rowPosition;
                    targetColumnPosition = Board.possibleMoveableChars[i].colPosition;
                    hi = i;
                }
            }
            //Attack
            if (shortestDistance == 1)
            {
                //Attack()
            }
            //Move. Terrible system, must find a better way for later
            else
            {
                bool canMoveLeft = true;
                bool canMoveRight = true;
                bool canMoveUp = true;
                bool canMoveDown = true;
                bool didMove = false;
                //check for dead spaces in the way
                for (int i = 0; i < Board.numDeadSpaces; i++)
                {
                    if (Board.deadPoints[i].x == currentColumnPosition + 1 && Board.deadPoints[i].y == currentRowPosition)
                    {
                        canMoveUp = false;
                    }
                    if (Board.deadPoints[i].x == currentColumnPosition && Board.deadPoints[i].y == currentRowPosition + 1)
                    {
                        canMoveRight = false;
                    }
                    if (Board.deadPoints[i].x == currentColumnPosition - 1 && Board.deadPoints[i].y == currentRowPosition)
                    {
                        canMoveDown = false;
                    }
                    if (Board.deadPoints[i].x == currentColumnPosition && Board.deadPoints[i].y == currentRowPosition - 1)
                    {
                        canMoveLeft = false;
                    }
                }


                //Move in the row
                if (Mathf.Abs(targetRowPosition - currentRowPosition) > Mathf.Abs(targetColumnPosition - currentColumnPosition))
                {
                    //Move right
                    if (targetRowPosition > currentRowPosition)
                    {
                        if (canMoveRight)
                        {
                            didMove = true;
                            transform.GetComponent<Piece>().transform.position += new Vector3(0.0f, Board.pieceDistance, 0.0f);
                            transform.GetComponent<Piece>().SetRowAndCol(currentRowPosition + 1, currentColumnPosition);
                        }
                    }
                    else
                    {
                        if (canMoveLeft)
                        {
                            didMove = true;
                            transform.GetComponent<Piece>().transform.position += new Vector3(0.0f, -Board.pieceDistance, 0.0f);
                            transform.GetComponent<Piece>().SetRowAndCol(currentRowPosition - 1, currentColumnPosition);
                        }
                    }
                }
                //Move in the column
                else
                {
                    //Move right
                    if (targetColumnPosition > currentColumnPosition)
                    {
                        if (canMoveUp)
                        {
                            didMove = true;
                            transform.GetComponent<Piece>().transform.position += new Vector3(Board.pieceDistance, 0.0f, 0.0f);
                            transform.GetComponent<Piece>().SetRowAndCol(currentRowPosition, currentColumnPosition + 1);
                        }
                    }
                    else
                    {
                        if (canMoveDown)
                        {
                            didMove = true;
                            transform.GetComponent<Piece>().transform.position += new Vector3(-Board.pieceDistance, 0.0f, 0.0f);
                            transform.GetComponent<Piece>().SetRowAndCol(currentRowPosition, currentColumnPosition - 1);
                        }
                    }
                }
                if (!didMove)
                {
                    if (canMoveRight)
                    {
                        didMove = true;
                        transform.position += new Vector3(Board.pieceDistance, 0.0f, 0.0f);
                        transform.GetComponent<Piece>().SetRowAndCol(currentRowPosition + 1, currentColumnPosition);
                    }
                    else if (canMoveLeft)
                    {
                        didMove = true;
                        transform.position += new Vector3(-Board.pieceDistance, 0.0f, 0.0f);
                        transform.GetComponent<Piece>().SetRowAndCol(currentRowPosition - 1, currentColumnPosition);
                    }
                    else if (canMoveUp)
                    {
                        didMove = true;
                        transform.position += new Vector3(0.0f, Board.pieceDistance, 0.0f);
                        transform.GetComponent<Piece>().SetRowAndCol(currentRowPosition, currentColumnPosition + 1);
                    }
                    else if (canMoveDown)
                    {
                        didMove = true;
                        transform.position += new Vector3(0.0f, -Board.pieceDistance, 0.0f);
                        transform.GetComponent<Piece>().SetRowAndCol(currentRowPosition, currentColumnPosition - 1);
                    }
                }
            }
        }

    }
}
