using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelection : MonoBehaviour
{
    private Animator anim;
    public bool isSelected;
	// Use this for initialization
	void Start ()
    {
         
	}
	
	// Update is called once per frame
	void Update ()
    { 
        if(isSelected)
        {
            //anim.SetBool("playerSelectionAnim", true);
           // anim.Play("PlayerSelection"); 
        }
        else
        {
           // anim.SetBool("playerSelectionAnim", false);

        }
	}
}
