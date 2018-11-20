using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JSONFactory;

public class PanelManager : MonoBehaviour, DialogueStateManager
{
    public ManagerState currentState { get; private set; }

    private PanelConfig rightPanel;
    private PanelConfig leftPanel;

    private NarrativeEvent currentEvent;

    private bool leftCharacterActive = true;

    private int stepIndex = 0;

    public bool isPressed;

    void Start()
    {
        isPressed = false;
    }
    public void BootSequence()
    {

        rightPanel = GameObject.Find("RightCharacterPanel").GetComponent<PanelConfig>();
        leftPanel = GameObject.Find("LeftCharacterPanel").GetComponent<PanelConfig>();
        currentEvent = JSONAssembly.RunJSONFactoryForScene(1);
        InitiziliasePanels();


    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isPressed == true)
        {
            if(PanelConfig.isDialogueTextOver)
            {
               // isPressed = true;
                UpdatePanelState();
            }
            Debug.Log("leftCharacterActive:     "+ leftCharacterActive);
            Debug.Log("rightPanel.isTalking:     " + rightPanel.isTalking);
            Debug.Log("rightPanel.isTalking:     " + rightPanel.isTalking);
           
        }
    }

    private void InitiziliasePanels()
    {
        leftPanel.isTalking = true;
        rightPanel.isTalking = false;
        
        stepIndex++;
        
        leftPanel.Configure(currentEvent.dialogues[stepIndex]);
        rightPanel.Configure(currentEvent.dialogues[stepIndex + 1]);

        leftCharacterActive = !leftCharacterActive;

    }


    private void ConfigurePanels()
    {
        if(leftCharacterActive)
        {
            leftPanel.isTalking = true;
            rightPanel.isTalking = false;

            leftPanel.Configure(currentEvent.dialogues[stepIndex]);
            rightPanel.ToggleCharcterMask();
        }
        else
        {
            leftPanel.isTalking = false;
            rightPanel.isTalking = true;

            leftPanel.ToggleCharcterMask();
            rightPanel.Configure(currentEvent.dialogues[stepIndex]);            
        }
    }

    void UpdatePanelState()
    {
        if(stepIndex < currentEvent.dialogues.Count)
        {
            ConfigurePanels();

            leftCharacterActive = !leftCharacterActive;

            stepIndex++;
        }
        else
        {
            
        }
    }
}
