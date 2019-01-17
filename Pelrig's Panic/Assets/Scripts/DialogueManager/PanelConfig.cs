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
    private int count = 1;
    public Font[] characterFont;
    void Start()
    {
        isDialogueTextOver = false;
    }

    public void Configure(Dialogue currentDialogue)
    {
        characterImage.sprite = DialogueManager.atlasManager.loadSprite(currentDialogue.CharacterImage);

        characterName.text = currentDialogue.CharacterName;

        //dialogue.font = Resources.Load<Font>("Fonts/orang juice2.0") as Font;

        //Debug.Log("dialogue.font :      "+ currentDialogue.FontName);



        if (isTalking)
        {
            StartCoroutine(AnimateText(currentDialogue.DialogueText));
        }
        else
        {
            dialogue.text = "";
        }
    }

    IEnumerator AnimateText(string dialogueText)
    {
        dialogue.text = "";
        foreach (char letter in dialogueText)
        {
            
            dialogue.text += letter;
            
            yield return new WaitForSeconds(0.005f);

            count++;
            if (dialogueText.Length < count)
            {
                count = 1;
                PanelManager.isPressed = true;
            }
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) && dialogueText.Length < count)
            {
                count = 1;
                dialogue.text = dialogueText;
                PanelManager.isPressed = true;
                break;
            }
        }
    }
}
