using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterMovement : MonoBehaviour {

    float startX;
    [SerializeField] bool movingLeft;
    [SerializeField] float displacement;

	// Use this for initialization
	void Start () {
        startX = transform.position.x;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 newPos = transform.position;
        if (movingLeft)
        {
            newPos.x += 1.5f * Time.deltaTime;
        }
        else
        {
            newPos.x -= 1.5f * Time.deltaTime;
        }
        transform.position = newPos;
        if (Mathf.Abs(transform.position.x - startX) > displacement )
        {
            movingLeft = !movingLeft;
        }
        
	}
}
