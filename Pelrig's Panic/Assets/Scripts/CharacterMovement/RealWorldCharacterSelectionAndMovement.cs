using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealWorldCharacterSelectionAndMovement : MonoBehaviour {

    GameObject[] controllaleCharacters;
    int characterSlected;

    float characterMovement;
    private Vector3 front;
    private Vector3 right;
    RealWorldCamera cam;

    [SerializeField] Material glowingMaterial;
    [SerializeField] Material normalMaterial;

    // Use this for initialization
    void Start () {
        controllaleCharacters = GameObject.FindGameObjectsWithTag("Player");
        controllaleCharacters[0].GetComponent<MeshRenderer>().material = glowingMaterial;
        characterSlected = 0;
        characterMovement = 7.0f;
        cam = GetComponent<Camera>().GetComponent<RealWorldCamera>();
        front = GameObject.FindGameObjectWithTag("MainCamera").transform.forward;
        front.y = 0;
        front = Vector3.Normalize(front);
        // To get the right and left directional sense
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * front;
    }
	
	// Update is called once per frame
	void Update () {
        //Select the character to lock on to, movement and camera
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.transform.tag == "Player")
                {
                    for (int i = 0; i < controllaleCharacters.Length; i++)
                    {
                        if (controllaleCharacters[i] == hit.transform.gameObject)
                        {
                            if (i != characterSlected)
                            {
                                controllaleCharacters[characterSlected].GetComponent<MeshRenderer>().material = normalMaterial;
                                characterSlected = i;
                                controllaleCharacters[characterSlected].GetComponent<MeshRenderer>().material = glowingMaterial;
                            }
                            cam.selectedUnit = controllaleCharacters[i];
                            break;
                        }
                    }
                }
            }
        }
        //Switch characters
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            controllaleCharacters[characterSlected].GetComponent<MeshRenderer>().material = normalMaterial;
            characterSlected++;
            characterSlected = characterSlected >= controllaleCharacters.Length ? 0 : characterSlected;
            cam.selectedUnit = controllaleCharacters[characterSlected];
            controllaleCharacters[characterSlected].GetComponent<MeshRenderer>().material = glowingMaterial;
        }

        //Movement of the characters
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            float speed = characterMovement;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                speed *= 1.3f;
            }
            Vector3 horizontalMovement = right * speed * Time.deltaTime * Input.GetAxis("Horizontal");
            Vector3 verticalMovement = front * speed * Time.deltaTime * Input.GetAxis("Vertical");

            controllaleCharacters[characterSlected].GetComponent<CharacterController>().Move(horizontalMovement + verticalMovement);
            //controllaleCharacters[characterSlected].GetComponent<Rigidbody>().AddForce(horizontalMovement + verticalMovement);
        }
    }
}
