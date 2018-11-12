using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndButtonToggle : MonoBehaviour
{
    public bool isVisible;
    private Button tempBtn;
    // Use this for initialization
    void Start ()
    {
        isVisible = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (isVisible)
        {
            gameObject.GetComponent<Image>().enabled = true;
        }
        else
        {
            gameObject.GetComponent<Image>().enabled = false; 
        }		
	}
}
