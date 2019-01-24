using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EndButtonToggle : MonoBehaviour
{
    public static bool isEnable;

    [SerializeField]
    private GameObject endButton;
    // Use this for initialization
    void Start ()
    {
        isEnable = false; 
	}
	
	// Update is called once per frame
	void Update ()
    {
       if(!DialoguePanelManager.playerControlsLocked && !TutorialCards.isTutorialRunning)
        {
            endButton.SetActive(true);
        }
       else
        {
            endButton.SetActive(false);
        }
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
        if (!isEnable && !DialoguePanelManager.playerControlsLocked)
        {
            EndTurnButtonScript.isButtonPressed = true;
            isEnable = true;
        }
    }
}
