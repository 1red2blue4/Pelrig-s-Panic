using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour {

    private float cameraSpeed;
    private float cameraScrollSpeed;
    private float cameraMaxZoom;
    private float cameraMinZoom;
    private int clickedRow;
    private int clickedColumn;
    private GameObject columnHighlight;
    //in place in case this script is attached to another object that is not a camera
    private Camera thisCamera;
    
	void Start ()
    {
        cameraSpeed = 20.0f;
        cameraScrollSpeed = 20.0f;
        cameraMaxZoom = 11.0f;
        cameraMinZoom = 3.0f;
        thisCamera = gameObject.GetComponent<Camera>();
        columnHighlight = GameObject.FindGameObjectWithTag("ColumnHighlight");
        MovementManager.Setup();
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        CheckClick();
        MoveCamera();
        CheckCoinCollect();
        CheckForLineupSwap();
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
