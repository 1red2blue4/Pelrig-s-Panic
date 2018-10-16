using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {

    [SerializeField] private int enteredUniversalTileWidth;
    [SerializeField] private int enteredUniversalTileHeight;
    [SerializeField] private GameObject tilePiece;
    [SerializeField] private GameObject tilePieceDead;
    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject[] cannons;
    [SerializeField] private int enteredNumCannons;
    bool first = true;
    public const int MAXCOINNUM = 100;
    public static GameObject[] allTiles;
    public static Piece[] possibleMoveableChars;
    public static Piece[] allCoins;
    public static Piece[] spawnedEnemies;
    public static Cannon[] allCannons;

    public static int numCannons;
    public static int currentCannon;

    public static Wall[] allWalls;

    public static int[,] spaceFieldType;
    public static int numDeadSpaces;
    public static point[] deadPoints;
    public static point[] cannonPoints;

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
    public static int numPossibleMoveableCharacters = 5;
    public static int currentNumCoins = 0;
    public static int numCoinsCollected = 0;
    public const float approxGoldenRatio = 1.618f;

    public int remainingEnemies = 4;
    public static int numberOfEnemies = 0;



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
        universalTileWidth = enteredUniversalTileWidth;
        universalTileHeight = enteredUniversalTileHeight;

        numCannons = enteredNumCannons;
        currentCannon = 0;
        if (numCannons > 0)
        {
            allCannons = new Cannon[numCannons];
            GameObject[] allCannonObjects = new GameObject[numCannons]; //5 for now

            for (int i = 0; i < numCannons; i++)
            {
                allCannonObjects[i] = Instantiate(cannons[i], new Vector3(10000.0f, 10000.0f, 0.0f), Quaternion.identity);
                allCannons[i] = allCannonObjects[i].GetComponent<Cannon>();
                allCannons[i].cannonID = i;
            }
        } 



        //allocate arrays
        possibleMoveableChars = new Piece[5];
        allTiles = new GameObject[universalTileHeight * universalTileWidth];
        
        allCoins = new Piece[MAXCOINNUM];
        GameObject[] allCoinObjects = new GameObject[MAXCOINNUM];
        for (int i = 0; i < allCoinObjects.Length; i++)
        {
            allCoinObjects[i] = Instantiate(coinPiece, new Vector3(10000.0f, 10000.0f, 0.0f), Quaternion.identity);
            allCoins[i] = allCoinObjects[i].GetComponent<Piece>();
        }
        //retrieve pieces from the gameObject with this board
        possibleMoveableChars = gameObject.GetComponents<Piece>();

        //find walls
        allWalls = (Wall[])FindObjectsOfType(typeof(Wall));

        //set up non-traversable spaces
        numDeadSpaces = 0;
        spaceFieldType = new int[universalTileWidth, universalTileHeight];
        deadPoints = new point[universalTileHeight*universalTileWidth];
        cannonPoints = new point[universalTileHeight * universalTileWidth];

        //set up board
        pieceDistance = 1.06f;
        CreateBoard(universalTileWidth, universalTileHeight, midBoardX, midBoardY, spaceFieldType);
	}
	
	// Update is called once per frame
	void Update ()
    {
        SendEverythingDown();
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
                //look for the location
                Vector3 placement;
                GameObject piece;
                placement = new Vector3(-halfWidth + (float)j * pieceDistance + midX, halfHeight - (float)i * pieceDistance - midY, 0.0f);
                //check if it is a dead space
                bool deadSpace = false;
                for (int k = 0; k < allWalls.Length; k++)
                {
                    if (allWalls[k].CalcIfOutside(placement))
                    {
                        deadSpace = true;
                    }
                }
                //label if it is a dead space or not
                if (deadSpace == true)
                {
                    spaceFieldType[j, i] = 0;
                    deadPoints[numDeadSpaces] = new point(j, i);
                    numDeadSpaces++;
                }
                else
                {
                    bool hasCannon = false;
                    if (numCannons > 0)
                    {
                        for (int k = 0; k < allCannons.Length; k++)
                        {
                            if (allCannons[k].cannon.rowPosition == i && allCannons[k].cannon.colPosition == j)
                            {
                                hasCannon = true;
                            }
                        }
                        if (hasCannon == true)
                        {
                            spaceFieldType[j, i] = 2;
                            cannonPoints[currentCannon] = new point(j, i);
                            currentCannon++;
                        }
                    }
                    if (!hasCannon)
                    {
                        spaceFieldType[j, i] = 1;
                    }
                }
                //for every dead field, note the space as dead and do not place a regular tile there
                if (deadSpaces[j, i] == 0)
                {
                    piece = Instantiate(tilePieceDead, placement, Quaternion.identity);
                    piece.name = "gridRow" + i + "Column" + j + " Dead Space";
                }
                else if (deadSpaces[j, i] == 1)
                {
                    piece = Instantiate(tilePiece, placement, Quaternion.identity);
                    piece.name = "gridRow" + i + "Column" + j;
                    GridPositioner sinkDown = piece.GetComponent<GridPositioner>();
                    sinkDown.CheckWhatsBeneath();
                }
                else if (deadSpaces[j, i] == 2)
                {
                    Cannon myCannon = allCannons[currentCannon-1];
                    for (int k = 0; k < numCannons; k++)
                    {
                        if (allCannons[currentCannon - 1].cannonID == k)
                        {
                            myCannon = allCannons[k];
                        }
                    }
                    piece = Instantiate(myCannon.cannon.thePiece, placement, Quaternion.identity);
                    piece.name = "gridRow" + i + "Column" + j + "WithCannon";
                }
                else
                {
                    piece = new GameObject();
                }
                allTiles[i * tileWidth + j] = piece;
            }
        }

        int[] tempRows = new int[possibleMoveableChars.Length];
        int[] tempCols = new int[possibleMoveableChars.Length];

        int[] units = { 10, 9, 8, 7, 6 };
        //set the hero pieces
        for (int i = 0; i < possibleMoveableChars.Length; i++)
        {

            //get one of the locations
            int randCol = 13;
            int randRow = units[i];
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
            GridPositioner bringDown = piece.GetComponent<GridPositioner>();
            bringDown.CheckWhatsBeneath();
        }
    }

    private void CoinSpawn(int numCoins, int tileWidth, int tileHeight, float midX, float midY)
    {
        //if the time elapsed is enough to create a coin, make a coin
        if (coinResetTimer >= timeToWait)
        {
            //find a location for the coin
            //check for the spots where the coin may not be placed
            int[] disallowedRows = new int[possibleMoveableChars.Length + currentNumCoins + numDeadSpaces + spawnedEnemies.Length];
            int[] disallowedCols = new int[possibleMoveableChars.Length + currentNumCoins + numDeadSpaces + spawnedEnemies.Length];
            for (int i = 0; i < possibleMoveableChars.Length + currentNumCoins + numDeadSpaces + spawnedEnemies.Length; i++)
            {
                //do not get the same space as a hero
                if (i < possibleMoveableChars.Length)
                {
                    disallowedRows[i] = possibleMoveableChars[i].rowPosition;
                    disallowedCols[i] = possibleMoveableChars[i].colPosition;
                }
                //do not get the same space as another enemy
                else if (i >= possibleMoveableChars.Length && i < possibleMoveableChars.Length + spawnedEnemies.Length)
                {
                    disallowedRows[i] = allCoins[i - possibleMoveableChars.Length].rowPosition;
                    disallowedCols[i] = allCoins[i - possibleMoveableChars.Length].colPosition;
                }
                //do not get the same space a coin
                else if (i >= possibleMoveableChars.Length + spawnedEnemies.Length && i < possibleMoveableChars.Length + spawnedEnemies.Length + currentNumCoins)
                {
                    disallowedRows[i] = allCoins[i - possibleMoveableChars.Length - spawnedEnemies.Length].rowPosition;
                    disallowedCols[i] = allCoins[i - possibleMoveableChars.Length - spawnedEnemies.Length].colPosition;
                }
                //do not get a dead space
                else
                {
                    disallowedRows[i] = deadPoints[i - possibleMoveableChars.Length - currentNumCoins - spawnedEnemies.Length].y;
                    disallowedCols[i] = deadPoints[i - possibleMoveableChars.Length - currentNumCoins - spawnedEnemies.Length].x;
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
            GridPositioner bringDown = piece.GetComponent<GridPositioner>();
            bringDown.CheckWhatsBeneath();

            //every situation where currentNumCoins increases or decreases, adjust the timeToWait
            currentNumCoins++;
            timeToWait *= approxGoldenRatio;
            coinResetTimer = 0.0f;
        }
    }

    private void SendEverythingDown()
    {
        bool sendingDown = false;
        for (int i = 0; i < allTiles.Length; i++)
        {
            GridPositioner sendDown = allTiles[i].GetComponent<GridPositioner>();
            sendingDown = sendDown.GuideToObjectBeneath(0.1f);
        }
        for (int i = 0; i < possibleMoveableChars.Length; i++)
        {
            GridPositioner sendDown = possibleMoveableChars[i].thePiece.GetComponent<GridPositioner>();
            sendingDown = sendDown.GuideToObjectBeneath(0.1f);
        }
        for (int i = 0; i < allCoins.Length; i++)
        {
            GridPositioner sendDown = allCoins[i].thePiece.GetComponent<GridPositioner>();
            sendingDown = sendDown.GuideToObjectBeneath(0.1f);
        }
        if (numberOfEnemies > 0)
        {
            for (int i = 0; i < spawnedEnemies.Length; i++)
            {
                GridPositioner sendDown = spawnedEnemies[i].thePiece.GetComponent<GridPositioner>();
                sendingDown = sendDown.GuideToObjectBeneath(0.1f);
            }
        }
        if (!sendingDown)
        {
            if (first)
            {
                numberOfEnemies = 4;

                spawnedEnemies = new Piece[4];
                int[] array = { 2, 9, 8, 11, 35, 5, 35, 11 };
                GameObject[] spawnedEnemyObjects = new GameObject[4]; //5 for now
                for (int i = 0; i < (2 * spawnedEnemies.Length); i += 2)
                {
                    float halfWidth = (float)universalTileWidth / 2.0f;
                    float halfHeight = (float)universalTileHeight / 2.0f;
                    float finalXPos = -halfWidth + (float)array[i] * pieceDistance + midBoardX;
                    float finalYPos = halfHeight - (float)array[i + 1] * pieceDistance - midBoardY;
                    Vector3 placement = new Vector3(finalXPos, finalYPos, 0.0f);
                    spawnedEnemyObjects[i/2] = Instantiate(enemy, placement, Quaternion.identity);
                    spawnedEnemies[i/2] = spawnedEnemyObjects[i/2].GetComponent<Piece>();
                    spawnedEnemies[i/2].SetRowAndCol(array[i+1], array[i]);

                    GridPositioner bringDown = spawnedEnemyObjects[i / 2].GetComponent<Piece>().GetComponent<GridPositioner>();
                    bringDown.CheckWhatsBeneath();
                }

                first = false;
            }
            //Main loop
            else
            {
                CoinSpawn(currentNumCoins, universalTileWidth, universalTileHeight, midBoardX, midBoardY);
            }

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

    //Public function as EnemyAI needs the access locations
    public void SpawnEnemy()
    {
        //find a location for the coin
        //check for the spots where the coin may not be placed
        int[] disallowedRows = new int[possibleMoveableChars.Length + currentNumCoins + numDeadSpaces + spawnedEnemies.Length];
        int[] disallowedCols = new int[possibleMoveableChars.Length + currentNumCoins + numDeadSpaces + spawnedEnemies.Length];
        for (int i = 0; i < possibleMoveableChars.Length + currentNumCoins + numDeadSpaces + spawnedEnemies.Length; i++)
        {
            //do not get the same space as a hero
            if (i < possibleMoveableChars.Length)
            {
                disallowedRows[i] = possibleMoveableChars[i].rowPosition;
                disallowedCols[i] = possibleMoveableChars[i].colPosition;
            }
            //do not get the same space as another enemy
            else if (i >= possibleMoveableChars.Length && i < possibleMoveableChars.Length + spawnedEnemies.Length)
            {
                disallowedRows[i] = allCoins[i - possibleMoveableChars.Length].rowPosition;
                disallowedCols[i] = allCoins[i - possibleMoveableChars.Length].colPosition;
            }
            //do not get the same space as a coin
            else if (i >= possibleMoveableChars.Length + spawnedEnemies.Length && i < possibleMoveableChars.Length + spawnedEnemies.Length + currentNumCoins)
            {
                disallowedRows[i] = allCoins[i - possibleMoveableChars.Length - spawnedEnemies.Length].rowPosition;
                disallowedCols[i] = allCoins[i - possibleMoveableChars.Length - spawnedEnemies.Length].colPosition;
            }
            //do not get a dead space
            else
            {
                disallowedRows[i] = deadPoints[i - possibleMoveableChars.Length - currentNumCoins - spawnedEnemies.Length].y;
                disallowedCols[i] = deadPoints[i - possibleMoveableChars.Length - currentNumCoins - spawnedEnemies.Length].x;
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
        float halfWidth = (float)universalTileWidth / 2.0f;
        float halfHeight = (float)universalTileHeight / 2.0f;
        float finalXPos = -halfWidth + (float)col * pieceDistance + midBoardX;
        float finalYPos = halfHeight - (float)row * pieceDistance - midBoardY;
        Vector3 placement = new Vector3(finalXPos, finalYPos, 0.0f);
        spawnedEnemies[numberOfEnemies].transform.position = placement;
        //GameObject piece = Instantiate(spawnedEnemies[numberOfEnemies].GetPiece(), placement, Quaternion.identity);
        spawnedEnemies[numberOfEnemies].SetName("Enemy " + numberOfEnemies);
        spawnedEnemies[numberOfEnemies].SetRowAndCol(row, col);
        GridPositioner bringDown = spawnedEnemies[numberOfEnemies].GetComponent<GridPositioner>();
        bringDown.CheckWhatsBeneath();

        //every situation where currentNumCoins increases or decreases, adjust the timeToWait
        numberOfEnemies++;
    }
}
