using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {

    [SerializeField] private int enteredUniversalTileWidth;
    [SerializeField] private int enteredUniversalTileHeight;
    [SerializeField] private GameObject tilePiece;
    [SerializeField] private GameObject tilePieceDead;
    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject pirateBossObject;
    [SerializeField] private GameObject cannonPrefab;
    [SerializeField] private GameObject generatorPrefab;
    public static Generator[] generators;
    static public bool first;

    public GameObject mainCamera;

    public const int MAXCOINNUM = 100;
    public static GameObject[] allTiles;
    public static Piece[] possibleMoveableChars;
    public static Piece[] allCoins;
    public static List<Piece> spawnedEnemies;
    public static Cannon[] allCannons;

    public static int numGenerators;
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
    
    public static GameObject pirateBossSpawned;
    public static Piece pirateBoss;

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
        first = true;
        numGenerators = 0;
        numCannons = 0;
        spawnedEnemies = new List<Piece>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        midBoardX = 0.0f;
        midBoardY = 0.0f;
        Time.maximumDeltaTime = 0.1f;
        timer = 0.0f;
        coinResetTimer = 0.0f;
        timeToWait = 1.0f;
        universalTileWidth = enteredUniversalTileWidth;
        universalTileHeight = enteredUniversalTileHeight;

        //allocate arrays
        possibleMoveableChars = new Piece[5];
        allTiles = new GameObject[universalTileHeight * universalTileWidth];
        //Debug.Log("allTiles:    "+ allTiles);
        
        allCoins = new Piece[MAXCOINNUM];
        GameObject[] allCoinObjects = new GameObject[MAXCOINNUM];
        for (int i = 0; i < allCoinObjects.Length; i++)
        {
            allCoinObjects[i] = Instantiate(coinPiece, new Vector3(10000.0f, 10000.0f, 0.0f), Quaternion.identity);
            allCoins[i] = allCoinObjects[i].GetComponent<Piece>();
        }
        //retrieve pieces from the gameObject with this board
        possibleMoveableChars = gameObject.GetComponents<Piece>();

        for (int i = 0; i < possibleMoveableChars.Length; i++)
        {
            possibleMoveableChars[i].presenceValue = possibleMoveableChars[i].initialPresenceValue;
            possibleMoveableChars[i].resistanceValue = possibleMoveableChars[i].initialResistanceValue;
        }
        

        //find walls
        allWalls = (Wall[])FindObjectsOfType(typeof(Wall));

        //set up non-traversable spaces
        numDeadSpaces = 0;
        spaceFieldType = new int[universalTileWidth, universalTileHeight];
        deadPoints = new point[universalTileHeight*universalTileWidth];
        cannonPoints = new point[universalTileHeight * universalTileWidth];

        //set up board
        pieceDistance = 1.56f;
        CreateBoard(universalTileWidth, universalTileHeight, midBoardX, midBoardY, spaceFieldType);
	}
	
	// Update is called once per frame
	void Update ()
    {
        SendEverythingDown();
        RotateCharacterPosition(possibleMoveableChars, possibleMoveableChars.Length, 2);
        RotateCharacterPositionWithList(spawnedEnemies, spawnedEnemies.Count, 1);
        Piece[] cannonPieces = new Piece[allCannons.Length];
        for (int i = 0; i < cannonPieces.Length; i++)
        {
            cannonPieces[i] = allCannons[i].cannon;
        }
        RotateCharacterPosition(cannonPieces, cannonPieces.Length, 2);
        Piece[] generatorPieces = new Piece[generators.Length];
        for (int i = 0; i < generatorPieces.Length; i++)
        {
            generatorPieces[i] = generators[i].generator;
        }
        RotateCharacterPosition(generatorPieces, generatorPieces.Length, 2);

        GridPositioner childPositionerBoss = pirateBoss.thePiece.transform.GetChild(0).GetComponent<GridPositioner>();
        childPositionerBoss.mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        childPositionerBoss.AdjustToCamera();

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

        int[] unitsRow = { 5, 6, 6, 7, 7 };
        int[] unitsCol = { 8, 9, 8, 8, 9 };
        //set the hero pieces
        for (int i = 0; i < possibleMoveableChars.Length; i++)
        {
            //get one of the locations
            int randCol = unitsCol[i];
            int randRow = unitsRow[i];
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
           // Debug.Log("selectedUnit:     " + possibleMoveableChars[i].rowPosition);

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
            possibleMoveableChars[i].thePiece.transform.Rotate(new Vector3(270.0f, 0.0f, 0.0f));
            GridPositioner bringDown = piece.GetComponent<GridPositioner>();
            bringDown.CheckWhatsBeneath();
            bringDown.mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }
    }

    private void RotateCharacterPosition(Piece[] piecesToRotate, int numOfThesePieces, int numChildGridPositioners)
    {
        for (int i = 0; i < numOfThesePieces; i++)
        {
            GridPositioner[] tempGridPositioners = new GridPositioner[numChildGridPositioners];
            for (int k = 0; k < numChildGridPositioners; k++)
            {
                GridPositioner childPositioner = piecesToRotate[i].thePiece.GetComponent<GridPositioner>();
                //look for the image to rotate
                for (int j = 0; j < piecesToRotate[i].thePiece.transform.childCount; j++)
                {
                    if (piecesToRotate[i].thePiece.transform.GetChild(j).GetComponent<GridPositioner>() != null)
                    {
                        if (k == 0)
                        {
                            childPositioner = piecesToRotate[i].thePiece.transform.GetChild(j).GetComponent<GridPositioner>();
                        }
                        else
                        {
                            for (int g = k - 1; g >= 0; g--)
                            {
                                if (piecesToRotate[i].thePiece.transform.GetChild(j).GetComponent<GridPositioner>() != tempGridPositioners[g])
                                {
                                    childPositioner = piecesToRotate[i].thePiece.transform.GetChild(j).GetComponent<GridPositioner>();
                                }
                            }
                        }
                    }
                }
                if (childPositioner != piecesToRotate[i].thePiece.GetComponent<GridPositioner>() && childPositioner != null)
                {
                    childPositioner.mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
                    childPositioner.AdjustToCamera();
                    tempGridPositioners[k] = childPositioner;
                }
            }
        }
    }

    private void RotateCharacterPositionWithList(List<Piece> piecesToRotate, int numOfThesePieces, int numChildGridPositioners)
    {
        for (int i = 0; i < numOfThesePieces; i++)
        {
            GridPositioner[] tempGridPositioners = new GridPositioner[numChildGridPositioners];
            for (int k = 0; k < numChildGridPositioners; k++)
            {
                GridPositioner childPositioner = piecesToRotate[i].thePiece.GetComponent<GridPositioner>();
                //look for the image to rotate
                for (int j = 0; j < piecesToRotate[i].thePiece.transform.childCount; j++)
                {
                    if (piecesToRotate[i].thePiece.transform.GetChild(j).GetComponent<GridPositioner>() != null)
                    {
                        if (k == 0)
                        {
                            childPositioner = piecesToRotate[i].thePiece.transform.GetChild(j).GetComponent<GridPositioner>();
                        }
                        else
                        {
                            for (int g = k - 1; g >= 0; g--)
                            {
                                if (piecesToRotate[i].thePiece.transform.GetChild(j).GetComponent<GridPositioner>() != tempGridPositioners[g])
                                {
                                    childPositioner = piecesToRotate[i].thePiece.transform.GetChild(j).GetComponent<GridPositioner>();
                                }
                            }
                        }
                    }
                }
                if (childPositioner != piecesToRotate[i].thePiece.GetComponent<GridPositioner>() && childPositioner != null)
                {
                    childPositioner.mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
                    childPositioner.AdjustToCamera();
                    tempGridPositioners[k] = childPositioner;
                }
            }
        }
    }

    /*
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
            bringDown.mainCamera = mainCamera;
            bringDown.CheckWhatsBeneath();

            //every situation where currentNumCoins increases or decreases, adjust the timeToWait
            currentNumCoins++;
            timeToWait *= approxGoldenRatio;
            coinResetTimer = 0.0f;
        }
    }
    */
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
            GridPositioner sendDown = possibleMoveableChars[i].thePiece.transform.GetComponent<GridPositioner>();
            sendingDown = sendDown.GuideToObjectBeneath(0.1f);
        }
        for (int i = 0; i < allCoins.Length; i++)
        {
            GridPositioner sendDown = allCoins[i].thePiece.GetComponent<GridPositioner>();
            sendingDown = sendDown.GuideToObjectBeneath(0.1f);
            sendDown.AdjustToCamera();
        }
        if (numCannons > 0)
        {
            for (int i = 0; i < allCannons.Length; i++)
            {
                GridPositioner sendDown = allCannons[i].gameObject.GetComponent<GridPositioner>();
                sendingDown = sendDown.GuideToObjectBeneath(0.1f);
                //sendDown.AdjustToCamera();
            }
        }
        if (numGenerators > 0)
        {
            for (int i = 0; i < generators.Length; i++)
            {
                GridPositioner sendDown = generators[i].gameObject.GetComponent<GridPositioner>();
                sendingDown = sendDown.GuideToObjectBeneath(0.1f);
                //sendDown.AdjustToCamera();
            }
        }
        if (pirateBoss != null)
            sendingDown = pirateBoss.thePiece.GetComponent<GridPositioner>().GuideToObjectBeneath(0.1f);
        if (spawnedEnemies.Count > 0)
        {
            for (int i = 0; i < spawnedEnemies.Count; i++)
            {
                if (spawnedEnemies[i] == null)
                {
                    spawnedEnemies.RemoveAt(i);
                    if (i >= spawnedEnemies.Count)
                        break;
                }
                GridPositioner sendDown = spawnedEnemies[i].thePiece.GetComponent<GridPositioner>();
                sendingDown = sendDown.GuideToObjectBeneath(0.1f);
            }
        }
        if (!sendingDown)
        {
            if (first)
            {                
                int[] array = { 19, 3, 19, 9, 26, 4, 26, 8 };
                //GameObject[] spawnedEnemyObjects = new GameObject[4]; //5 for now
                for (int i = 0; i < (8); i += 2)
                {
                    GameObject spawnedEnemyObject;
                    float halfWidth = (float)universalTileWidth / 2.0f;
                    float halfHeight = (float)universalTileHeight / 2.0f;
                    float finalXPos = -halfWidth + (float)array[i] * pieceDistance + midBoardX;
                    float finalYPos = halfHeight - (float)array[i + 1] * pieceDistance - midBoardY;
                    Vector3 placement = new Vector3(finalXPos, finalYPos, 0.0f);
                    spawnedEnemyObject = Instantiate(enemy, placement, Quaternion.identity);
                    spawnedEnemies.Add(spawnedEnemyObject.GetComponent<Piece>());
                    spawnedEnemies[i/2].SetRowAndCol(array[i+1], array[i]);

                    GridPositioner bringDown = spawnedEnemyObject.GetComponent<Piece>().GetComponent<GridPositioner>();
                    bringDown.CheckWhatsBeneath();
                }
                //Put the pirate boss here
                int bossCol = 25;
                int bossRow = 6;
                float halfWidth1 = (float)universalTileWidth / 2.0f;
                float halfHeight1 = (float)universalTileHeight / 2.0f;
                float finalXPos1 = -halfWidth1 + bossCol * pieceDistance + midBoardX;
                float finalYPos1 = halfHeight1 - bossRow * pieceDistance - midBoardY;
                Vector3 pos1 = new Vector3(finalXPos1, finalYPos1, 0.0f);
                pirateBossSpawned = Instantiate(pirateBossObject, pos1, Quaternion.identity);
                pirateBoss = pirateBossSpawned.GetComponent<Piece>();
                pirateBoss.SetRowAndCol(bossRow, bossCol);
                GridPositioner bringDown1 = pirateBoss.thePiece.GetComponent<GridPositioner>();
                bringDown1.CheckWhatsBeneath();

                PlaceCannons();
                PlaceGenerators();
                first = false;
            }
            //Main loop
            else
            {
                //CoinSpawn(currentNumCoins, universalTileWidth, universalTileHeight, midBoardX, midBoardY);
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
    public void SpawnEnemy(int numberOfEnemies)
    {
        //find a location for the coin
        //check for the spots where the coin may not be placed
        for (int j = 0; j < numberOfEnemies; j++)
        {

            int[] disallowedRows = new int[possibleMoveableChars.Length + currentNumCoins + numDeadSpaces + spawnedEnemies.Count + 1];
            int[] disallowedCols = new int[possibleMoveableChars.Length + currentNumCoins + numDeadSpaces + spawnedEnemies.Count + 1];
            for (int i = 0; i < possibleMoveableChars.Length + currentNumCoins + numDeadSpaces + spawnedEnemies.Count; i++)
            {
                //do not get the same space as a hero
                if (i < possibleMoveableChars.Length)
                {
                    disallowedRows[i] = possibleMoveableChars[i].rowPosition;
                    disallowedCols[i] = possibleMoveableChars[i].colPosition;
                }
                //do not get the same space as another enemy
                else if (i >= possibleMoveableChars.Length && i < possibleMoveableChars.Length + spawnedEnemies.Count)
                {
                    disallowedRows[i] = allCoins[i - possibleMoveableChars.Length].rowPosition;
                    disallowedCols[i] = allCoins[i - possibleMoveableChars.Length].colPosition;
                }
                //do not get the same space as a coin
                else if (i >= possibleMoveableChars.Length + spawnedEnemies.Count && i < possibleMoveableChars.Length + spawnedEnemies.Count + currentNumCoins)
                {
                    disallowedRows[i] = allCoins[i - possibleMoveableChars.Length - spawnedEnemies.Count].rowPosition;
                    disallowedCols[i] = allCoins[i - possibleMoveableChars.Length - spawnedEnemies.Count].colPosition;
                }
                //do not get a dead space
                else
                {
                    disallowedRows[i] = deadPoints[i - possibleMoveableChars.Length - currentNumCoins - spawnedEnemies.Count].y;
                    disallowedCols[i] = deadPoints[i - possibleMoveableChars.Length - currentNumCoins - spawnedEnemies.Count].x;
                }
            }
            disallowedRows[disallowedRows.Length - 1] = pirateBoss.rowPosition;
            disallowedCols[disallowedCols.Length - 1] = pirateBoss.colPosition;
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

            GameObject spawnedEnemyObject = Instantiate(enemy, placement, Quaternion.identity);
            spawnedEnemyObject.GetComponent<Piece>().SetRowAndCol(row, col);
            GridPositioner bringDown = spawnedEnemyObject.GetComponent<Piece>().GetComponent<GridPositioner>();
            bringDown.CheckWhatsBeneath();
            spawnedEnemies.Add(spawnedEnemyObject.GetComponent<Piece>());
        }
    }

    void PlaceCannons()
    {
        //colums, row for each cannon
        allCannons = new Cannon[4]; //4
        int[] arrayColumns = { 18,18,12,12};
        int[] arrayRows = {2,10,2,10 };
        //GameObject[] spawnedEnemyObjects = new GameObject[4]; //5 for now
        for (int i = 0; i < allCannons.Length; i ++)
        {
            float halfWidth = (float)universalTileWidth / 2.0f;
            float halfHeight = (float)universalTileHeight / 2.0f;
            float finalXPos = -halfWidth + (float)arrayColumns[i] * pieceDistance + midBoardX;
            float finalYPos = halfHeight - (float)arrayRows[i] * pieceDistance - midBoardY;
            Vector3 placement = new Vector3(finalXPos, finalYPos, 0.0f);
            GameObject newCannon = Instantiate(cannonPrefab, placement, Quaternion.identity);
            allCannons[i] = newCannon.GetComponent<Cannon>();
            allCannons[i].cannon.SetRowAndCol(arrayRows[i], arrayColumns[i]);
            newCannon.name = "cannon" + i;
            GridPositioner bringDown = allCannons[i].gameObject.GetComponent<GridPositioner>();
            bringDown.CheckWhatsBeneath();
            GameObject.Find("gridRow" + arrayRows[i] + "Column" + arrayColumns[i]).GetComponent<SpriteRenderer>().enabled = false;
            numCannons++;
        }
    }

    void PlaceGenerators()
    {
        //colums, row for each cannon
        generators = new Generator[2]; //4
        int[] arrayColumns = { 8, 8};
        int[] arrayRows = { 3, 9};
        //GameObject[] spawnedEnemyObjects = new GameObject[4]; //5 for now
        for (int i = 0; i < generators.Length; i++)
        {
            float halfWidth = (float)universalTileWidth / 2.0f;
            float halfHeight = (float)universalTileHeight / 2.0f;
            float finalXPos = -halfWidth + (float)arrayColumns[i] * pieceDistance + midBoardX;
            float finalYPos = halfHeight - (float)arrayRows[i] * pieceDistance - midBoardY;
            Vector3 placement = new Vector3(finalXPos, finalYPos, 0.0f);
            GameObject newGenerator = Instantiate(generatorPrefab, placement, Quaternion.identity);
            generators[i] = newGenerator.GetComponent<Generator>();
            generators[i].generator.SetRowAndCol(arrayRows[i], arrayColumns[i]);

            GridPositioner bringDown = generators[i].gameObject.GetComponent<GridPositioner>();
            bringDown.CheckWhatsBeneath();
            GameObject.Find("gridRow" + arrayRows[i] + "Column" + arrayColumns[i]).GetComponent<SpriteRenderer>().enabled = false;
            numGenerators++;
        }
    }
}
