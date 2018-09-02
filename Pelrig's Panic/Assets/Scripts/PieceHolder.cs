using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceHolder : MonoBehaviour {

    public Piece[] allPieces;
    public Piece[] allCoins;
    public Piece[] allCharacters;

    void Start()
    {
        allCharacters = Board.possibleMoveableChars;
        allPieces = new Piece[allCoins.Length + allCharacters.Length];
        for (int i = 0; i < allPieces.Length; i++)
        {
            if (i < allCharacters.Length)
            {
                allPieces[i] = allCharacters[i];
            }
            else
            {
                allPieces[i] = allCoins[i - allCharacters.Length];
            }
        }
    }
}
