using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealWorldCharacterSelectionAndMovement : MonoBehaviour {

    GameObject[] controllaleCharacters;
    int characterSlected;

    [SerializeField] float characterMovement;
    private Vector3 front;
    private Vector3 right;

    // Use this for initialization
    void Start () {
        controllaleCharacters = GameObject.FindGameObjectsWithTag("Player");
        characterSlected = 0;

        front = GameObject.FindGameObjectWithTag("MainCamera").transform.forward;
        front.y = 0;
        front = Vector3.Normalize(front);
        // To get the right and left directional sense
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * front;
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Tab))
        { 
            characterSlected++;
            characterSlected = characterSlected >= controllaleCharacters.Length ? 0 : characterSlected;
        }

        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            Vector3 horizontalMovement = right * characterMovement * Time.deltaTime * Input.GetAxis("Horizontal");
            Vector3 verticalMovement = front * characterMovement * Time.deltaTime * Input.GetAxis("Vertical");

            controllaleCharacters[characterSlected].transform.position += (horizontalMovement + verticalMovement);
        }
    }
}
