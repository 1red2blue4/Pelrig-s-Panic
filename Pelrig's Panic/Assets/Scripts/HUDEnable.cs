using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDEnable : MonoBehaviour
{
    public static bool isVisible;
    // Use this for initialization
    void Start()
    {
        isVisible = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (!DialoguePanelManager.playerControlsLocked && !TutorialCards.isTutorialRunning && isVisible)
        {
            gameObject.GetComponent<Image>().enabled = true;
        }
        else
        {
            gameObject.GetComponent<Image>().enabled = false;
        }
    }
}
