using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateCaptainAI : MonoBehaviour {

    float time;
    int countMove = 0;
    public bool isTurnActive;
    [SerializeField] private GameObject presenceObj;
    [SerializeField] private GameObject resistanceObj;
    bool isEncumbered;
    bool cursorChanged = false;
    public Stats stats;
    public Texture2D mouseTarget;

    // Use this for initialization
    void Start()
    {
        stats = GetComponent<Stats>();
        isEncumbered = false;
        isTurnActive = false;
        time = 0.0f;
        presenceObj.GetComponent<MeshRenderer>().sortingOrder = 3;
        resistanceObj.GetComponent<MeshRenderer>().sortingOrder = 3;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.GetComponent<Piece>().rowPosition != 1000 && !PlayerControls.isPlayerTurn)
        {
            if (isTurnActive)
            {
                time += Time.deltaTime;
                if (time >= 1.5f)
                {
                    time = 0.0f;
                    MoveAndCheckUnitCollision(false);
                    countMove++;
                    if (countMove >= 5)
                    {
                        MoveAndCheckUnitCollision(true);
                        isTurnActive = false;
                        countMove = 0;
                        time = 0.0f;
                    }
                }
                //For now, heuristic movement to closest unit
                //CheckCoinDestroy();

            }
        }
        else
        {
            if (stats.health <= 0)
            {
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                Destroy(gameObject);
            }
        }

        CheckPlayer();
    }

    void CheckPlayer()
    {
        int playersAround = 0;
        for (int i = 0; i < Board.possibleMoveableChars.Length; i++)
        {
            if (Board.possibleMoveableChars[i].rowPosition == 1000)
                continue;
            bool a = false;
            bool b = false;
            if (transform.GetComponent<Piece>().rowPosition == Board.possibleMoveableChars[i].rowPosition - 1 ||
                transform.GetComponent<Piece>().rowPosition == Board.possibleMoveableChars[i].rowPosition + 1 ||
                transform.GetComponent<Piece>().rowPosition == Board.possibleMoveableChars[i].rowPosition)
            {
                a = true;
            }
            if (transform.GetComponent<Piece>().colPosition == Board.possibleMoveableChars[i].colPosition - 1 ||
                transform.GetComponent<Piece>().colPosition == Board.possibleMoveableChars[i].colPosition + 1 ||
                transform.GetComponent<Piece>().colPosition == Board.possibleMoveableChars[i].colPosition)
            {
                b = true;
            }
            if (a && b)
            {
                playersAround += 1;
            }
        }

        if (!isEncumbered && playersAround >= 4)
        {
            stats.damage /= 2;
            stats.health /= 3;
            isEncumbered = true;
        }

        UIValues resistance = resistanceObj.GetComponent<UIValues>();
        resistance.SetValue(stats.health);

        UIValues attack = presenceObj.GetComponent<UIValues>();
        attack.SetValue(stats.damage);
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

    private void OnMouseEnter()
    {
        if (!cursorChanged && !CannonCrossbarController.isCannonSelected && CheckIfAPlayerAround())
        {
            cursorChanged = true;
            Cursor.SetCursor(mouseTarget, Vector2.zero, CursorMode.Auto);
        }
    }

    private void OnMouseExit()
    {
        if (cursorChanged)
        {
            cursorChanged = false;
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }

    }

    bool CheckIfAPlayerAround()
    {
        if (PlayerControls.selectedUnit)
        {
            if (PlayerControls.selectedUnit.GetComponent<Stats>().canAttack)
            {
                Piece unit = Board.possibleMoveableChars[PlayerControls.theOne];
                bool a = false;
                bool b = false;
                if (transform.GetComponent<Piece>().rowPosition == unit.rowPosition - 1 ||
                     transform.GetComponent<Piece>().rowPosition == unit.rowPosition + 1 ||
                     transform.GetComponent<Piece>().rowPosition == unit.rowPosition)
                {
                    a = true;
                }
                if (transform.GetComponent<Piece>().colPosition == unit.colPosition - 1 ||
                    transform.GetComponent<Piece>().colPosition == unit.colPosition + 1 ||
                    transform.GetComponent<Piece>().colPosition == unit.colPosition)
                {
                    b = true;
                }
                if (a && b)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void MoveAndCheckUnitCollision(bool onlyAttack)
    {
        int random = (int)Random.Range(0.0f, 1.99f);
        int shortestDistance = 15;
        int targetRowPosition = 0;

        int targetColumnPosition = 0;
        int currentRowPosition = transform.GetComponent<Piece>().rowPosition;
        int currentColumnPosition = transform.GetComponent<Piece>().colPosition;
        int targetPlayer = 0;
        
        for (int i = 0; i < Board.possibleMoveableChars.Length; i++)
        {
            int distance = Mathf.Abs(Board.possibleMoveableChars[i].rowPosition - currentRowPosition) +
                Mathf.Abs(Board.possibleMoveableChars[i].colPosition - currentColumnPosition);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                targetRowPosition = Board.possibleMoveableChars[i].rowPosition;
                targetColumnPosition = Board.possibleMoveableChars[i].colPosition;
                targetPlayer = i;
            }
        }
        //Attack
        if (shortestDistance >= 15)
        {
            targetRowPosition = 8;
            targetColumnPosition = 37;
            if (targetRowPosition == currentRowPosition &&
                targetColumnPosition == currentColumnPosition)
            {
                countMove = 0;
                isTurnActive = false;
                return;
            }

        }
        if (shortestDistance == 1)
        {
            if (stats.canAttack)
            {
                Board.possibleMoveableChars[targetPlayer].thePiece.GetComponent<Stats>().TakeDamage(stats.damage);
                stats.canAttack = false;
            }
        }
        //Move. Terrible system, must find a better way for later
        else if ( !onlyAttack)
        {
            bool canMoveLeft = true;
            bool canMoveRight = true;
            bool canMoveUp = true;
            bool canMoveDown = true;
            bool didMove = false;
            //check for dead spaces and other enemies in the way
            for (int i = 0; i < Board.numDeadSpaces; i++)
            {
                if (Board.deadPoints[i].x == currentColumnPosition && Board.deadPoints[i].y == currentRowPosition - 1)
                {
                    canMoveUp = false;
                }
                if (Board.deadPoints[i].x == currentColumnPosition + 1 && Board.deadPoints[i].y == currentRowPosition)
                {
                    canMoveRight = false;
                }
                if (Board.deadPoints[i].x == currentColumnPosition && Board.deadPoints[i].y == currentRowPosition + 1)
                {
                    canMoveDown = false;
                }
                if (Board.deadPoints[i].x == currentColumnPosition - 1 && Board.deadPoints[i].y == currentRowPosition)
                {
                    canMoveLeft = false;
                }
            }

            for (int i = 0; i < Board.allCannons.Length; i++)
            {
                if (Board.allCannons[i].cannon.colPosition == currentColumnPosition && Board.allCannons[i].cannon.rowPosition == currentRowPosition - 1)
                {
                    canMoveUp = false;
                }
                if (Board.allCannons[i].cannon.colPosition == currentColumnPosition + 1 && Board.allCannons[i].cannon.rowPosition == currentRowPosition)
                {
                    canMoveRight = false;
                }
                if (Board.allCannons[i].cannon.colPosition == currentColumnPosition && Board.allCannons[i].cannon.rowPosition == currentRowPosition + 1)
                {
                    canMoveDown = false;
                }
                if (Board.allCannons[i].cannon.colPosition == currentColumnPosition - 1 && Board.allCannons[i].cannon.rowPosition == currentRowPosition)
                {
                    canMoveLeft = false;
                }
            }

            for (int i = 0; i < Board.spawnedEnemies.Count; i++)
            {
                if (Board.spawnedEnemies[i].colPosition == currentColumnPosition && Board.spawnedEnemies[i].rowPosition == currentRowPosition - 1)
                {
                    canMoveUp = false;
                }
                if (Board.spawnedEnemies[i].colPosition == currentColumnPosition + 1 && Board.spawnedEnemies[i].rowPosition == currentRowPosition)
                {
                    canMoveRight = false;
                }
                if (Board.spawnedEnemies[i].colPosition == currentColumnPosition && Board.spawnedEnemies[i].rowPosition == currentRowPosition + 1)
                {
                    canMoveDown = false;
                }
                if (Board.spawnedEnemies[i].colPosition == currentColumnPosition - 1 && Board.spawnedEnemies[i].rowPosition == currentRowPosition)
                {
                    canMoveLeft = false;
                }
            }

            //Move in the row
            if (Mathf.Abs(targetRowPosition - currentRowPosition) < Mathf.Abs(targetColumnPosition - currentColumnPosition))
            {
                //Move right
                if (targetColumnPosition > currentColumnPosition)
                {
                    if (canMoveRight)
                    {
                        didMove = true;
                        transform.GetComponent<Piece>().transform.position = GameObject.Find("gridRow" + (currentRowPosition) + "Column" + (currentColumnPosition + 1)).transform.position;
                        transform.GetComponent<Piece>().SetRowAndCol(currentRowPosition, currentColumnPosition + 1);
                    }
                }
                else
                {
                    if (canMoveLeft)
                    {
                        didMove = true;
                        transform.GetComponent<Piece>().transform.position = GameObject.Find("gridRow" + (currentRowPosition) + "Column" + (currentColumnPosition - 1)).transform.position;
                        transform.GetComponent<Piece>().SetRowAndCol(currentRowPosition, currentColumnPosition - 1);
                    }
                }
            }
            //Move in the column
            else
            {
                //Move right
                if (targetRowPosition < currentRowPosition)
                {
                    if (canMoveUp)
                    {
                        didMove = true;
                        transform.GetComponent<Piece>().SetRowAndCol(currentRowPosition - 1, currentColumnPosition);
                        transform.GetComponent<Piece>().transform.position = GameObject.Find("gridRow" + (currentRowPosition - 1) + "Column" + (currentColumnPosition)).transform.position;
                    }
                }
                else
                {
                    if (canMoveDown)
                    {
                        didMove = true;
                        transform.GetComponent<Piece>().SetRowAndCol(currentRowPosition + 1, currentColumnPosition);
                        transform.GetComponent<Piece>().transform.position = GameObject.Find("gridRow" + (currentRowPosition + 1) + "Column" + (currentColumnPosition)).transform.position;
                    }
                }
            }
            while (!didMove)
            {

                int randd = (int)Random.Range(0.0f, 3.99f);
                if (randd == 0 && canMoveRight)
                {
                    didMove = true;
                    transform.GetComponent<Piece>().SetRowAndCol(currentRowPosition, currentColumnPosition + 1);
                    transform.GetComponent<Piece>().transform.position = GameObject.Find("gridRow" + (currentRowPosition) + "Column" + (currentColumnPosition + 1)).transform.position;
                }
                else if (randd == 1 && canMoveLeft)
                {
                    didMove = true;
                    transform.GetComponent<Piece>().SetRowAndCol(currentRowPosition, currentColumnPosition - 1);
                    transform.GetComponent<Piece>().transform.position = GameObject.Find("gridRow" + (currentRowPosition) + "Column" + (currentColumnPosition - 1)).transform.position;
                }
                else if (randd == 2 && canMoveUp)
                {
                    didMove = true;
                    transform.GetComponent<Piece>().SetRowAndCol(currentRowPosition - 1, currentColumnPosition);
                    transform.GetComponent<Piece>().transform.position = GameObject.Find("gridRow" + (currentRowPosition - 1) + "Column" + (currentColumnPosition)).transform.position;
                }
                else if (randd == 3 && canMoveDown)
                {
                    didMove = true;
                    transform.GetComponent<Piece>().SetRowAndCol(currentRowPosition + 1, currentColumnPosition);
                    transform.GetComponent<Piece>().transform.position = GameObject.Find("gridRow" + (currentRowPosition + 1) + "Column" + (currentColumnPosition)).transform.position;
                }
            }
        }
    }

}
