using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Temp : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnMouseOver()
    {
        Debug.Log("Button mouse over");
        gameObject.GetComponent<Image>().enabled = false;
    }
    void OnMouseExit()
    {
        gameObject.GetComponent<Image>().enabled = false;
    }
}
