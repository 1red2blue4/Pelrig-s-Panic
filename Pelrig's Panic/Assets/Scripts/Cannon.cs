using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour {
    
    [SerializeField] public int charges;
    [SerializeField] public Piece cannon;
    [SerializeField] public int cannonID;

    public void UseCannon(List<Piece> enemyList, int cannonRange)
    {
        if (charges > 0)
        {
            for (int i = 0; i < enemyList.Count; i++)
            {
                //first check if the enemy has not been spawned
                //then check if it's within range
                if ((enemyList[i].rowPosition != 0 || enemyList[i].colPosition != 0) 
                    && (cannon.rowPosition <= enemyList[i].rowPosition + cannonRange && cannon.rowPosition >= enemyList[i].rowPosition - cannonRange)
                    && (cannon.colPosition <= enemyList[i].colPosition + cannonRange && cannon.colPosition >= enemyList[i].colPosition - cannonRange)
                    )
                {
                    //if it is, kill the enemy
                    /*
                    enemyList[i].SetRowAndCol(0, 0);
                    GameObject foundEnemy = enemyList[i].thePiece;
                    foundEnemy.transform.position = new Vector3(10000, 10000, 0.0f);
                    enemyList[i].rowPosition = -5;
                    enemyList[i].colPosition = -5;
                    for (int j = i; j < Board.spawnedEnemies.Count - 2; j++)
                    {
                        Board.spawnedEnemies[j] = Board.spawnedEnemies[j + 1];
                    }
                    //in every situation where the number of coins increases or decreases, adjust the timeToWait
                    //Board.numberOfEnemies--;
                    */
                    enemyList.RemoveAt(i);
                    charges--;
                }
            }
        }
    }
}
