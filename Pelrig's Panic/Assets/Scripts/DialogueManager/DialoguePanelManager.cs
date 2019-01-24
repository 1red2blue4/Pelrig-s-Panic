using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JSONText;
using UnityEngine.SceneManagement;

public class DialoguePanelManager : MonoBehaviour, DialogueStateManager
{
    public ManagerState currentState { get; private set; }
    private DialoguePanelConfig characterPanel;
    private NarrativeEvent currentEvent;
    private bool CharacterActive = true;
    private int stepIndex = -1;
    public static bool isPressed;
    public static bool playerControlsLocked;
    public static int countDialogueLength;
    [SerializeField] private int maxCountDialogueLength;

    [SerializeField]
    private GameObject dialoguePanel;

    void Start()
    {
        // isPressed = true;
        //playerControlsLocked = false;
    }
    public void BootSequence()
    { 
        characterPanel = GameObject.Find("CharacterPanel").GetComponent<DialoguePanelConfig>();        

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            currentEvent = JSONAssembly.RunJSONFactoryForScene(1); 
        }
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            currentEvent = JSONAssembly.RunJSONFactoryForScene(2);
        }
        InitiziliasePanels();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isPressed == true)
        {
            isPressed = false; 
            if(DialoguePanelConfig.isDialogueTextOver)
            {               
                UpdatePanelState(); 
            }
            BootSequence();                
        }
        if (Input.GetKey(KeyCode.P) || countDialogueLength >= currentEvent.dialogues.Count)
        {
            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                dialoguePanel.SetActive(false);
                countDialogueLength = 0;
                stepIndex = 0;
                playerControlsLocked = false;
                SceneManager.LoadScene("PirateshipScene");
            }
            if (SceneManager.GetActiveScene().buildIndex == 2)
            {
                dialoguePanel.SetActive(false);
                playerControlsLocked = false;
                countDialogueLength = currentEvent.dialogues.Count;
            }
        }
        else if (countDialogueLength < currentEvent.dialogues.Count)
        {
            playerControlsLocked = true;
        }        
    }
    private void InitiziliasePanels()
    { 
        characterPanel.isTalking = true;        
        stepIndex++;
        countDialogueLength++;
        characterPanel.Configure(currentEvent.dialogues[stepIndex]);
        CharacterActive = !CharacterActive;
    }
    private void ConfigurePanels()
    {
        if(CharacterActive)
        {
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
