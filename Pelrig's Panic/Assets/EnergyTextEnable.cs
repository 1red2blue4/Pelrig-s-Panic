﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyTextEnable : MonoBehaviour
{
    [SerializeField]
    private GameObject energyText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!PanelManager.playerControlsLocked && !TutorialCards.isTutorialRunning)
        {
            energyText.SetActive(true);
        }
        else
        {
            energyText.SetActive(false);
        }
    }
}