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
    private Camera thisCamera;
    
	void Start ()
    {
        cameraSpeed = 20.0f;
        cameraScrollSpeed = 20.0f;
        cameraMaxZoom = 11.0f;
        cameraMinZoom = 3.0f;
        thisCamera = gameObject.GetComponent<Camera>();
        MovementManager.directionLineup = new MovementManager.Direction[50];
        MovementManager.SetStartDirectionLineup();
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        CheckClick();
        MoveCamera();
	}



    public void CheckClick()
    {

        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (Input.GetMouseButtonDown(0))
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
}
