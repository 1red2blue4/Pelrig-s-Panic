using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour {

    private float cameraSpeed;
    private float cameraScrollSpeed;
    private float cameraMaxZoom;
    private float cameraMinZoom;
    private GameObject columnHighlight;
    //in place in case this script is attached to another object that is not a camera
    private Camera thisCamera;
    GameObject selectedUnit;

    int[] moveValues;

    [SerializeField] Material glowingMaterial;
    Material normalMaterial;

    void Start()
    {
        cameraSpeed = 20.0f;
        cameraScrollSpeed = 20.0f;
        cameraMaxZoom = 11.0f;
        cameraMinZoom = 3.0f;
        thisCamera = gameObject.GetComponent<Camera>();
        columnHighlight = GameObject.FindGameObjectWithTag("ColumnHighlight");
        MovementManager.Setup();
        moveValues = new int[4];
        selectedUnit = null;
        GiveNumbers();

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
        CheckCoinCollect();
        CheckForLineupSwap();
        CheckPlayer();
        if (selectedUnit != null)
        {
            MovePlayer();
        }
    }

    void MovePlayer()
    {
        int direction = -1;
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            direction = 1;
        }

        else if (Input.GetKey(KeyCode.DownArrow))
        {
            direction = 3;
        }

        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            direction = 0;
        }

        else if (Input.GetKey(KeyCode.RightArrow))
        {
            direction = 2;
        }

        if (direction != -1)
            MovementManager.Move(selectedUnit.GetComponent<Piece>(), direction);
    }

    void CheckPlayer()
    {
        
        for (int i = 0; i < Board.possibleMoveableChars.Length; i++)
        {
            int enemiesAround = 0;
            for (int j = 0; j < Board.spawnedEnemies.Length; j++)
            {
                bool a = false;
                bool b = false;
                if (Board.possibleMoveableChars[i].GetComponent<Piece>().rowPosition == Board.spawnedEnemies[j].GetComponent<Piece>().rowPosition - 1 || Board.possibleMoveableChars[i].GetComponent<Piece>().rowPosition == Board.spawnedEnemies[j].GetComponent<Piece>().rowPosition + 1)
                {
                    a = true;
                }
                if (Board.possibleMoveableChars[i].GetComponent<Piece>().colPosition == Board.spawnedEnemies[j].GetComponent<Piece>().colPosition - 1 || Board.possibleMoveableChars[i].GetComponent<Piece>().colPosition == Board.spawnedEnemies[j].GetComponent<Piece>().colPosition + 1)
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
                            selectedUnit = Board.possibleMoveableChars[i].thePiece;
                            normalMaterial = selectedUnit.GetComponent<MeshRenderer>().material;
                            glowingMaterial.color = normalMaterial.color;
                            selectedUnit.GetComponent<MeshRenderer>().material = glowingMaterial;
                            break;
                        }
                    }

                    /*Transform objectHit = hit.transform;
                    for (int i = 0; i < Board.possibleMoveableChars.Length; i++)
                    {
                        if (objectHit == Board.possibleMoveableChars[i].thePiece.transform)
                        {
                            MovementManager.Move(Board.possibleMoveableChars[i]);
                        }
                    }*/
                }
                else
                {
                    selectedUnit.GetComponent<MeshRenderer>().material = normalMaterial;
                    selectedUnit = null;
                }
            }
        }
    }

    public void MoveCamera()
    {
        if (Input.GetAxis("Horizontal") > 0)
        {
            transform.position += new Vector3(cameraSpeed, 0.0f, 0.0f) * Time.deltaTime;
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            transform.position -= new Vector3(cameraSpeed, 0.0f, 0.0f) * Time.deltaTime;
        }
        if (Input.GetAxis("Vertical") > 0)
        {
            transform.position += new Vector3(0.0f, cameraSpeed, 0.0f) * Time.deltaTime;
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            transform.position -= new Vector3(0.0f, cameraSpeed, 0.0f) * Time.deltaTime;
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && gameObject.GetComponent<Camera>().orthographicSize > cameraMinZoom)
        {
            gameObject.GetComponent<Camera>().orthographicSize -= cameraScrollSpeed * Time.deltaTime;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && gameObject.GetComponent<Camera>().orthographicSize < cameraMaxZoom)
        {
            gameObject.GetComponent<Camera>().orthographicSize += cameraScrollSpeed * Time.deltaTime;
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
