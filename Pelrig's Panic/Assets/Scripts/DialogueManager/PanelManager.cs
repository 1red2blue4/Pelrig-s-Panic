using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JSONFactory;
using UnityEngine.SceneManagement;

public class PanelManager : MonoBehaviour, DialogueStateManager
{
    public ManagerState currentState { get; private set; }
    private PanelConfig characterPanel;
    private NarrativeEvent currentEvent;
    private bool CharacterActive = true;
    private int stepIndex = -1;
    public static bool isPressed;
    public static bool playerControlsLocked;
    public static int countDialogueLength;

    [SerializeField]
    private GameObject dialoguePanel;

    void Start()
    {
        // isPressed = true;
        //playerControlsLocked = false;
    }
    public void BootSequence()
    { 
        characterPanel = GameObject.Find("CharacterPanel").GetComponent<PanelConfig>();
        currentEvent = JSONAssembly.RunJSONFactoryForScene(1);
        InitiziliasePanels();
    }


    void Update()
    {
        
        // Debug.Log("countDialogueLength  :       " + countDialogueLength);
        if (Input.GetMouseButtonDown(0) && isPressed == true)
        {
            isPressed = false; 
            if(PanelConfig.isDialogueTextOver)
            {               
                UpdatePanelState(); 
            }
            Debug.Log("countDialogueLength  :       "+ countDialogueLength);
            BootSequence();      
            
        }
        if (countDialogueLength >= 18)
        {
            Debug.Log("Enter here");
            dialoguePanel.SetActive(false);
        }
        if (SceneManager.GetActiveScene().buildIndex == 1 && (Input.GetKey(KeyCode.P) || countDialogueLength >= 16))
        { 
            countDialogueLength = 0;
            stepIndex = 0;
            playerControlsLocked = true;
            SceneManager.LoadScene("PirateShipUINad");
        }
        if (SceneManager.GetActiveScene().buildIndex == 2 && countDialogueLength <= 17)
        {           
            playerControlsLocked = true;
        }
        else
        { 
            playerControlsLocked = false;
        }
    }

    private void InitiziliasePanels()
    { 
        characterPanel.isTalking = true;        
        stepIndex++;
        countDialogueLength++;
        characterPanel.Configure(currentEvent.dialogues[stepIndex]);
        CharacterActive = !CharacterActive;
       // Debug.Log("currentEvent.dialogues[stepIndex]:   " + currentEvent.dialogues[stepIndex]);
       /* if (currentCharacter < textSets[currentTextSet].Length)
        {
            //currentCharacter = textSets[currentTextSet].Length - 1;
            // This is for automatic dialogue box appears in the interval 0f 10secs.
            //if (endConversation)
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                currentCharacter = textSets[currentTextSet].Length - 1;
                wentToEndOfText = true;
                //endConversation = false;               
            }
        }*/
    }


    private void ConfigurePanels()
    {
        if(CharacterActive)
        {
            characterPanel.ToggleCharcterMask();
            characterPanel.Configure(currentEvent.dialogues[stepIndex]);
        }
        else
        {
            
        }
    }

    void UpdatePanelState()
    {
        if(stepIndex < currentEvent.dialogues.Count)
        {
            ConfigurePanels();

            CharacterActive = !CharacterActive;

            stepIndex++;
             
        }
        else
        {
            
        }
    }
}
