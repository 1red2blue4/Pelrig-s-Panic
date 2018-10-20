using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTurnButtonScript : MonoBehaviour
{
    //When EndTurn button is presed
    public static bool isButtonPressed = false;
    Button button;

    private void Start()
    {
        button = GameObject.Find("GridLevelStuff").GetComponentInChildren<Button>();
    }
    public void OnEndTurn()
    {
        if (PlayerControls.isPlayerTurn)
        {
            isButtonPressed = true;
        }
    }

    private void Update()
    {
        if (!PlayerControls.isPlayerTurn)
        {
            if (button.interactable == true)
            {
                button.interactable = false;
            }
        }
        else
        {
            if (button.interactable == false)
            {
                button.interactable = true;
            }
        }
    }

}
