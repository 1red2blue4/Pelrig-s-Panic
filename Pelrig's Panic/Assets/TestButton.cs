using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TestButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float time = 5.0f;

    public bool onClicked;

    public static bool isEnable;

    [SerializeField]
   // private GameObject endturnBtn;
    // Start is called before the first frame update
    void Start()
    {
        onClicked = false;
    }

    

    void Update()
    {

        time -= Time.deltaTime;

         if (Mathf.RoundToInt(time) < 1)
         {
             time = 5;
             //
             onClicked = false;
         }     
         if(Mathf.RoundToInt(time) == 2 && onClicked == true)
         {
             Debug.Log("disable the button after 4secs");
             gameObject.GetComponent<Image>().enabled = false;
         }
        if(!PlayerControls.isPlayerTurn)
        {
            //onClicked = false;
        } 
        if(isEnable)
        {
            //transform.GetComponent<Image>().enabled = true;
        }
        else
        {
          //  transform.GetComponent<Image>().enabled = false;
        }
    }

    public void OnClick()
    {
        transform.GetComponent<Image>().enabled = true;
        time = 5f;
        Debug.Log("OnClick");
        Debug.Log("time:    " + time);
        onClicked = true;

        /*if (!isEnable && !DialoguePanelManager.playerControlsLocked)
        {*/
            //onClicked = true;
           // transform.GetComponent<Image>().enabled = true;
            //endturnBtn.SetActive(true);

       // }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.GetComponent<Image>().enabled = true;
        Debug.Log("OnPointerEnter");

        //gameObject.SetActive(true);

    }
    public void OnPointerExit(PointerEventData eventData)
    {
     /*   if(!onClicked)
        {
            
            Debug.Log("OnPointerExit");
        }*/
        if(!onClicked)
        {
            Debug.Log("OnPointerExit");
            transform.GetComponent<Image>().enabled = false;
        }

        //gameObject.SetActive(false);
    }
}
