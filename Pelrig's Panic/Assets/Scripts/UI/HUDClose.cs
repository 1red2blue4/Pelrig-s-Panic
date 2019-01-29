using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDClose : MonoBehaviour
{
    [SerializeField]
    GameObject hudPanel;
    // Start is called before the first frame update    

    private bool onoff; 

    void Start()
    {
        onoff = false;
    }
    void Update()
    {
       
    }

    public void OpenHudPanel()
    {       
        onoff = !onoff; // toggles onoff at each click
        Debug.Log("onoff:       "+ onoff);
        if (onoff)
        {
            // hudPanel.SetActive(true);
            hudPanel.SetActive(false);
        }
        else
        {
            //hudPanel.SetActive(false);
            hudPanel.SetActive(true);
        }
    }
}
