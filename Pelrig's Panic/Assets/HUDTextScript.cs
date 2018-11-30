using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDTextScript : MonoBehaviour
{
    [SerializeField] Text[] presenceTextArr;
    [SerializeField] Text[] resistTextArr;
    
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
        for (int i = 0; i < Board.possibleMoveableChars.Length; i++)
        {
            presenceTextArr[i].text = Board.possibleMoveableChars[i].presenceValue.ToString();
            resistTextArr[i].text = Board.possibleMoveableChars[i].resistanceValue.ToString();            
        }
    }
}
