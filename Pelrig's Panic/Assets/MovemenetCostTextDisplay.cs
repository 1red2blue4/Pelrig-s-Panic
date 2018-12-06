using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovemenetCostTextDisplay : MonoBehaviour
{
    public static bool isCostText;
    private Text costText; 
	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(isCostText)
        {
            if(PlayerControls.isLeft)
            {
                gameObject.SetActive(true);
                costText.text = "50";
            }
        }
        else
        {

        }
	}
}
