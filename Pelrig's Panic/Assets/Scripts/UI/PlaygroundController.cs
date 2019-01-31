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
    public Sprite onSprite; 
    public Sprite offSprite;

    [SerializeField]
    private string buttonText;
    // Start is called before the first frame update
    void Start()
    {
        isMouseover = false;
    }
    
   // GameObject currentHover;

    public void OnPointerEnter(PointerEventData eventData)
    {
        /*if (eventData.pointerCurrentRaycast.gameObject != null)
        {
            Debug.Log("Mouse Over: " + eventData.pointerCurrentRaycast.gameObject.name);
            currentHover = eventData.pointerCurrentRaycast.gameObject;
        }*/
        //transform.GetComponent<Image>().color = Color.red;
        transform.GetComponent<Image>().sprite = onSprite;
        transform.GetChild(0).GetComponent<Text>().text = "";
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Mouse exit");
        /* for (int i = 0; i < buttonSize.Length; i++)
         {
             buttonSize[i].GetComponent<Image>().color = Color.white;
         }
         currentHover = null;*/
        //transform.GetComponent<Image>().color = Color.white;
        transform.GetComponent<Image>().sprite = offSprite;
        //transform.GetChild(0).GetComponent<Text>().text = buttonText;


    }

    void Update()
    {
      //  if (currentHover)
           // Debug.Log(currentHover.name + " @ " + Input.mousePosition);
    }
}
