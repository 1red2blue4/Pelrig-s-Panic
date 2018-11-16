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

    public void EnableEndTurn()
    {
        
        if(!isEnable)
        {
            Debug.Log("End turn button pressed");
            PlayerControls.EnemyTurnsActivate();
            isEnable = true;
        }       
    }
}
