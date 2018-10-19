using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour {

    [SerializeField] public GameObject[] allCameras;
    private float cameraSpeed;
    private float cameraScrollSpeed;
    [SerializeField] private float cameraMaxZoom;
    [SerializeField] private float cameraMinZoom;
    private GameObject columnHighlight;
    //0: bottom left; 1: straight on; 2: bottom right
    private int cameraRotPosition;
    private int prevCameraRotPosition;
    private int numCameraRotPositions;
    private bool cameraRotPress;
    private float cameraMovementBetween;
    private bool movingCamera;
    GameObject selectedUnit;
    int theOne;

    private float cameraChangeVertical;
    private float cameraChangeHorizontal;

    public static int[] moveValues;

    [SerializeField] Material glowingMaterial;
    Material normalMaterial;
    public static bool isPlayerTurn;

    void Start()
    {
        cameraChangeHorizontal = 0.0f;
        cameraChangeVertical = 0.0f;
        movingCamera = false;
        cameraMovementBetween = 0.0f;
        numCameraRotPositions = 4;
        allCameras[0] = gameObject;
        for (int i = 1; i < numCameraRotPositions; i++)
        {
            allCameras[i] = GameObject.FindGameObjectWithTag("Camera" + i);
        }
        cameraRotPress = false;
        cameraRotPosition = 1;
        prevCameraRotPosition = cameraRotPosition;
        theOne = 0;
        cameraSpeed = 20.0f;
        cameraScrollSpeed = 20.0f;
        columnHighlight = GameObject.FindGameObjectWithTag("ColumnHighlight");
        MovementManager.Setup();
        moveValues = new int[4];
        selectedUnit = null;
        GiveNumbers();
        isPlayerTurn = true;
    }

    void GiveNumbers()
    {
        //Left - 0, up - 1, right - 2, down - 3
        for (int i = 0; i < 4; i++)
        {
            int randomNumber = (int)Random.Range(0.0f, 9.99f);
            if (randomNumber < 2)
            {
                moveValues[i] = 2;
            }
            else if (randomNumber < 5)
            {
                moveValues[i] = 3;
            }
            else if (randomNumber < 8)
            {
                moveValues[i] = 4;
            }
            else if (randomNumber < 9)
            {
                moveValues[i] = 5;
            }
            else
            {
                moveValues[i] = 6;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckClick();
        MoveCamera();
        CheckRotateCamera();
        if (movingCamera)
        {
            RepositionCamera(cameraRotPosition, prevCameraRotPosition, cameraMovementBetween);
        }
        CheckCoinCollect();
        //CheckForLineupSwap();
        CheckPlayer();
        if (selectedUnit != null && isPlayerTurn)
        {
            MovePlayer();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GiveNumbers();
            isPlayerTurn = false;
        }
    }

    void MovePlayer()
    {
        int direction = -1;
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            direction = 1;
        }

        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            direction = 3;
        }

        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            direction = 0;
        }

        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            direction = 2;
        }

        if (direction != -1)
        {
            if (MovementManager.Move(Board.possibleMoveableChars[theOne], direction, moveValues[direction]))
            {

            }
        }
    }

    void CheckPlayer()
    {
        for (int i = 0; i < Board.possibleMoveableChars.Length; i++)
        {
            int enemiesAround = 0;
            if (Board.numberOfEnemies <= 0)
            {
                return;
            }
            for (int j = 0; j < Board.spawnedEnemies.Length; j++)
            {
                bool a = false;
                bool b = false;
                if (Board.possibleMoveableChars[i].GetComponent<Piece>().rowPosition == Board.spawnedEnemies[j].GetComponent<Piece>().rowPosition - 1 ||
                    Board.possibleMoveableChars[i].GetComponent<Piece>().rowPosition == Board.spawnedEnemies[j].GetComponent<Piece>().rowPosition ||
                    Board.possibleMoveableChars[i].GetComponent<Piece>().rowPosition == Board.spawnedEnemies[j].GetComponent<Piece>().rowPosition + 1)
                {
                    a = true;
                }
                if (Board.possibleMoveableChars[i].GetComponent<Piece>().colPosition == Board.spawnedEnemies[j].GetComponent<Piece>().colPosition - 1 ||
                    Board.possibleMoveableChars[i].GetComponent<Piece>().colPosition == Board.spawnedEnemies[j].GetComponent<Piece>().colPosition ||
                    Board.possibleMoveableChars[i].GetComponent<Piece>().colPosition == Board.spawnedEnemies[j].GetComponent<Piece>().colPosition + 1)
                {
                    b = true;
                }
                if (a && b)
                {
                    enemiesAround += 1;
                }
            }

            if (enemiesAround >= 2)
            {
                Board.possibleMoveableChars[i].SetRowAndCol(1000, 1000);
                Board.possibleMoveableChars[i].GetPiece().transform.position = new Vector3(10000, 10000, 0);
            }
        }
    }


    public void CheckClick()
    {
        if (!isPlayerTurn)
        {
            if (selectedUnit != null)
            {
                selectedUnit.GetComponent<MeshRenderer>().material = normalMaterial;
                selectedUnit = null;
            }
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity)) 
            {
                if (hit.collider.tag == "Player")
                {
                    if (selectedUnit != null)
                    {
                        selectedUnit.GetComponent<MeshRenderer>().material = normalMaterial;
                        selectedUnit = null;
                    }

                    for (int i = 0; i < Board.possibleMoveableChars.Length; i++)
                    {
                        if (hit.transform == Board.possibleMoveableChars[i].thePiece.transform)
                        {
                            theOne = i;
                            selectedUnit = Board.possibleMoveableChars[i].thePiece;
                            normalMaterial = selectedUnit.GetComponent<MeshRenderer>().material;
                            glowingMaterial.color = normalMaterial.color;
                            glowingMaterial.mainTexture = normalMaterial.mainTexture;
                            selectedUnit.GetComponent<MeshRenderer>().material = glowingMaterial;
                            break;
                        }
                    }
                }
                else
                {
                    if (selectedUnit != null)
                    {
                        selectedUnit.GetComponent<MeshRenderer>().material = normalMaterial;
                        selectedUnit = null;
                    }
                }
            }
        }
    }

    public void MoveCamera()
    {
        if (Input.GetAxis("Horizontal") > 0 && cameraChangeHorizontal < 200.0f)
        {
            for (int i = 0; i < numCameraRotPositions; i++)
            {
                allCameras[i].transform.position += new Vector3(cameraSpeed, 0.0f, 0.0f) * Time.deltaTime;
                cameraChangeHorizontal += cameraSpeed * Time.deltaTime;
            }
        }
        else if (Input.GetAxis("Horizontal") < 0 && cameraChangeHorizontal > -200.0f)
        {
            for (int i = 0; i < numCameraRotPositions; i++)
            {
                allCameras[i].transform.position -= new Vector3(cameraSpeed, 0.0f, 0.0f) * Time.deltaTime;
                cameraChangeHorizontal -= cameraSpeed * Time.deltaTime;
            }
        }
        if (Input.GetAxis("Vertical") > 0 && cameraChangeVertical < 200.0f)
        {
            for (int i = 0; i < numCameraRotPositions; i++)
            {
                allCameras[i].transform.position += new Vector3(0.0f, cameraSpeed, 0.0f) * Time.deltaTime;
                cameraChangeVertical += cameraSpeed * Time.deltaTime;
            }
        }
        else if (Input.GetAxis("Vertical") < 0 && cameraChangeVertical > -200.0f)
        {
            for (int i = 0; i < numCameraRotPositions; i++)
            {
                allCameras[i].transform.position -= new Vector3(0.0f, cameraSpeed, 0.0f) * Time.deltaTime;
                cameraChangeVertical -= cameraSpeed * Time.deltaTime;
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && gameObject.GetComponent<Camera>().orthographicSize > cameraMinZoom)
        {
            for (int i = 0; i < numCameraRotPositions; i++)
            {
                allCameras[i].GetComponent<Camera>().orthographicSize -= cameraScrollSpeed * Time.deltaTime;
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && gameObject.GetComponent<Camera>().orthographicSize < cameraMaxZoom)
        {
            for (int i = 0; i < numCameraRotPositions; i++)
            {
                allCameras[i].GetComponent<Camera>().orthographicSize += cameraScrollSpeed * Time.deltaTime;
            }
        }
    }

    public void CheckRotateCamera()
    {
        if (Input.GetAxis("ChangeCamera") != 0 && !cameraRotPress)
        {
            prevCameraRotPosition = cameraRotPosition;
            float direction = Input.GetAxis("ChangeCamera");
            cameraRotPress = true;
            if (direction > 0)
            {
                cameraRotPosition++;
                if (cameraRotPosition >= numCameraRotPositions)
                {
                    cameraRotPosition = 1;
                }
            }
            else
            {
                cameraRotPosition--;
                if (cameraRotPosition < 1)
                {
                    cameraRotPosition = numCameraRotPositions - 1;
                }
            }
            SetCamera();
        }
        else if (Input.GetAxis("ChangeCamera") == 0)
        {
            cameraRotPress = false;
        }
    }

    private void SetCamera()
    {
        cameraMovementBetween = 0.0f;
        movingCamera = true;
    }

    private void RepositionCamera(int camPos, int prevPos, float timeToMove)
    {
        
        transform.position = Vector3.Lerp(allCameras[prevPos].transform.position, allCameras[camPos].transform.position, timeToMove);
        transform.rotation = Quaternion.Lerp(allCameras[prevPos].transform.rotation, allCameras[camPos].transform.rotation, timeToMove);
        if (cameraMovementBetween < 1.0f)
        {
            cameraMovementBetween += 3.0f*Time.deltaTime;
        }
        else
        {
            cameraMovementBetween = 1.0f;
            movingCamera = false;
        }
    }

    private void CheckCoinCollect()
    {
        //for all the heroes and coins...
        for (int i = 0; i < Board.possibleMoveableChars.Length; i++)
        {
            
            for (int j = 0; j < Board.currentNumCoins; j++)
            {
                //check for a hit
                if (Board.possibleMoveableChars[i].rowPosition == Board.allCoins[j].rowPosition && Board.possibleMoveableChars[i].colPosition == Board.allCoins[j].colPosition)
                {
                    Board.allCoins[j].GetPiece().transform.position = new Vector3(10000, 10000, 0.0f);
                    Board.allCoins[j].rowPosition = -5;
                    Board.allCoins[j].colPosition = -5;
                    for (int k = j; k < Board.allCoins.Length - 2; k++)
                    {
                        Board.allCoins[k] = Board.allCoins[k + 1];
                    }
                    //in every situation where the number of coins increases or decreases, adjust the timeToWait
                    Board.currentNumCoins--;
                    Board.timeToWait /= Board.approxGoldenRatio;
                    Board.numCoinsCollected++;
                }
            }
        }
    }

    private void CheckForLineupSwap()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            MovementManager.SwitchDirectionLineup(MovementManager.Direction.Left, columnHighlight);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            MovementManager.SwitchDirectionLineup(MovementManager.Direction.Right, columnHighlight);
        }
    }
}
