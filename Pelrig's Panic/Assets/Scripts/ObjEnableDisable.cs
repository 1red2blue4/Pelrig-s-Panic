using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjEnableDisable : MonoBehaviour
{
    [SerializeField]
    private GameObject energyText;

    [SerializeField]
    private GameObject controlButton;

    [SerializeField]
    private GameObject hudClose;

   // //[SerializeField]
    private GameObject hudPanel;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        if(!DialoguePanelManager.playerControlsLocked && !TutorialCards.isTutorialRunning)
        {
            energyText.SetActive(true);
            controlButton.SetActive(true);
            hudClose.SetActive(true);
           // hudPanel.SetActive(true);
        }
        else
        {
            energyText.SetActive(false);
            controlButton.SetActive(false);
            hudClose.SetActive(false);
           // hudPanel.SetActive(false);
        }
    }
}
