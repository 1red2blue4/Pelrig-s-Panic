﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {

    

    [SerializeField] private GameObject tilePiece;
    [SerializeField] private GameObject tilePieceDead;
    public const int MAXCOINNUM = 50;
    public static Piece[] possibleMoveableChars;
    public static Piece[] allCoins;

    public static int[,] spaceFieldType;
    public static int numDeadSpaces;
    public static point[] deadPoints;

    public static float midBoardX;
    public static float midBoardY;
    //coin piece is applied in the inspector
    [SerializeField] private GameObject coinPiece;
    public static float pieceDistance;
    public static int universalTileWidth;
    public static int universalTileHeight;
    public static float timer;
    public static float coinResetTimer;
    public static float timeToWait;
    //initialize here instead since we don't want it to set to 0 every time a board script is instantiated
    public static int currentNumCoins = 0;
    public static int numCoinsCollected = 0;
    public const float approxGoldenRatio = 1.618f;

    public struct point
    {
        public int x;
        public int y;

        public point(int ex, int why)
        {
            x = ex;
            y = why;
        }
    }

    //start is the top left; end is the bottom right
    public struct field
    {
        public point start;
        public point end;
        public bool inUse;
    }


    // Use this for initialization
    void Start ()
    {
        midBoardX = 0.0f;
        midBoardY = 0.0f;
        Time.maximumDeltaTime = 0.1f;
        timer = 0.0f;
        coinResetTimer = 0.0f;
        timeToWait = 1.0f;
        universalTileWidth = 16;
        universalTileHeight = 10;
        //allocate arrays
        possibleMoveableChars = new Piece[4];
        allCoins = new Piece[MAXCOINNUM];
        GameObject[] allCoinObjects = new GameObject[MAXCOINNUM];
        for (int i = 0; i < allCoinObjects.Length; i++)
        {
            allCoinObjects[i] = Instantiate(coinPiece, new Vector3(10000.0f, 10000.0f, 0.0f), Quaternion.identity);
            allCoins[i] = allCoinObjects[i].GetComponent<Piece>();
        }
        //retrieve pieces from the gameObject with this board
        possibleMoveableChars = gameObject.GetComponents<Piece>();

        //set up non-traversable spaces
        numDeadSpaces = 0;
        spaceFieldType = new int[universalTileWidth, universalTileHeight];
        deadPoints = new point[universalTileHeight*universalTileWidth];

        for (int i = 0; i < universalTileHeight; i++)
        {
            for (int j = 0; j < universalTileWidth; j++)
            {
                if (i >= 1 && i <= 6 && j >= 1 && j <= 9)
                {
                    spaceFieldType[j, i] = 0;
                    deadPoints[numDeadSpaces] = new point(j, i);
                    numDeadSpaces++;
                }
                else
                {
                    spaceFieldType[j, i] = 1;
                }
            }
        }

        //set up board
        pieceDistance = 1.06f;
        CreateBoard(universalTileWidth, universalTileHeight, midBoardX, midBoardY, spaceFieldType);
        MovementManager.directionLineup = new MovementManager.Direction[25];
        MovementManager.SetStartDirectionLineup();
	}
	
	// Update is called once per frame
	void Update ()
    {
        CoinSpawn(currentNumCoins, universalTileWidth, universalTileHeight, midBoardX, midBoardY);
        timer += Time.deltaTime;
        coinResetTimer += Time.deltaTime;
    }

    private void CreateBoard(int tileWidth, int tileHeight, float midX, float midY, int[,] deadSpaces)
    {
        float halfWidth = (float)tileWidth / 2.0f;
        float halfHeight = (float)tileHeight / 2.0f;

        //set the visuals of the background of the board
        for (int i = 0; i < tileHeight; i++)
        {
            for (int j = 0; j < tileWidth; j++)
            {
                Vector3 placement;
                GameObject piece;
                //for every dead field, note the space as dead and do not place a regular tile there
                if (deadSpaces[j, i] == 0)
                {
                    placement = new Vector3(-halfWidth + (float)j * pieceDistance + midX, halfHeight - (float)i * pieceDistance - midY, 0.0f);
                    piece = Instantiate(tilePieceDead, placement, Quaternion.identity);
                    piece.name = "gridRow" + i + "Column" + j + " Dead Space";
                }
                else
                {
                    placement = new Vector3(-halfWidth + (float)j * pieceDistance + midX, halfHeight - (float)i * pieceDistance - midY, 0.0f);
                    piece = Instantiate(tilePiece, placement, Quaternion.identity);
                    piece.name = "gridRow" + i + "Column" + j;
                }
            }
        }

        int[] tempRows = new int[possibleMoveableChars.Length];
        int[] tempCols = new int[possibleMoveableChars.Length];

        //set the hero pieces
        for (int i = 0; i < possibleMoveableChars.Length; i++)
        {
            //get one of the locations
            int randCol = (int)Mathf.Floor(Random.value * (float)tileWidth);
            int randRow = (int)Mathf.Floor(Random.value * (float)tileHeight);
            //on the off chance it rolls exactly 1, pick the largest value instead of overflowing
            if (randRow == tileWidth)
            {
                randRow = tileWidth - 1;
            }
            if (randCol == tileHeight)
            {
                randCol = tileHeight - 1;
            }

            possibleMoveableChars[i].SetRowAndCol(randRow, randCol);

            //check if in the same spot as another thing
            tempRows[i] = randRow;
            tempCols[i] = randCol;
            bool shouldReset = false;

            //same spot as another hero
            for (int j = i - 1; j >= 0; j--)
            {
                if (randRow == tempRows[j] && randCol == tempCols[j])
                {
                    shouldReset = true;
                }
            }
            //same spot as a dead space
            for (int j = 0; j < numDeadSpaces; j++)
            {
                if (randRow == deadPoints[j].y && randCol == deadPoints[j].x)
                {
                    shouldReset = true;
                }
            }

            //if in the same spot as another hero, try again
            if (shouldReset)
            {
                i--;
                continue;
            }


            //place the hero
            //PlaceObject(tileWidth, tileHeight, randRow, randCol, pieceDistance, midX, midY, possibleMoveableChars[i], 0, false);
            float finalXPos = -halfWidth + (float)randCol * pieceDistance + midX;
            float finalYPos = halfHeight - (float)randRow * pieceDistance - midY;
            Vector3 placement = new Vector3(finalXPos, finalYPos, 0.0f);
            GameObject piece = Instantiate(possibleMoveableChars[i].GetPiece(), placement, Quaternion.identity);
            possibleMoveableChars[i].SetName(possibleMoveableChars[i].GetPiece().name);
            piece.name = possibleMoveableChars[i].GetName();
            possibleMoveableChars[i].thePiece = piece;
        }
    }

    private void CoinSpawn(int numCoins, int tileWidth, int tileHeight, float midX, float midY)
    {
        //if the time elapsed is enough to create a coin, make a coin
        if (coinResetTimer >= timeToWait)
        {
            
            //find a location for the coin
            //check for the spots where the coin may not be placed
            int[] disallowedRows = new int[possibleMoveableChars.Length + currentNumCoins + numDeadSpaces];
            int[] disallowedCols = new int[possibleMoveableChars.Length + currentNumCoins + numDeadSpaces];
            for (int i = 0; i < possibleMoveableChars.Length + currentNumCoins + numDeadSpaces; i++)
            {
                //do not get the same space as a hero
                if (i < possibleMoveableChars.Length)
                {
                    disallowedRows[i] = possibleMoveableChars[i].rowPosition;
                    disallowedCols[i] = possibleMoveableChars[i].colPosition;
                }
                //do not get the same space as another coin
                else if (i >= possibleMoveableChars.Length && i < possibleMoveableChars.Length + currentNumCoins)
                {
                    disallowedRows[i] = allCoins[i - possibleMoveableChars.Length].rowPosition;
                    disallowedCols[i] = allCoins[i - possibleMoveableChars.Length].colPosition;
                }
                //do not get a dead space
                else
                {
                    disallowedRows[i] = deadPoints[i - possibleMoveableChars.Length - currentNumCoins].y;
                    disallowedCols[i] = deadPoints[i - possibleMoveableChars.Length - currentNumCoins].x;
                }
            }
            //by default, cannot place a coin until you find a space that is allowed
            bool canPlace = false;
            int debugCount = 0;
            int row = 0;
            int col = 0;
            while (!canPlace)
            {
                //select an arbitrary row and column to place the coin
                row = (int)Mathf.Floor(Random.value * universalTileHeight);
                col = (int)Mathf.Floor(Random.value * universalTileWidth);
                canPlace = CheckIfCanPlace(row, col, disallowedRows, disallowedCols);
                //just in case every space is invalid, throw it at (0, 0) after 100 checks
                debugCount++;
                if (debugCount >= 100)
                {
                    canPlace = true;
                }
            }

            //place the coin
            //PlaceObject(tileWidth, tileHeight, row, col, pieceDistance, midX, midY, allCoins[currentNumCoins], currentNumCoins, true);
            float halfWidth = (float)tileWidth / 2.0f;
            float halfHeight = (float)tileHeight / 2.0f;
            float finalXPos = -halfWidth + (float)col * pieceDistance + midX;
            float finalYPos = halfHeight - (float)row * pieceDistance - midY;
            Vector3 placement = new Vector3(finalXPos, finalYPos, 0.0f);
            GameObject piece = Instantiate(allCoins[currentNumCoins].GetPiece(), placement, Quaternion.identity);
            allCoins[currentNumCoins].SetName("Coin " + currentNumCoins);
            piece.name = allCoins[currentNumCoins].GetName();
            allCoins[currentNumCoins].thePiece = piece;
            allCoins[currentNumCoins].SetRowAndCol(row, col);

            //every situation where currentNumCoins increases or decreases, adjust the timeToWait
            currentNumCoins++;
            timeToWait *= approxGoldenRatio;
            coinResetTimer = 0.0f;
        }
    }

    private bool CheckIfCanPlace(int checkedRw, int checkedCl, int[] disallowedRws, int[] disallowedCls)
    {
        //if the selected space is already covered by any piece, return false; otherwise, return true
        for (int i = 0; i < disallowedRws.Length; i++)
        {
            if (checkedRw == disallowedRws[i] && checkedCl == disallowedCls[i])
            {
                return false;
            }
        }
        return true;
    }

    private void PlaceObject(float tlWdth, float tlHght, int rw, int cl, float pcDistnc, float mdX, float mdY, Piece pieceToPlace, int number, bool isNumbered)
    {
        //place the coin
        float halfWidth = (float)tlWdth / 2.0f;
        float halfHeight = (float)tlHght / 2.0f;
        float finalXPos = -halfWidth + (float)cl * pieceDistance + mdX;
        float finalYPos = halfHeight - (float)rw * pieceDistance - mdY;
        Vector3 placement = new Vector3(finalXPos, finalYPos, 0.0f);
        GameObject piece = Instantiate(pieceToPlace.GetPiece(), placement, Quaternion.identity);
        if (isNumbered)
        {
            pieceToPlace.SetName(pieceToPlace.GetPiece().name + number);
        }
        else
        {
            pieceToPlace.SetName(pieceToPlace.GetPiece().name);
        }
        
        piece.name = pieceToPlace.GetName();
        pieceToPlace.thePiece = piece;
    }
}
