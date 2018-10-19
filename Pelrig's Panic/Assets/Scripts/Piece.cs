using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{

    public string id;
    public int rowPosition;
    public int colPosition;
    [SerializeField] private string name;
    private PieceType type;
    [SerializeField] public GameObject thePiece;
    float timer = 0.0f;

    private void Start()
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

    private void Update()
    {
        if (transform.tag == "Player")
        {
            bool isColumnRight = false;
            bool isRowRight = false;
            for (int i = 7; i < 10; i++)
            {
                if (rowPosition == i)
                    isRowRight = true;
            }
            for (int i = 3; i < 7; i++)
            {
                if (colPosition == i)
                    isColumnRight = true;
            }

            if (isColumnRight && isRowRight)
            {
                timer += Time.deltaTime;
                if (timer >= 30.0f)
                {
                    GameObject.Find("WinScreen").GetComponentInChildren<YouWin>().youWon = true;
                }
            }
            else
            {
                timer = 0.0f;
            }
        }
    }
}
