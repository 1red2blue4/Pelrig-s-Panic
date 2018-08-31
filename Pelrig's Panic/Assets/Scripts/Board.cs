using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {

    [SerializeField] private GameObject tilePiece;
    public static Piece[] possibleMoveableChars;
    public static float pieceDistance;
    public static int universalTileWidth;
    public static int universalTileHeight;
    public static float timer;
    public static float coinResetTimer;
    public static int currentNumCoins;
    public const float approxGoldenRatio = 1.618f;
    public static float timeToWait;

    // Use this for initialization
    void Start ()
    {
        Time.maximumDeltaTime = 0.1f;
        timer = 0.0f;
        coinResetTimer = 0.0f;
        timeToWait = 1.0f;
        currentNumCoins = 0;
        universalTileWidth = 16;
        universalTileHeight = 10;
        //allocate arrays
        possibleMoveableChars = new Piece[4];
        //retrieve pieces from the gameObject with this board
        possibleMoveableChars = gameObject.GetComponents<Piece>();
        //set up board
        pieceDistance = 1.06f;
        CreateBoard(universalTileWidth, universalTileHeight, 0.0f, 0.0f);
        MovementManager.directionLineup = new MovementManager.Direction[100];
        MovementManager.SetStartDirectionLineup();
	}
	
	// Update is called once per frame
	void Update ()
    {
        CoinSpawn(currentNumCoins);
        timer += Time.deltaTime;
        coinResetTimer += Time.deltaTime;
    }

    private void CreateBoard(int tileWidth, int tileHeight, float midX, float midY)
    {
        float halfWidth = (float)tileWidth / 2.0f;
        float halfHeight = (float)tileHeight / 2.0f;

        //set the visuals of the background of the board
        for (int i = 0; i < tileHeight; i++)
        {
            for (int j = 0; j < tileWidth; j++)
            {
                Vector3 placement = new Vector3(-halfWidth + (float)j*pieceDistance + midX, halfHeight - (float)i*pieceDistance - midY, 0.0f);
                GameObject piece = Instantiate(tilePiece, placement, Quaternion.identity);
                piece.name = "gridRow" + i + "Column" + j;
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

            //check if in the same spot as another hero
            tempRows[i] = randRow;
            tempCols[i] = randCol;
            bool shouldReset = false;

            for (int j = i - 1; j >= 0; j--)
            {
                if (randRow == tempRows[j] && randCol == tempCols[j])
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
            Vector3 placement = new Vector3(-halfWidth + (float)randCol * pieceDistance + midX, halfHeight - (float)randRow * pieceDistance - midY, 0.0f);
            GameObject piece = Instantiate(possibleMoveableChars[i].GetPiece(), placement, Quaternion.identity);
            possibleMoveableChars[i].SetName(possibleMoveableChars[i].GetPiece().name);
            piece.name = possibleMoveableChars[i].GetName();
            possibleMoveableChars[i].thePiece = piece;
        }
    }

    private void CoinSpawn(int numCoins)
    {
        Debug.Log("Timer: " + coinResetTimer);
        Debug.Log("Time To Wait: " + timeToWait);
        if (coinResetTimer >= timeToWait)
        {
            //every situation where currentNumCoins increases or decreases, adjust the timeToWait
            currentNumCoins++;
            timeToWait *= approxGoldenRatio;
            coinResetTimer = 0.0f;
            Debug.Log("Num Coins: " + currentNumCoins);
        }
    }
}
