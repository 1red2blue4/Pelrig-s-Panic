using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlsRealWorld : MonoBehaviour {

    private float cameraSpeed;
    private float cameraScrollSpeed;
    private float cameraMaxZoom;
    private float cameraMinZoom;
    //in place in case this script is attached to another object that is not a camera
    private Camera thisCamera;
    
	void Start ()
    {
        cameraSpeed = 20.0f;
        cameraScrollSpeed = 20.0f;
        cameraMaxZoom = 11.0f;
        cameraMinZoom = 3.0f;
        thisCamera = gameObject.GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        MoveCamera();
	}


    public void MoveCamera()
    {
        if (Input.GetAxis("Horizontal") > 0)
        {
            transform.position += new Vector3(cameraSpeed, 0.0f, 0.0f) * Time.deltaTime;
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            transform.position -= new Vector3(cameraSpeed, 0.0f, 0.0f) * Time.deltaTime;
        }
        if (Input.GetAxis("Vertical") > 0)
        {
            transform.position += new Vector3(0.0f, cameraSpeed, 0.0f) * Time.deltaTime;
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            transform.position -= new Vector3(0.0f, cameraSpeed, 0.0f) * Time.deltaTime;
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && gameObject.GetComponent<Camera>().orthographicSize > cameraMinZoom)
        {
            gameObject.GetComponent<Camera>().orthographicSize -= cameraScrollSpeed * Time.deltaTime;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && gameObject.GetComponent<Camera>().orthographicSize < cameraMaxZoom)
        {
            gameObject.GetComponent<Camera>().orthographicSize += cameraScrollSpeed * Time.deltaTime;
        }
    }
}
