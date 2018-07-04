using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour {

    private int rowPosition;
    private int colPosition;
    [SerializeField] private string name;
    private PieceType type;
    [SerializeField] private GameObject thePiece;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void SetRowAndCol(int row, int col)
    {
        rowPosition = row;
        colPosition = col;
    }

    public void SetName(string nm)
    {
        name = nm;
    }

    public string GetName()
    {
        return name;
    }

    public GameObject GetPiece()
    {
        return thePiece;
    }
}
