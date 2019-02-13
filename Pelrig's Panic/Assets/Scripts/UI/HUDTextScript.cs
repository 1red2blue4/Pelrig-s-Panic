using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDTextScript : MonoBehaviour
{
    [SerializeField] Text[] presenceTextArr;
    [SerializeField] Text[] resistTextArr;

    [SerializeField] RectTransform[] dreamMeterBarArr;

    private float startingWidth;

    private int meterValue;
    private int healthValue;
    private int attackValue;
    private bool temp;

    [SerializeField]
    private GameObject[] characterPortrait;

    public static bool isVisible;

    // Use this for initialization
    void Start ()
    {
        if (dreamMeterBarArr[0] != null)
        {
            startingWidth = dreamMeterBarArr[0].rect.width;
        }

        for (int i = 0; i < dreamMeterBarArr.Length; i++)
        {
            dreamMeterBarArr[i].sizeDelta = new Vector2(0.0f, dreamMeterBarArr[i].rect.height);
         }
    }
	
	// Update is called once per frame
	void Update ()
    {       
        UpdateText();
        UpdateMeter();
    }

    void UpdateText()
    {       
        for (int i = 0; i < Board.possibleMoveableChars.Length; i++)
        {
            attackValue = Board.possibleMoveableChars[i].thePiece.GetComponent<Stats>().damage;
            presenceTextArr[i].text = attackValue.ToString();

            healthValue = Board.possibleMoveableChars[i].thePiece.GetComponent<Stats>().health;
            resistTextArr[i].text = healthValue.ToString();                   
        }

        if(!DialoguePanelManager.playerControlsLocked && !TutorialCards.isTutorialRunning)
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

    void UpdateMeter()
    {
        for (int i = 0; i < Board.possibleMoveableChars.Length; i++)
        {
            meterValue = Board.possibleMoveableChars[i].thePiece.GetComponent<Stats>().meterUnitsFilled;
            dreamMeterBarArr[i].sizeDelta = new Vector2(startingWidth * meterValue / Board.possibleMoveableChars[i].thePiece.GetComponent<Stats>().maxMeter, dreamMeterBarArr[i].rect.height);
            dreamMeterBarArr[i].GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 2, dreamMeterBarArr[i].rect.width);
        }
    }
}
