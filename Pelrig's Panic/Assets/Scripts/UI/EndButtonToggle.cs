//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.EventSystems;

//public class EndButtonToggle : MonoBehaviour
//{
//    public static bool isEnable;

//    [SerializeField]
//    private GameObject endButton;
//    // Use this for initialization
//    void Start ()
//    {
//        isEnable = false; 
//	}
	
//	// Update is called once per frame
//	void Update ()
//    {
//       if(!DialoguePanelManager.playerControlsLocked && !TutorialCards.isTutorialRunning)
//        {
//            endButton.SetActive(true);
//        }
//       else
//        {
//            endButton.SetActive(false);
//        }
//	}

//    public static void EnableEndTurn()
//    { 
//        if(!isEnable)
//        {
//            Debug.Log("enable end turn");
//            //GameObject.Find("EndTurn").GetComponent<Image>().enabled = true;
//            EndTurnButtonScript.isButtonPressed = true;
//            isEnable = true;
//        }       

//    }

//    public static void DisableEndTurn()
//    {
//        if (isEnable)
//        {
//            Debug.Log("Disable end turn");
//            EndTurnButtonScript.isButtonPressed = false;
//            isEnable = false;
//            //GameObject.Find("EndTurn").GetComponent<Image>().enabled = false;
//        }
//    }

//    public void LocalEnableEndTurn()
//    {
//        if (!isEnable && !DialoguePanelManager.playerControlsLocked)
//        {
//            Debug.Log("Enable end turn onclicked");
//            EndTurnButtonScript.isButtonPressed = true;
//            isEnable = true;
//        }
//    }
//}
