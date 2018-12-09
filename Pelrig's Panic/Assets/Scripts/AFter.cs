using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AFter : MonoBehaviour {

    private float timer;
    private float randNum;

	// Use this for initialization
	void Start ()
    {
        timer = 0.0f;
        randNum = Random.Range(5.0f, 15.0f);
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (timer >= randNum)
        {
            SceneManager.LoadScene("PirateShipWithBoard");
        }
        timer += Time.deltaTime;
	}
}