using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class YouWin : MonoBehaviour {

    GameObject child;
    public bool youWon;
    public bool youLose;
    public static int roundCount;
    Board boardObject;
    public static bool noChange;
    static bool checkChange;
    static bool playerTurn;
	// Use this for initialization
	void Start () {
        roundCount = 0;
        child = transform.GetChild(0).gameObject;
        child.SetActive(false);
        youWon = false;
        youLose = false;
        boardObject = GameObject.Find("GridCreator").GetComponentInChildren<Board>();

	}
	
	// Update is called once per frame
	void Update ()
    {        
        if (youLose)
        {
            SceneManager.LoadScene("FantasyWorldEndScene");
            child.SetActive(true);
            gameObject.GetComponentInChildren<TextMesh>().text = "YOU LOSE";
        }
		else if (youWon || roundCount >= 2)
        {
            child.SetActive(true);
        }

        if (checkChange)
        {

        }
	}
    public static void RoundCount()
    {
        if (!checkChange)
        {
            playerTurn = PlayerControls.isPlayerTurn;
            checkChange = true;
            roundCount++;
        }
    }
}