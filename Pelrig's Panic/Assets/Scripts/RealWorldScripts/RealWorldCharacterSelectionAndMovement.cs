using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealWorldCharacterSelectionAndMovement : MonoBehaviour {

    GameObject[] controllableCharacters;
    int characterSlected;

    float characterMovement;
    private Vector3 front;
    private Vector3 right;
    RealWorldCamera cam;

    [SerializeField] Material glowingMaterial;
    Material normalMaterial;

    // Use this for initialization
    void Start () {
        characterMovement = 6.0f;

        controllableCharacters = GameObject.FindGameObjectsWithTag("Player");
        cam = GetComponent<Camera>().GetComponent<RealWorldCamera>();
        front = GetComponent<Camera>().transform.forward;

        normalMaterial = controllableCharacters[0].GetComponent<MeshRenderer>().material;
        glowingMaterial.mainTexture = normalMaterial.mainTexture;
        glowingMaterial.color = normalMaterial.color;
        controllableCharacters[0].GetComponent<MeshRenderer>().material = glowingMaterial;

        characterSlected = 0;
        cam.selectedUnit = controllableCharacters[0];
        
        front.y = 0;
        front = Vector3.Normalize(front);
        // To get the right and left directional sense
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * front;
    }
	
	// Update is called once per frame
	void Update () {
        if (TextManager.textViewEmptied)
        {
            //Select the character to lock on to, movement and camera
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    if (hit.transform.tag == "Player")
                    {
                        for (int i = 0; i < controllableCharacters.Length; i++)
                        {
                            if (controllableCharacters[i] == hit.transform.gameObject)
                            {
                                SelectCharacter(i);
                                break;
                            }
                        }
                    }
                }
            }
            //Switch characters
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                int newSelectedCharacter = characterSlected + 1;
                newSelectedCharacter = newSelectedCharacter >= controllableCharacters.Length ? 0 : newSelectedCharacter;
                SelectCharacter(newSelectedCharacter);
            }

            //Movement of the characters
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                float speed = characterMovement;
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    speed *= 1.2f;
                }
                Vector3 horizontalMovement = right * speed * Time.deltaTime * Input.GetAxis("Horizontal");
                Vector3 verticalMovement = front * speed * Time.deltaTime * Input.GetAxis("Vertical");

                controllableCharacters[characterSlected].GetComponent<CharacterController>().Move(horizontalMovement + verticalMovement);
            }

            //Tab switch between selectable characters
            for (KeyCode i = KeyCode.F1; i < KeyCode.F1 + controllableCharacters.Length; i++)
            {
                if (Input.GetKeyDown(i))
                {
                    SelectCharacter(i - KeyCode.F1);
                }
            }
        }
    }

    //Removes the material from old character, also focuses new selected character
    void SelectCharacter(int characterNumber)
    {
        if (characterNumber != characterSlected)
        {
            controllableCharacters[characterSlected].GetComponent<MeshRenderer>().material = normalMaterial;
            characterSlected = characterNumber;
            cam.selectedUnit = controllableCharacters[characterSlected];

            normalMaterial = controllableCharacters[characterSlected].GetComponent<MeshRenderer>().material;
            glowingMaterial.mainTexture = normalMaterial.mainTexture;
            glowingMaterial.color = normalMaterial.color;
            controllableCharacters[characterSlected].GetComponent<MeshRenderer>().material = glowingMaterial;
        }
    }
}
