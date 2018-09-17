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
        int currentColumnPosition = transform.GetComponent<Piece>().colPosition;
        for (int i = 0; i < Board.possibleMoveableChars.Length; i++)
        {
            int distance = Mathf.Abs((Board.possibleMoveableChars[i].rowPosition - currentRowPosition) +
                (Board.possibleMoveableChars[i].colPosition - currentColumnPosition));
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                targetRowPosition = Board.possibleMoveableChars[i].rowPosition;
                targetColumnPosition = Board.possibleMoveableChars[i].colPosition;
            }
        }
        //Attack
        if (shortestDistance == 1)
        {
            //Attack()
        }
        //Move
        else
        {
            //Move in the row
            if (Mathf.Abs(targetRowPosition - currentRowPosition) > Mathf.Abs(targetColumnPosition - currentColumnPosition))
            {

            }
            //Move in the column
            else
            {

            }
        }

    }
}
