using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealWorldMovement : MonoBehaviour {

    [SerializeField]
    private int movementSpeed;

    [SerializeField]
    private int rotationSpeed;

    public bool isSelected;

    private void Start()
    {
        isSelected = false;
    }

    void FixedUpdate()
    {
        //Make player characters move only if they are selected
        if (isSelected)
        {
            if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
            {
                float movement = Input.GetAxis("Vertical") * Time.deltaTime * movementSpeed;
                float rotation = Input.GetAxis("Horizontal") * Time.deltaTime * rotationSpeed;

                transform.Rotate(0, rotation, 0);
                transform.Translate(0, 0, movement);
            }
        }
    }
}
