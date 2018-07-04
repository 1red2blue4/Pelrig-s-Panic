using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour {


    private int clickedRow;
    private int clickedColumn;
    private Camera thisCamera;
    
	void Start ()
    {
        thisCamera = gameObject.GetComponent<Camera>();
        MovementManager.directionLineup = new MovementManager.Direction[50];
        MovementManager.SetStartDirectionLineup();
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        CheckClick();
	}



    public void CheckClick()
    {

        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (Input.GetMouseButtonDown(0))
        {
            if (hit.collider != null)
            {
                Transform objectHit = hit.transform;
                for (int i = 0; i < Board.gamePieceObjects.Length; i++)
                {
                    if (objectHit == Board.gamePieceObjects[i].transform)
                    {
                        MovementManager.Move(Board.possibleMoveableChars[i]);
                    }
                }
            }
        }
    }
}
