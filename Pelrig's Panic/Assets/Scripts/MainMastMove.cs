using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMastMove : MonoBehaviour {

    public GameObject mainMast;

    public float speed = 1.0f;
    public float amount = 1.0f;

    Transform shake = mainMast.transform;
    // Use this for initialization
    void Start () {
        //mainMast = GameObject.FindWithTag("MainMast");
    }
	
	// Update is called once per frame
	void Update () {
        
         shake.position.x = Mathf.Sin(Time.time * speed) * amount;
	}

    /*public void MoveMastWhenHit()
    {

    }*/
}
