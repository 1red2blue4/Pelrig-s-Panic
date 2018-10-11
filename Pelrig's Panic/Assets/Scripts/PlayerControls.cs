﻿using System.Collections;
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
    
	void Start ()
    {
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
        cameraSpeed = 20.0f;
        cameraScrollSpeed = 20.0f;
        columnHighlight = GameObject.FindGameObjectWithTag("ColumnHighlight");
        MovementManager.Setup();
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        CheckClick();
        MoveCamera();
        CheckRotateCamera();
        if (movingCamera)
        {
            RepositionCamera(cameraRotPosition, prevCameraRotPosition, cameraMovementBetween);
        }
        CheckCoinCollect();
        CheckForLineupSwap();
        SelectCharacter();
	}



    public void CheckClick()
    {

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity)) 
            {
                if (hit.collider != null)
                {
                    Transform objectHit = hit.transform;
                    for (int i = 0; i < Board.possibleMoveableChars.Length; i++)
                    {
                        if (objectHit == Board.possibleMoveableChars[i].thePiece.transform)
                        {
                            MovementManager.Move(Board.possibleMoveableChars[i]);
                        }
                    }
                }
            }
        }
    }

    public void SelectCharacter()
    {
        
        if (Input.GetKeyDown(KeyCode.Y))
        {
            MovementManager.Move(Board.possibleMoveableChars[0]);
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            MovementManager.Move(Board.possibleMoveableChars[1]);
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            MovementManager.Move(Board.possibleMoveableChars[2]);
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            MovementManager.Move(Board.possibleMoveableChars[3]);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            MovementManager.Move(Board.possibleMoveableChars[4]);
        }
    }

    public void MoveCamera()
    {
        if (Input.GetAxis("Horizontal") > 0)
        {
            for (int i = 0; i < numCameraRotPositions; i++)
            {
                allCameras[i].transform.position += new Vector3(cameraSpeed, 0.0f, 0.0f) * Time.deltaTime;
            }
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            for (int i = 0; i < numCameraRotPositions; i++)
            {
                allCameras[i].transform.position -= new Vector3(cameraSpeed, 0.0f, 0.0f) * Time.deltaTime;
            }
        }
        if (Input.GetAxis("Vertical") > 0)
        {
            for (int i = 0; i < numCameraRotPositions; i++)
            {
                allCameras[i].transform.position += new Vector3(0.0f, cameraSpeed, 0.0f) * Time.deltaTime;
            }
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            for (int i = 0; i < numCameraRotPositions; i++)
            {
                allCameras[i].transform.position -= new Vector3(0.0f, cameraSpeed, 0.0f) * Time.deltaTime;
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
