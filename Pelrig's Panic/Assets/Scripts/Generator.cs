using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Generator : MonoBehaviour
{
    bool isOn  = false;
    public Piece generator;
    [SerializeField] public GameObject onImage;
    [SerializeField] public GameObject offImage;
    [SerializeField] private GameObject textHolder;
    Text generatorPopupText;
    // Use this for initialization
    void Start ()
    {
        generator = GetComponent<Piece>();
        //generatorPopupText = textHolder.GetComponent<TextMesh>;
        //gameObject.GetComponent<TextMesh>().text = null;

    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!isOn)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                GeneratorPopupText.isVisible = true;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                   
                    if (hit.transform.gameObject == gameObject)
                    {
                        
                        isOn = CheckForPlayersAround();
                    }
                    else if (hit.transform.gameObject.transform.parent != null)
                    {
                        if (hit.transform.gameObject.transform.parent.gameObject == gameObject)
                        {
                            isOn = CheckForPlayersAround();
                        }
                    }
                }
            }
        }

        if (isOn)
        {
            onImage.SetActive(true);
        }
        else
        {            
            onImage.SetActive(false);
        }
    }

    bool CheckForPlayersAround()
    {
        int playersAround = 0;
        foreach (var playerObjects in Board.possibleMoveableChars)
        {
            if (playerObjects.rowPosition <= generator.rowPosition + 1 && 
                playerObjects.rowPosition >= generator.rowPosition - 1 &&
                playerObjects.colPosition <= generator.colPosition + 1 && 
                playerObjects.colPosition >= generator.colPosition - 1)
            {
                playersAround++;
               
            }
        }

        //generatorPopupText.text = "Temp:" + displayNumber;
        //gameObject.GetComponent<TextMesh>().text = displayNumber.ToString();
       // var displayNumber = 3 - playersAround;
       


        if (playersAround >= 3)
        {
            ExperimentalResources.generatorsActive++;
            int temp = ExperimentalResources.generatorsActive++;
            transform.GetChild(0).GetComponent<TextMesh>().text = "Activated" + "\ngenerator";
            return true;
        }
        return false;
    }
}
