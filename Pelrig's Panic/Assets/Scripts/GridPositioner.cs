using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPositioner : MonoBehaviour {

    public Vector3 startPos;
    public float difference;
    public float speed;
    public GameObject mainCamera;

    public void CheckWhatsBeneath()
    {
        RaycastHit hit;
        Ray rayResult = new Ray(transform.position, new Vector3(0.0f, 0.0f, 1.0f));
        if (Physics.Raycast(rayResult, out hit, Mathf.Infinity))
        {
            startPos = transform.position;
            float zChange = hit.collider.transform.position.z - transform.position.z;
            difference = zChange;
            speed = 0;
        }
    }

    public void GuideToObjectBeneath(float distanceToMove)
    {
        //if it's still falling, fall
        if (difference > 0)
        {
            transform.position += new Vector3(0.0f, 0.0f, distanceToMove + speed);
            difference -= distanceToMove + speed;
            speed += distanceToMove * 0.05f;
        }
        //if it's done falling, adjust its position to where it should land
        else
        {
            transform.position += new Vector3(0.0f, 0.0f, difference);
            difference = 0;
        }
    }

    public void AdjustToCamera()
    {
        transform.rotation = mainCamera.transform.rotation;
    }
}
