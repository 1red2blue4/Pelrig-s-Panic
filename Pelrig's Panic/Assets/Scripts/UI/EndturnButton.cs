using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndturnButton : MonoBehaviour
{
    [SerializeField]
    private GameObject endButton;

    public static bool isEnable;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!DialoguePanelManager.playerControlsLocked && !TutorialCards.isTutorialRunning)
        {
            endButton.SetActive(true);
        }
        else
        {
            endButton.SetActive(false);
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

    public static void EnableEndTurn()
    {
        if (!isEnable)
        {
            EndTurnButtonScript.isButtonPressed = true;
            isEnable = true;
            GameObject.Find("EndTurn").GetComponent<Image>().enabled = true;
        }

    }

    public static void DisableEndTurn()
    {
        if (isEnable)
        {
            EndTurnButtonScript.isButtonPressed = false;
            isEnable = false;
            GameObject.Find("EndTurn").GetComponent<Image>().enabled = false;
        }
    }
}
