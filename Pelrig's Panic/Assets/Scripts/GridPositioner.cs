using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPositioner : MonoBehaviour {

    public float difference;
    public float speed;

    public void CheckWhatsBeneath()
    {
        RaycastHit hit;
        Ray rayResult = new Ray(transform.position, new Vector3(0.0f, 0.0f, 1.0f));
        if (Physics.Raycast(rayResult, out hit, Mathf.Infinity))
        {
            float zChange = hit.collider.transform.position.z - transform.position.z;
            difference = zChange;
            speed = 0;
        }
    }

    public void GuideToObjectBeneath(float distanceToMove)
    {
        if (difference > 0)
        {
            transform.position += new Vector3(0.0f, 0.0f, distanceToMove);
            difference -= distanceToMove + speed;
            speed += distanceToMove * 0.05f;
        }
    }
}
