using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PanelConfig : MonoBehaviour
{
    public bool isTalking;

    public Image characterImage;

    public Image TextBG;

    public Text characterName;

    public Text dialogue;

    private Color maskActiveColor = new Color(103.0f / 255.0f, 101.0f / 255.0f, 101.0f / 255.0f);


    public static bool isDialogueTextOver;

    void Start()
    {
        isDialogueTextOver = false;
    }

    public void ToggleCharcterMask()
    {
        if(isTalking)
        {
            characterImage.color = Color.white;
            TextBG.color = Color.white;
        }
        else
        {
            characterImage.color = maskActiveColor;
            TextBG.color = maskActiveColor;
        }
    }


    public void Configure(Dialogue currentDialoue)
    {
        ToggleCharcterMask();

        characterImage.sprite = DialogueManager.atlasManager.loadSprite(currentDialoue.atlasImageName);

        characterName.text = currentDialoue.name;
       
        if (isTalking)
        {
            StartCoroutine(AnimateText(currentDialoue.dialogueText));
        }
        else
        {
            dialogue.text = "";
        }
    }


    IEnumerator AnimateText(string dialogueText)
    {
        dialogue.text = "";

        foreach(char letter in dialogueText)
           {
            dialogue.text += letter;
            yield return new WaitForSeconds(0.05f);
            isDialogueTextOver = true;
            Debug.Log("Letter played:   " + letter);
        }
    }
}
