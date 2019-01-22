using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDTextScript : MonoBehaviour
{
    [SerializeField] Text[] presenceTextArr;
    [SerializeField] Text[] resistTextArr;

    private int healthValue;
    private int attackValue;
    private bool temp;

    [SerializeField]
    private GameObject[] characterPortrait;

    public static bool isVisible;

    // Use this for initialization
    void Start ()
    {
       
    }
	
	// Update is called once per frame
	void Update ()
    {       
        UpdateText();        
    }

    void UpdateText()
    {       
        foreach (var item in Board.possibleMoveableChars)
        {
            for (int i = 0; i < Board.possibleMoveableChars.Length; i++)
            {
                healthValue = Board.possibleMoveableChars[i].thePiece.GetComponent<Stats>().health;
                presenceTextArr[i].text = healthValue.ToString();

                attackValue = Board.possibleMoveableChars[i].thePiece.GetComponent<Stats>().damage;
                resistTextArr[i].text = attackValue.ToString();                   
            }
        }

        if(!PanelManager.playerControlsLocked && !TutorialCards.isTutorialRunning)
        {
            for(int i = 0; i < characterPortrait.Length; i++)
            {
                characterPortrait[i].SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < characterPortrait.Length; i++)
            {
                characterPortrait[i].SetActive(false);
            }
        }
    }
}
