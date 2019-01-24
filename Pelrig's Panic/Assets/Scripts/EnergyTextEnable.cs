using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyTextEnable : MonoBehaviour
{
    [SerializeField]
    private GameObject energyText;

    [SerializeField]
    private GameObject controlButton;
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
        }
        else
        {
            energyText.SetActive(false);
            controlButton.SetActive(false);
        }
    }
}
