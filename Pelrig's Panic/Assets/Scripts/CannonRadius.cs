using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonRadius : MonoBehaviour {

    bool cannonSelected;
    List<Cannon> cannons;

    bool isRight = false;
    bool isLeft = false;
    bool isUp = false;
    bool isDown = false;

	// Use this for initialization
	void Start () {
        cannons = new List<Cannon>();
	}
	
	// Update is called once per frame
	void Update () {
        if (PlayerControls.isPlayerTurn)
        {
            cannonSelected = false;
            for(int i = 0; i < Board.allCannons.Length; i++)
            {
                if (Board.allCannons[i].GetComponent<Cannon>().isCanonSelected)
                {
                    cannonSelected = true;
                    isRight = true;
                    isLeft = true;
                    isUp = true;
                    isDown = true;

                    if (isLeft)
                    {
                        
                            GameObject.Find("gridRow" + (Board.allCannons[i].GetComponent<Cannon>().cannon.rowPosition) + "Column" + (Board.allCannons[i].GetComponent<Cannon>().cannon.rowPosition - 5)).transform.GetChild(0).GetComponent<CannonPopup>().isVisible = true;
                            //GameObject.Find("gridRow" + (character.rowPosition) + "Column" + (character.colPosition - 1)).transform.GetChild(1).GetComponent<FreeSpaceHighlightAnim>().isVisible = true;
                       
                    }
                    if (isRight)
                    {
                            GameObject.Find("gridRow" + (Board.allCannons[i].GetComponent<Cannon>().cannon.rowPosition) + "Column" + (Board.allCannons[i].GetComponent<Cannon>().cannon.rowPosition + 5)).transform.GetChild(0).GetComponent<CannonPopup>().isVisible = true;
                            //GameObject.Find("gridRow" + (character.rowPosition) + "Column" + (character.colPosition + 1)).transform.GetChild(1).GetComponent<FreeSpaceHighlightAnim>().isVisible = true;
                        
                    }
                    if (isUp)
                    {
                        
                            GameObject.Find("gridRow" + (Board.allCannons[i].GetComponent<Cannon>().cannon.rowPosition - 5) + "Column" + (Board.allCannons[i].GetComponent<Cannon>().cannon.rowPosition)).transform.GetChild(0).GetComponent<CannonPopup>().isVisible = true;
                            //GameObject.Find("gridRow" + (character.rowPosition - 1) + "Column" + (character.colPosition)).transform.GetChild(1).GetComponent<FreeSpaceHighlightAnim>().isVisible = true;
                        
                    }
                    if (isDown)
                    {
                        
                            GameObject.Find("gridRow" + (Board.allCannons[i].GetComponent<Cannon>().cannon.rowPosition + 5) + "Column" + (Board.allCannons[i].GetComponent<Cannon>().cannon.rowPosition)).transform.GetChild(0).GetComponent<CannonPopup>().isVisible = true;
                            //GameObject.Find("gridRow" + (character.rowPosition + 1) + "Column" + (character.colPosition)).transform.GetChild(1).GetComponent<FreeSpaceHighlightAnim>().isVisible = true;
                        
                    }
                }
            }
        }
	}
}
