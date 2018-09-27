using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealWorldCamera : MonoBehaviour {

    GameObject selectedUnit;
    [SerializeField] public float cameraSpeed;
    private Vector3 front;
    private Vector3 right;

    Vector3 groundCamOffset;
    Vector3 camTarget;
    Vector3 camSmoothDampV;
    Camera cam;

    // Use this for initialization
    void Start () {
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
        Debug.Log("groundPos: " + groundPos);
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
            GetComponent<Camera>().orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime;
            if (GetComponent<Camera>().orthographicSize < 2.0f)
            {
                GetComponent<Camera>().orthographicSize = 2.0f;
            }
            else if (GetComponent<Camera>().orthographicSize > 20.0f)
            {
                GetComponent<Camera>().orthographicSize = 20.0f;
            }
        }

        //Move camera with WASD when not selected any character
        if (selectedUnit == null)
        {
            //Camera movement across the map
            if (Input.GetAxis("HorizontalCamera") != 0 || Input.GetAxis("VerticalCamera") != 0)
            {
                Vector3 horizontalMovement = right * cameraSpeed * Time.deltaTime * Input.GetAxis("HorizontalCamera");
                Vector3 verticalMovement = front * cameraSpeed * Time.deltaTime * Input.GetAxis("VerticalCamera");

                transform.position += (horizontalMovement + verticalMovement);
            }
        }
        //Make camera follow the selected unit
        else
        {
            // Center whatever position is clicked
            transform.position = Vector3.SmoothDamp(cam.transform.position, selectedUnit.transform.position + groundCamOffset, ref camSmoothDampV, 0.4f);
        }
        //Select the character to control
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.transform.tag == "Player")
                {
                    //Change selected target, and switch their control scheme
                    if (selectedUnit != null)
                    {
                        selectedUnit.GetComponent<RealWorldMovement>().isSelected = false;
                    }
                    selectedUnit = hit.collider.gameObject;
                    selectedUnit.GetComponent<RealWorldMovement>().isSelected = true;
                    print(selectedUnit.name);

                    float mouseX = Input.mousePosition.x / cam.pixelWidth;
                    float mouseY = Input.mousePosition.y / cam.pixelHeight;
                    Vector3 clickPt = GetWorldPosAtViewportPoint(mouseX, mouseY);
                }
            }
        }

        //Unselect a selected camera
        else if (Input.GetMouseButtonDown(1))
        {
            if (selectedUnit != null)
            {
                selectedUnit.GetComponent<RealWorldMovement>().isSelected = false;
                print("Nothing");
                selectedUnit = null;
            }
        }
                
    }
}
