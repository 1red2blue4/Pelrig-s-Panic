using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PirateShipAnimatic : MonoBehaviour
{
    private float timer;

	// Use this for initialization
	void Start ()
    {
	    	
	}
	
	// Update is called once per frame
	void Update ()
    {
        timer += Time.deltaTime;
        if (timer < 5.0f)
        {
            transform.Rotate(0, 9f * Time.deltaTime, 0);
        }
        else if (timer >= 5.0f && timer < 10.0f)
        {
            transform.Rotate(-2.5f * Time.deltaTime, 0, 0);
        }
        else if (timer >= 10.0f)
        {
            transform.Rotate(0, 15.0f * Time.deltaTime, 0);
        }
	}
}
