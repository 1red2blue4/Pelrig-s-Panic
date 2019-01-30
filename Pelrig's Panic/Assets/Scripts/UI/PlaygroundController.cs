using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlaygroundController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
   // [SerializeField]
   // private Button[] buttonSize;
    public static bool isMouseover;
     
    // Start is called before the first frame update
    void Start()
    {
        isMouseover = false;
    }
    
    /*public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("OnPointerEnter" + name); //This is never logging
       for(int i = 0; i < buttonSize.Length; i++)
        {
          // if(buttonSize[i].tag == "Player")
                //Debug.Log("Index of :   "+buttonSize[i].name);
          // else if (buttonSize[i].tag == "Finish")
             //   Debug.Log("Index of :   Atlantis");

      if(buttonSize[i].name == "Pirateship")
            {
                Debug.Log("Index of : Start ");
                buttonSize[0].GetComponent<Image>().color = Color.red;
            }
            else if (buttonSize[i].name == "Atlantis")
            {
                Debug.Log("Index of :  Credits ");
                buttonSize[1].GetComponent<Image>().color = Color.red;
            }
            //buttonSize[i].GetComponent<Image>().color = Color.red;

        }
        
    }*/

    
   // GameObject currentHover;

    public void OnPointerEnter(PointerEventData eventData)
    {
        /*if (eventData.pointerCurrentRaycast.gameObject != null)
        {
            Debug.Log("Mouse Over: " + eventData.pointerCurrentRaycast.gameObject.name);
            currentHover = eventData.pointerCurrentRaycast.gameObject;
        }*/
        isMouseover = true;
        if(isMouseover)
        {
            transform.GetComponent<Image>().color = Color.red;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Mouse exit");
        /* for (int i = 0; i < buttonSize.Length; i++)
         {
             buttonSize[i].GetComponent<Image>().color = Color.white;
         }
         currentHover = null;
         */
        isMouseover = false;
        if (!isMouseover)
        {
            transform.GetComponent<Image>().color = Color.white;
        }
    }

    void Update()
    {
      //  if (currentHover)
           // Debug.Log(currentHover.name + " @ " + Input.mousePosition);
    }
}
