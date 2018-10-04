using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealWorldCamera : MonoBehaviour {

    //For focus
    public GameObject selectedUnit;
    [SerializeField] public float cameraSpeed;
    private Vector3 front;
    private Vector3 right;

    Vector3 groundCamOffset;
    Vector3 camTarget;
    Vector3 camSmoothDampV;
    Camera cam;

    // Use this for initialization
    void Awake () {
        //Forward with respect to the camera and not the scene
        front = GetComponent<Camera>().transform.forward;
        front.y = 0;
        front = Vector3.Normalize(front);
        // To get the right and left directional sense
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * front;
        selectedUnit = null;

        //Camera offsetting for focusing on the selected unit
        cam = GetComponent<Camera>();
        Vector3 groundPos = GetWorldPosAtViewportPoint(0.5f, 0.5f);
        groundCamOffset = cam.transform.position - groundPos;
        camTarget = cam.transform.position;
    }

    private Vector3 GetWorldPosAtViewportPoint(float vx, float vy)
    {
        Ray worldRay = cam.ViewportPointToRay(new Vector3(vx, vy, 0));
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float distanceToGround;
        groundPlane.Raycast(worldRay, out distanceToGround);
        return worldRay.GetPoint(distanceToGround);
    }

    // Update is called once per frame
    void Update()
    {
        //Zoom in and out
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            float cameraOrthoSize = GetComponent<Camera>().orthographicSize - Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime;
            cameraOrthoSize = (cameraOrthoSize > 20.0f ? 20.0f : cameraOrthoSize);
            cameraOrthoSize = (cameraOrthoSize < 2.5f ? 2.5f : cameraOrthoSize);
            GetComponent<Camera>().orthographicSize = cameraOrthoSize;
        }

        //Move camera with Arrow keys
        //Camera movement across the map
        if (Input.GetAxis("HorizontalCamera") != 0 || Input.GetAxis("VerticalCamera") != 0)
        {
            Vector3 horizontalMovement = right * cameraSpeed * Time.deltaTime * Input.GetAxis("HorizontalCamera");
            Vector3 verticalMovement = front * cameraSpeed * Time.deltaTime * Input.GetAxis("VerticalCamera");
        
            transform.position += (horizontalMovement + verticalMovement);

            //unselect the unit as camera is not locked
            selectedUnit = null;
        }

        //Lock camera
        if (selectedUnit != null)
        {
            // Center whatever position is clicked
            transform.position = Vector3.SmoothDamp(cam.transform.position, selectedUnit.transform.position + groundCamOffset, ref camSmoothDampV, 0.2f);
        }
    }
}
