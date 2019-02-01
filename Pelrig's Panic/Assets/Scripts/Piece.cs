using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{

    public string id;
    public int rowPosition;
    public int colPosition;
    [SerializeField] private string name;
    public int presenceValue;
    public int resistanceValue;
    public int initialPresenceValue;
    public int initialResistanceValue;
    public int health;
    public int attack;
    public Vector3 currPos;
    public Vector3 prevPos;
    [SerializeField] private float scootSpeed;

    private PieceType type;
    [SerializeField] public GameObject thePiece;
    float timer = 0.0f;

    private void Start()
    {
        presenceValue = initialPresenceValue;
        resistanceValue = initialResistanceValue;
    }

    public void TakeDamage(int hit)
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
        if (prevPos != currPos)
        {
            MoveToNewPos();
        }
    }

    private void MoveToNewPos()
    {
        Vector3 moveDirection = currPos - prevPos;
        moveDirection.Normalize();
        thePiece.transform.position += moveDirection * Time.deltaTime * scootSpeed;
        prevPos = thePiece.transform.position;
        Vector3 newMoveDirection = currPos - prevPos;
        newMoveDirection.Normalize();
        //you've gone too far
        if (newMoveDirection != moveDirection)
        {
            thePiece.transform.position = currPos;
            prevPos = currPos;
        }
    }
}
