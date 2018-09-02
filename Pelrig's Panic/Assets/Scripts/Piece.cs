using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour {

    public string id;
    public int rowPosition;
    public int colPosition;
    [SerializeField] private string name;
    private PieceType type;
    [SerializeField] public GameObject thePiece;

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
