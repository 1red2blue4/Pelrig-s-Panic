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
            if(PanelConfig.isDialogueTextOver)
            {               
                UpdatePanelState(); 
            }
            BootSequence();                
        }
        if (countDialogueLength >= 18)
        {
            dialoguePanel.SetActive(false);
        }
        if (SceneManager.GetActiveScene().buildIndex == 1 && (Input.GetKey(KeyCode.P) || countDialogueLength >= 16))
        { 
            countDialogueLength = 0;
            stepIndex = 0;
            playerControlsLocked = true;
            SceneManager.LoadScene("PirateShipUI");
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
