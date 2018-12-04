using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Generator : MonoBehaviour
{
    bool isOn = false;
    public Piece generator;
    [SerializeField] private GameObject[] onImages;
    [SerializeField] private GameObject offImage;
    [SerializeField] private GameObject uiImage;
    [SerializeField] private GameObject textHolder;
    private int numCharsToSurround;
    Text generatorPopupText;
    // Use this for initialization
    void Start()
    {
        generator = GetComponent<Piece>();
        numCharsToSurround = 3;
        
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

        if (playersAround >= 0 && playersAround < numCharsToSurround)
        {
            uiImage.SetActive(true);
            if (uiImage.transform.GetChild(0) != null && uiImage.transform.GetChild(0).GetComponent<TextMesh>() != null)
            {
                uiImage.transform.GetChild(0).GetComponent<TextMesh>().text = (numCharsToSurround - playersAround).ToString();
                uiImage.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = uiImage.transform.GetComponent<SpriteRenderer>().sortingOrder;
            }
        }
        else
        {
            uiImage.SetActive(false);
        }



        if (playersAround >= numCharsToSurround)
        {
            ExperimentalResources.generatorsActive++;
            for (int i = 0; i < onImages.Length; i++)
            {
                onImages[i].SetActive(true);
                if (onImages[i].GetComponent<SpriteRenderer>() != null)
                {
                    onImages[i].GetComponent<SpriteRenderer>().enabled = true;
                }
            }
            offImage.SetActive(false);
            return true;
        }
        return false;
    }
}