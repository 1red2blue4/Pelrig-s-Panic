﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControls : MonoBehaviour
{
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
    int roundCounter = 0;

    private float cameraChangeVertical;
    private float cameraChangeHorizontal;

    public static int[] moveValues;

    [SerializeField] Material glowingMaterial;
    Material normalMaterial;
    public static bool isPlayerTurn;

    GameObject panelUnderCharacter;
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
        panelUnderCharacter = null;
    }

    public void GiveNumbers()
    {
        //Left - 0, up - 1, right - 2, down - 3
        for (int i = 0; i < 4; i++)
        {
            int randomNumber = (int)Random.Range(0.0f, 13.99f);
            if (randomNumber < 1)
            {
                moveValues[i] = 1;
            }
            else if (randomNumber < 4)
            {
                moveValues[i] = 2;
            }
            else if (randomNumber < 7)
            {
                moveValues[i] = 3;
            }
            else if (randomNumber < 10)
            {
                moveValues[i] = 4;
            }
            else if (randomNumber < 12)
            {
                moveValues[i] = 5;
            }
            else
            {
                moveValues[i] = 6;
            }
            //Debug.Log("moveValues[i]:   "+ moveValues[i]);
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
        
        //CheckCoinCollect();
        //CheckForLineupSwap();
        CheckPlayer();
        if (isPlayerTurn)
        {

            if (selectedUnit != null)
            {
                MovePlayer();
                
            }
            if (Input.GetKeyDown(KeyCode.Space) || EndTurnButtonScript.isButtonPressed)// || Input.GetMouseButtonDown(0))
            {
                //Clearing all highlighted possible moves and selected character.
                ClearAllGrids();
                // panelUnderCharacter.GetComponent<PanelUnderCharacter>().visible = false;
                if(selectedUnit)
                    DisablePanelUnderCharacter(selectedUnit);
                selectedUnit = null;
                EndTurnButtonScript.isButtonPressed = false;
                GiveNumbers();
                isPlayerTurn = false;
                roundCounter++;
                if (roundCounter >= 4)
                {
                    GameObject.Find("GridLevelStuff").GetComponentInChildren<Board>().SpawnEnemy((int)Random.Range(1.0f, 3.99f));
                    roundCounter = 0;
                }
                EnemyTurnsActivate();
            }
        }
        else if (EnemyMovesDone())
        {
            isPlayerTurn = true;
          //  GameObject.Find("EndTurn").transform.GetComponent<Button>().transition = Navigation.None;
            ExperimentalResources.ReInitializeResources();
           // GameObject.Find("EndTurn").transform.GetComponent<EndButtonToggle>().isVisible = false;
        }
    }

    public static void EnemyTurnsActivate()
    {
        bool countRound = false;
        for (int i = 0; i < Board.possibleMoveableChars.Length; i++)
        {
            if ((Board.possibleMoveableChars[i].rowPosition > 34 || Board.possibleMoveableChars[i].rowPosition < 38) &&
                (Board.possibleMoveableChars[i].colPosition == 8 || Board.possibleMoveableChars[i].colPosition == 9))
            {
                countRound = true;
                break;
            }
        }
        if (countRound)
        {
            Piece pirateBoss = GameObject.FindGameObjectWithTag("PirateBoss").GetComponent<Piece>();
            if (pirateBoss != null)
            {
                if ((pirateBoss.colPosition > 34 || Board.pirateBoss.rowPosition < 38) &&
                   (pirateBoss.colPosition == 8 || pirateBoss.colPosition == 9))
                {
                    YouWin.roundCount = 0;
                }
            }
            else
            {
                YouWin.roundCount++;
            }
        }
        else
        {
            YouWin.roundCount = 0;
        }
        for (int i = 0; i < Board.spawnedEnemies.Count; i++)
        {
            Board.spawnedEnemies[i].GetComponent<EnemyAI>().isTurnActive = true;
        }
        Board.pirateBoss.GetComponent<PirateCaptainAI>().isTurnActive = true;
    }

    bool EnemyMovesDone()
    {
        if (Board.pirateBoss.GetComponent<PirateCaptainAI>().isTurnActive == true)
        {
            return false;
        }
        for (int i = 0; i < Board.spawnedEnemies.Count; i++)
        {
            if (Board.spawnedEnemies[i].GetComponent<EnemyAI>().isTurnActive == true)
            {
                return false;
            }
        }
        return true;
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
        int count = 0;
        for (int i = 0; i < Board.possibleMoveableChars.Length; i++)
        {
            if (Board.possibleMoveableChars[i].rowPosition == 1000)
            {
                count++;                
                continue;
            }
            int enemiesAround = 0;
            for (int j = 0; j < Board.spawnedEnemies.Count; j++)
            {
                if (Board.possibleMoveableChars[i].rowPosition == Board.spawnedEnemies[j].rowPosition - 1 ||
                    Board.possibleMoveableChars[i].rowPosition == Board.spawnedEnemies[j].rowPosition ||
                    Board.possibleMoveableChars[i].rowPosition == Board.spawnedEnemies[j].rowPosition + 1)
                {
                }
                else
                {
                    continue;
                }
                if (Board.possibleMoveableChars[i].colPosition == Board.spawnedEnemies[j].colPosition - 1 ||
                    Board.possibleMoveableChars[i].colPosition == Board.spawnedEnemies[j].colPosition ||
                    Board.possibleMoveableChars[i].colPosition == Board.spawnedEnemies[j].colPosition + 1)
                {
                }
                else
                {
                    continue;
                }

                enemiesAround += 1;
            }
            if (Board.pirateBoss != null)
            {
                if (Board.possibleMoveableChars[i].rowPosition == Board.pirateBoss.rowPosition - 1 ||
                    Board.possibleMoveableChars[i].rowPosition == Board.pirateBoss.rowPosition ||
                    Board.possibleMoveableChars[i].rowPosition == Board.pirateBoss.rowPosition + 1)
                {
                    if (Board.possibleMoveableChars[i].colPosition == Board.pirateBoss.colPosition - 1 ||
                        Board.possibleMoveableChars[i].colPosition == Board.pirateBoss.colPosition ||
                        Board.possibleMoveableChars[i].colPosition == Board.pirateBoss.colPosition + 1)
                    {
                        enemiesAround += 2;
                    }
                }
            }

            if (enemiesAround >= 4)
            {
                if (selectedUnit == Board.possibleMoveableChars[i].GetPiece())
                {
                    selectedUnit = null;
                }
                Board.possibleMoveableChars[i].SetRowAndCol(1000, 1000);
                Board.possibleMoveableChars[i].GetPiece().transform.position = new Vector3(10000, 10000, 0);
            }
            
            UIValues resistance = Board.possibleMoveableChars[i].thePiece.GetComponent<ValueHolder>().resistanceObj.GetComponent<UIValues>();
            resistance.SetValue(resistance.initialValue - enemiesAround);
            Board.possibleMoveableChars[i].resistanceValue = resistance.initialValue - enemiesAround;
        }

        
        if (count >= 2 && !Board.first)
        {
            GameObject.Find("WinScreen").GetComponentInChildren<YouWin>().youLose = true;
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
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //ClearAllGrids();
            GameObject.Find("#Kent_Fantasy_Realm_temp").transform.GetChild(0).GetComponent<PanelUnderCharacter>().visible = true;
            GameObject.Find("#Meda_Fantasy_Realm_temp").transform.GetChild(0).GetComponent<PanelUnderCharacter>().visible = false;
            GameObject.Find("#Hally_Fantasy_Realm_temp").transform.GetChild(0).GetComponent<PanelUnderCharacter>().visible = false;
            GameObject.Find("#Ed_Fantasy_Realm_temp").transform.GetChild(0).GetComponent<PanelUnderCharacter>().visible = false;
            GameObject.Find("#Jade_Fantasy_Realm_temp").transform.GetChild(0).GetComponent<PanelUnderCharacter>().visible = false;
           

                UnoccupiedSpaceEnable(Board.possibleMoveableChars[theOne]);
            }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
           // ClearAllGrids();
            GameObject.Find("#Kent_Fantasy_Realm_temp").transform.GetChild(0).GetComponent<PanelUnderCharacter>().visible = false;
            GameObject.Find("#Meda_Fantasy_Realm_temp").transform.GetChild(0).GetComponent<PanelUnderCharacter>().visible = true;
            GameObject.Find("#Hally_Fantasy_Realm_temp").transform.GetChild(0).GetComponent<PanelUnderCharacter>().visible = false;
            GameObject.Find("#Ed_Fantasy_Realm_temp").transform.GetChild(0).GetComponent<PanelUnderCharacter>().visible = false;
            GameObject.Find("#Jade_Fantasy_Realm_temp").transform.GetChild(0).GetComponent<PanelUnderCharacter>().visible = false;
            UnoccupiedSpaceEnable(Board.possibleMoveableChars[theOne]);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
           // ClearAllGrids();
            GameObject.Find("#Kent_Fantasy_Realm_temp").transform.GetChild(0).GetComponent<PanelUnderCharacter>().visible = false;
            GameObject.Find("#Meda_Fantasy_Realm_temp").transform.GetChild(0).GetComponent<PanelUnderCharacter>().visible = false;
            GameObject.Find("#Hally_Fantasy_Realm_temp").transform.GetChild(0).GetComponent<PanelUnderCharacter>().visible = true;
            GameObject.Find("#Ed_Fantasy_Realm_temp").transform.GetChild(0).GetComponent<PanelUnderCharacter>().visible = false;
            GameObject.Find("#Jade_Fantasy_Realm_temp").transform.GetChild(0).GetComponent<PanelUnderCharacter>().visible = false;
            UnoccupiedSpaceEnable(Board.possibleMoveableChars[theOne]);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
           // ClearAllGrids();
            GameObject.Find("#Kent_Fantasy_Realm_temp").transform.GetChild(0).GetComponent<PanelUnderCharacter>().visible = false;
            GameObject.Find("#Meda_Fantasy_Realm_temp").transform.GetChild(0).GetComponent<PanelUnderCharacter>().visible = false;
            GameObject.Find("#Hally_Fantasy_Realm_temp").transform.GetChild(0).GetComponent<PanelUnderCharacter>().visible = false;
            GameObject.Find("#Ed_Fantasy_Realm_temp").transform.GetChild(0).GetComponent<PanelUnderCharacter>().visible = true;
            GameObject.Find("#Jade_Fantasy_Realm_temp").transform.GetChild(0).GetComponent<PanelUnderCharacter>().visible = false;
            UnoccupiedSpaceEnable(Board.possibleMoveableChars[theOne]);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            //ClearAllGrids();
            GameObject.Find("#Kent_Fantasy_Realm_temp").transform.GetChild(0).GetComponent<PanelUnderCharacter>().visible = false;
            GameObject.Find("#Meda_Fantasy_Realm_temp").transform.GetChild(0).GetComponent<PanelUnderCharacter>().visible = false;
            GameObject.Find("#Hally_Fantasy_Realm_temp").transform.GetChild(0).GetComponent<PanelUnderCharacter>().visible = false;
            GameObject.Find("#Ed_Fantasy_Realm_temp").transform.GetChild(0).GetComponent<PanelUnderCharacter>().visible = false;
            GameObject.Find("#Jade_Fantasy_Realm_temp").transform.GetChild(0).GetComponent<PanelUnderCharacter>().visible = true;
            UnoccupiedSpaceEnable(Board.possibleMoveableChars[theOne]);
        }
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            GameObject selectedBase;
            
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1)) 
            {

                if(hit.collider.tag == "BlankSpace")
                {
                    Debug.Log("Cleared all highlights when player switched using Alpha keys");
                    ClearAllGrids();
                }
                if (hit.collider.tag == "Player")
                {
                    Debug.Log("Clear cannon popup");
                    
                    GameObject.Find("#Kent_Fantasy_Realm_temp").transform.GetChild(0).GetComponent<PanelUnderCharacter>().visible = false;
                    GameObject.Find("#Meda_Fantasy_Realm_temp").transform.GetChild(0).GetComponent<PanelUnderCharacter>().visible = false;
                    GameObject.Find("#Hally_Fantasy_Realm_temp").transform.GetChild(0).GetComponent<PanelUnderCharacter>().visible = false;
                    GameObject.Find("#Ed_Fantasy_Realm_temp").transform.GetChild(0).GetComponent<PanelUnderCharacter>().visible = false;
                    GameObject.Find("#Jade_Fantasy_Realm_temp").transform.GetChild(0).GetComponent<PanelUnderCharacter>().visible = false;
                    if (selectedUnit != null)
                    {
                        selectedBase = selectedUnit;
                        //look for the image to rotate
                        for (int i = 0; i < selectedUnit.transform.childCount; i++)
                        {
                            if (selectedUnit.transform.GetChild(i).GetComponent<MeshRenderer>() != null)
                            {
                                selectedBase = selectedUnit.transform.GetChild(i).gameObject;
                              
                            }
                        }


                        DisablePanelUnderCharacter(selectedUnit);
                        selectedBase.GetComponent<MeshRenderer>().material = normalMaterial;
                        selectedUnit = null;
                    }                    

                    for (int i = 0; i < Board.possibleMoveableChars.Length; i++)
                    {
                        if (hit.transform == Board.possibleMoveableChars[i].thePiece.transform)
                        {
                            theOne = i;
                            selectedUnit = Board.possibleMoveableChars[i].thePiece;

                            selectedBase = selectedUnit;
                            //look for the image to rotate
                            for (int j = 0; j < selectedUnit.transform.childCount; j++)
                            {
                                if (selectedUnit.transform.GetChild(j).GetComponent<MeshRenderer>() != null)
                                {
                                    selectedBase = selectedUnit.transform.GetChild(j).gameObject; 
                                }
                            }
                            normalMaterial = selectedBase.GetComponent<MeshRenderer>().material;
                            glowingMaterial.color = normalMaterial.color; 
                            glowingMaterial.mainTexture = normalMaterial.mainTexture;
                            selectedBase.GetComponent<MeshRenderer>().material = glowingMaterial;
                            
                            GameObject panelUnderCharacter = null; 
                            for (int j = 0; j < selectedUnit.transform.childCount; j++)
                            {
                                if (selectedUnit.transform.GetChild(j).GetComponent<PanelUnderCharacter>() != null)
                                {
                                    panelUnderCharacter = selectedUnit.transform.GetChild(j).GetComponent<PanelUnderCharacter>().gameObject; 
                                }                                 
                            }
                            if (panelUnderCharacter != null)
                            {                               
                                panelUnderCharacter.GetComponent<PanelUnderCharacter>().visible = true;
                                
                                UnoccupiedSpaceEnable(Board.possibleMoveableChars[theOne]);
                            }
                            break;                            
                        }
                    }
                }
                else
                {
                    if (selectedUnit != null)
                    {
                        //selectedUnit.GetComponent<MeshRenderer>().material = normalMaterial;
                        GameObject panelUnderCharacter = null;
                        for (int i = 0; i < selectedUnit.transform.childCount; i++)
                        {
                            if (selectedUnit.transform.GetChild(i).GetComponent<PanelUnderCharacter>() != null)
                            {
                                panelUnderCharacter = selectedUnit.transform.GetChild(i).GetComponent<PanelUnderCharacter>().gameObject; 
                            }
                        }
                        if (panelUnderCharacter != null)
                        {
                            panelUnderCharacter.GetComponent<PanelUnderCharacter>().visible = false;
                            ClearAllGrids();
                        }
                        selectedUnit = null;
                    }
                }
            }
        }
    }

    public void DisablePanelUnderCharacter(GameObject selected)
    {
        for (int i = 0; i < selectedUnit.transform.childCount; i++)
        {
            if (selected.transform.GetChild(i).GetComponent<PanelUnderCharacter>() != null)
            {
                panelUnderCharacter = selected.transform.GetChild(i).GetComponent<PanelUnderCharacter>().gameObject;
            }
        }

        if (panelUnderCharacter != null)
        {
            ClearAllGrids();
            panelUnderCharacter.GetComponent<PanelUnderCharacter>().visible = false;
        }
    }

    public void MoveCamera()
    {
        if (Input.GetAxis("Horizontal") > 0 && cameraChangeHorizontal < 200.0f)
        {
            for (int i = 0; i < numCameraRotPositions; i++)
            {
                allCameras[i].transform.position += new Vector3(cameraSpeed, 0.0f, 0.0f) * Time.deltaTime;
                cameraChangeHorizontal -= cameraSpeed * Time.deltaTime;
            }
        }
        else if (Input.GetAxis("Horizontal") < 0 && cameraChangeHorizontal > -200.0f)
        {
            for (int i = 0; i < numCameraRotPositions; i++)
            {
                allCameras[i].transform.position -= new Vector3(cameraSpeed, 0.0f, 0.0f) * Time.deltaTime;
                cameraChangeHorizontal += cameraSpeed * Time.deltaTime;
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

   
    public static void UnoccupiedSpaceEnable(Piece character)
    {
        bool isUp  = false;
        bool isRight = false;
        bool isDown = false;
        bool isLeft = false; 
        //Up
        for (int i = 0; i < Board.possibleMoveableChars.Length; i++)
        {
              if (Board.possibleMoveableChars[i] == character)
              {
                  continue;
              }

              if (Board.possibleMoveableChars[i].rowPosition == character.rowPosition - 1 && Board.possibleMoveableChars[i].colPosition == character.colPosition)
              {
                  isUp = true;
              }            
        }
        for (int i = 0; i < Board.spawnedEnemies.Count; i++)
        {
            if (Board.spawnedEnemies[i].rowPosition == character.rowPosition - 1 &&
                Board.spawnedEnemies[i].colPosition == character.colPosition)
            {
                isUp = true;
            }
        }
        //check for dead spaces in the way
        for (int i = 0; i < Board.numDeadSpaces; i++)
        {
            if (Board.deadPoints[i].y == character.rowPosition - 1 &&
                Board.deadPoints[i].x == character.colPosition)
            {
                isUp = true;
            }
        }
        //check for cannons in the way
        for (int i = 0; i < Board.numCannons; i++)
        {
            if (Board.allCannons[i].cannon.rowPosition == character.rowPosition - 1 && Board.allCannons[i].cannon.colPosition == character.colPosition)
            {
                isUp = true;
            }
        }
        for (int i = 0; i < Board.numGenerators; i++)
        {
            if (Board.generators[i].generator.rowPosition == character.rowPosition - 1 && Board.generators[i].generator.colPosition == character.colPosition)
            {
                isUp = true;
            }
        }
        if (Board.pirateBoss.colPosition == character.colPosition && Board.pirateBoss.rowPosition == character.rowPosition - 1)
        {
            isUp = true;
        }
        if (!isUp)
        {
            if (GameObject.Find("gridRow" + (character.rowPosition - 1) + "Column" + character.colPosition) != null)
            {
                GameObject.Find("gridRow" + (character.rowPosition - 1) + "Column" + character.colPosition).transform.GetChild(0).GetComponent<FreeSpaceHighlight>().isVisible = true;
                GameObject.Find("gridRow" + (character.rowPosition - 1) + "Column" + character.colPosition).transform.GetChild(1).GetComponent<FreeSpaceHighlightAnim>().isVisible = true;
                GameObject.Find("gridRow" + (character.rowPosition - 1) + "Column" + character.colPosition).transform.GetChild(2).GetComponent<PossibleMoveCostText>().isVisible = true;

                isUp = false;
            }
        }
        //Right
        for (int i = 0; i < Board.possibleMoveableChars.Length; i++)
        {            
            if (Board.possibleMoveableChars[i] == character)
            {
                continue;
            }

            if (Board.possibleMoveableChars[i].rowPosition == character.rowPosition && Board.possibleMoveableChars[i].colPosition == character.colPosition + 1)
            {
                isRight = true;               
            }
        }
        for (int i = 0; i < Board.spawnedEnemies.Count; i++)
        {
            if (Board.spawnedEnemies[i].colPosition == character.colPosition + 1 && Board.spawnedEnemies[i].rowPosition == character.rowPosition)
            {
                isRight = true;
            }
        }
        //check for dead spaces in the way
        for (int i = 0; i < Board.numDeadSpaces; i++)
        {
            if (Board.deadPoints[i].x == character.colPosition + 1 && Board.deadPoints[i].y == character.rowPosition)
            {
                isRight = true;
            }
        }
        //check for cannons in the way
        for (int i = 0; i < Board.numCannons; i++)
        {
            if (Board.allCannons[i].cannon.colPosition == character.colPosition + 1 && Board.allCannons[i].cannon.rowPosition == character.rowPosition)
            {
                isRight = true;
            }
        }
        //check for generators in the way
        for (int i = 0; i < Board.numGenerators; i++)
        {
            if (Board.generators[i].generator.rowPosition == character.rowPosition && Board.generators[i].generator.colPosition == character.colPosition + 1)
            {
                isRight = true;
            }
        }
        if (Board.pirateBoss.colPosition == character.colPosition + 1 && Board.pirateBoss.rowPosition == character.rowPosition)
        {
            isRight = true;
        }
        if (!isRight)
        {
            if (GameObject.Find("gridRow" + (character.rowPosition) + "Column" + (character.colPosition + 1)) != null)
            {
                GameObject.Find("gridRow" + (character.rowPosition) + "Column" + (character.colPosition + 1)).transform.GetChild(0).GetComponent<FreeSpaceHighlight>().isVisible = true;
                GameObject.Find("gridRow" + (character.rowPosition) + "Column" + (character.colPosition + 1)).transform.GetChild(1).GetComponent<FreeSpaceHighlightAnim>().isVisible = true;
                GameObject.Find("gridRow" + (character.rowPosition) + "Column" + (character.colPosition + 1)).transform.GetChild(2).GetComponent<PossibleMoveCostText>().isVisible = true;
                GameObject.Find("gridRow" + (character.rowPosition) + "Column" + (character.colPosition + 1)).transform.GetComponent<TMPro.TextMeshPro>().text = PlayerControls.moveValues[1].ToString();
                isRight = false;
            }
        }
        //Down
        for (int i = 0; i < Board.possibleMoveableChars.Length; i++)
        {
            if (Board.possibleMoveableChars[i] == character)
            {
                continue;
            }
            if (Board.possibleMoveableChars[i].rowPosition == character.rowPosition + 1 && Board.possibleMoveableChars[i].colPosition == character.colPosition)
            {
                isDown = true;               
            }
        }
        for (int i = 0; i < Board.spawnedEnemies.Count; i++)
        {
            if (Board.spawnedEnemies[i].rowPosition == character.rowPosition + 1 && Board.spawnedEnemies[i].colPosition == character.colPosition)
            {
                isDown = true;
            }
        }
        //check for dead spaces in the way
        for (int i = 0; i < Board.numDeadSpaces; i++)
        {
            if (Board.deadPoints[i].y == character.rowPosition + 1 && Board.deadPoints[i].x == character.colPosition)
            {
                isDown = true;
            }
        }
        //check for cannons in the way
        for (int i = 0; i < Board.numCannons; i++)
        {
            if (Board.allCannons[i].cannon.rowPosition == character.rowPosition + 1 && Board.allCannons[i].cannon.colPosition == character.colPosition)
            {
                isDown = true;
            }
        }
        //check for generators in the way
        for (int i = 0; i < Board.numGenerators; i++)
        {
            if (Board.generators[i].generator.rowPosition == character.rowPosition + 1 && Board.generators[i].generator.colPosition == character.colPosition)
            {
                isDown = true;
            }
        }
        if (Board.pirateBoss.colPosition == character.colPosition && Board.pirateBoss.rowPosition == character.rowPosition + 1)
        {
            isDown = true;
        }
        if (!isDown)
        {
            if (GameObject.Find("gridRow" + (character.rowPosition + 1) + "Column" + (character.colPosition)) != null)
            {
                GameObject.Find("gridRow" + (character.rowPosition + 1) + "Column" + (character.colPosition)).transform.GetChild(0).GetComponent<FreeSpaceHighlight>().isVisible = true;
                GameObject.Find("gridRow" + (character.rowPosition + 1) + "Column" + (character.colPosition)).transform.GetChild(1).GetComponent<FreeSpaceHighlightAnim>().isVisible = true;
                GameObject.Find("gridRow" + (character.rowPosition + 1) + "Column" + (character.colPosition)).transform.GetChild(2).GetComponent<PossibleMoveCostText>().isVisible = true;
                isDown = false;
            }
        }
        //Left
        for (int i = 0; i < Board.possibleMoveableChars.Length; i++)
        {            
            if (Board.possibleMoveableChars[i] == character)
            {
                continue;
            }
            if (Board.possibleMoveableChars[i].rowPosition == character.rowPosition && Board.possibleMoveableChars[i].colPosition == character.colPosition - 1)
            {
                isLeft = true;                
            }
        }
        for (int i = 0; i < Board.spawnedEnemies.Count; i++)
        {
            if (Board.spawnedEnemies[i].colPosition == character.colPosition - 1 && Board.spawnedEnemies[i].rowPosition == character.rowPosition)
            {
                isLeft = true;
            }
        }
        //check for dead spaces in the way
        for (int i = 0; i < Board.numDeadSpaces; i++)
        {
            if (Board.deadPoints[i].x == character.colPosition - 1 && Board.deadPoints[i].y == character.rowPosition)
            {
                isLeft = true;
            }
        }
        //check for cannons in the way
        for (int i = 0; i < Board.numCannons; i++)
        {
            if (Board.allCannons[i].cannon.colPosition == character.colPosition - 1 && Board.allCannons[i].cannon.rowPosition == character.rowPosition)
            {
                isLeft = true;
            }
        }
        //check for generators in the way
        for (int i = 0; i < Board.numGenerators; i++)
        {
            if (Board.generators[i].generator.rowPosition == character.rowPosition && Board.generators[i].generator.colPosition == character.colPosition - 1)
            {
                isLeft = true;
            }
        }
        if (Board.pirateBoss.colPosition == character.colPosition - 1 && Board.pirateBoss.rowPosition == character.rowPosition)
        {
            isLeft = true;
        }
        if (!isLeft)
        {
            if (GameObject.Find("gridRow" + (character.rowPosition) + "Column" + (character.colPosition - 1)) != null)
            {
                GameObject.Find("gridRow" + (character.rowPosition) + "Column" + (character.colPosition - 1)).transform.GetChild(0).GetComponent<FreeSpaceHighlight>().isVisible = true;
                GameObject.Find("gridRow" + (character.rowPosition) + "Column" + (character.colPosition - 1)).transform.GetChild(1).GetComponent<FreeSpaceHighlightAnim>().isVisible = true;
                GameObject.Find("gridRow" + (character.rowPosition) + "Column" + (character.colPosition - 1)).transform.GetChild(2).GetComponent<PossibleMoveCostText>().isVisible = true;
                isLeft = false;
            }
        }
    }

    public static void ClearAllGrids()
    {
        for (int i = 0; i < Board.allTiles.Length; i++)
        {
            if (Board.allTiles[i].tag == "BlankSpace")
            {
                Board.allTiles[i].transform.GetChild(0).GetComponent<FreeSpaceHighlight>().isVisible = false;                
                Board.allTiles[i].transform.GetChild(1).GetComponent<FreeSpaceHighlightAnim>().isVisible = false;
                Board.allTiles[i].transform.GetChild(2).GetComponent<PossibleMoveCostText>().isVisible = false;
            }
        }
    }

    public static void EndButtonEnable()
    { 
        for(int i = 0; i < moveValues.Length; i++)
        {
            if (moveValues[i] > ExperimentalResources.resources || ExperimentalResources.resources == 0)
            {
               // GameObject.Find("EndTurn").transform.GetComponent<EndButtonToggle>().isVisible = true;
            }
        }        
    }
}
