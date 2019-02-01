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

    public void OnPointerEnter(PointerEventData eventData)
    { 
        transform.GetComponent<Image>().sprite = onSprite;
        transform.GetChild(0).GetComponent<Text>().text = "";
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Mouse exit"); 
        transform.GetComponent<Image>().sprite = offSprite; 
    }      
}
