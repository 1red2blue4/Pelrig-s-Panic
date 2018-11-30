﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CharacterPanel : MonoBehaviour
{
    public bool isEnable;
	// Use this for initialization
	void Start ()
    {
        isEnable = false;	
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(isEnable)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
	}
}