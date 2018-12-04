using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PossibleMoveCostText : MonoBehaviour
{
    public bool isVisible;
    int movementcost;
	// Use this for initialization
	void Start ()
    {
        isVisible = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(isVisible)
        {
            transform.GetComponent<TMPro.TextMeshPro>().enabled = true;
            //transform.GetComponent<TMPro.TextMeshPro>().text = PlayerControls.moveValues[movementcost].ToString();
        }
        else
        {
            transform.GetComponent<TMPro.TextMeshPro>().enabled = false;
            transform.GetComponent<TMPro.TextMeshPro>().text = null;
        }
	}
}
