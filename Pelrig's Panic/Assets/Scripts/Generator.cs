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
    void Update()
    {
        if (!isOn)
        {
            isOn = CheckForPlayersAround();
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
            onImage.SetActive(true);
            return true;
        }
        return false;
    }
}
