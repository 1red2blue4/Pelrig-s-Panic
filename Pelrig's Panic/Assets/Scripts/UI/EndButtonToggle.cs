using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EndButtonToggle : MonoBehaviour
{
    public static bool isEnable; 
    // Use this for initialization
    void Start ()
    {
        isEnable = false; 
	}
	
	// Update is called once per frame
	void Update ()
    {
       
	}

    public static void EnableEndTurn()
    { 
        if(!isEnable)
        { 
            EndTurnButtonScript.isButtonPressed = true;
            isEnable = true;
        }       
    }

    public static void DisableEndTurn()
    {
        if (isEnable)
        {
            EndTurnButtonScript.isButtonPressed = false;
            isEnable = false;
        }
    }

    public void LocalEnableEndTurn()
    {
        if (!isEnable)
        {
            EndTurnButtonScript.isButtonPressed = true;
            isEnable = true;
        }
    }
}
