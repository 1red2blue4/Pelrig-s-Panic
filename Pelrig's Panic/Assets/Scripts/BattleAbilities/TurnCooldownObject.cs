using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnCooldownObject : MonoBehaviour
{
    public int turn, maxTurn;


    // Start is called before the first frame update
    void Start()
    {
        maxTurn = 4;
        turn = maxTurn;
    }

    // Update is called once per frame
    void Update()
    {
        if (EndTurnButtonScript.isButtonPressed == true && turn > 0)
        {
            turn--;
        }
        else if (turn == 0 && EndTurnButtonScript.isButtonPressed == true)
        {
            turn = maxTurn;
        }

        Debug.Log(turn);
    }
}
