﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YouWin : MonoBehaviour {

    GameObject child;
    public bool youWon;
    float gameTime;
    Board boardObject;
	// Use this for initialization
	void Start () {
        gameTime = 0.0f;
        child = transform.GetChild(0).gameObject;
        child.SetActive(false);
        youWon = false;
        boardObject = GameObject.Find("GridCreator").GetComponentInChildren<Board>();
	}
	
	// Update is called once per frame
	void Update () {
        if (gameTime < 600.0f)
        {
            gameTime += Time.deltaTime;
        }
        else
        {
            youWon = true;
        }

        if (boardObject.remainingEnemies <= 0)
        {
            youWon = true;
        }

		if (youWon)
        {
            child.SetActive(true);
        }
	}
}