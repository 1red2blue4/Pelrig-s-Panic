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

                //temp = Board.possibleMoveableChars[i].thePiece.GetComponent<Stats>().canAttack;
                 
                //if(temp)
                //{
                //    if (Board.possibleMoveableChars[i].thePiece.name == gameObject.transform.GetChild(i).name)
                //    {
                        healthValue = Board.possibleMoveableChars[i].thePiece.GetComponent<Stats>().health;
                        presenceTextArr[i].text = healthValue.ToString();

                        attackValue = Board.possibleMoveableChars[i].thePiece.GetComponent<Stats>().damage;
                        resistTextArr[i].text = attackValue.ToString();
                //    }
                //}
                   
            }
        }
    }
}
