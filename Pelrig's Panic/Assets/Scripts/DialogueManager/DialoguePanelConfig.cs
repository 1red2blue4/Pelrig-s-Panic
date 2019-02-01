using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DialoguePanelConfig : MonoBehaviour
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

    [SerializeField] private Font edsFont;
    [SerializeField] private Font medasFont;
    [SerializeField] private Font kentsFont;
    [SerializeField] private Font jadesFont;
    [SerializeField] private Font hallysFont;
    [SerializeField] private Font universalFancyFont;

    void Start()
    {
        isDialogueTextOver = false;
    }

    public void Configure(Dialogue currentDialogue)
    {
        characterImage.sprite = DialogueManager.atlasManager.loadSprite(currentDialogue.CharacterImage);
        characterName.text = currentDialogue.CharacterName;        
        SetFont(characterName, true, characterName.text);

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
        SetFont(dialogue, false, characterName.text);
        foreach (char letter in dialogueText)
        {            
            dialogue.text += letter;            
            yield return new WaitForSeconds(0.004f);

            count++;
            if (dialogueText.Length < count)
            {
                count = 1;
                DialoguePanelManager.isPressed = true;
            }
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) && dialogueText.Length < count)
            {
                count = 1;
                dialogue.text = dialogueText;
                DialoguePanelManager.isPressed = true;
                break;
            }
        }        
    }

    public void SetFont(Text text, bool fancy, string name)
    {
        if (fancy)
        {
            text.font = universalFancyFont;
        }
        else
        {
            if (name == "hally" || name == "Hally")
            {
                text.font = hallysFont;
            }
            else if (name == "meda" || name == "Meda")
            {
                text.font = medasFont;
            }
            else if (name == "kent" || name == "Kent")
            {
                text.font = kentsFont;
            }
            else if (name == "jade" || name == "Jade")
            {
                text.font = jadesFont;
            }
            else if (name == "ed" || name == "Ed")
            {
                text.font = edsFont;
            }
        }
    }
}
