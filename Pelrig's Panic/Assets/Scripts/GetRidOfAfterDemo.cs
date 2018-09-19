using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GetRidOfAfterDemo : MonoBehaviour {

    public float timer;

	// Use this for initialization
	void Start () {
        timer = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if (timer >= 10)
        {
            SceneManager.LoadScene("PirateShipWithBoard");
        }
	}
}
