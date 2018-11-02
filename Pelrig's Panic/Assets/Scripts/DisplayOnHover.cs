using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayOnHover : MonoBehaviour {

    [SerializeField] private GameObject objToDisappear;

    [SerializeField] private GameObject objToBubblePres;
    [SerializeField] private GameObject objToBubbleResist;
    private GameObject bubblingObj;
    [SerializeField] public bool bubblesUpOnAction;
    public bool valueChanged;
    [SerializeField] public ValueHolder presAndResist;
    private int prevPresence;
    private int prevResist;
    private Vector3 startScale;
    private float distBetweenBubbling;
    private float bubblingRate;
    private float bloatRate;
    private float amountBloat;
    private float timer;

    void Start()
    {
        timer = 0.0f;
        if (bubblingObj)
        {
            amountBloat = 0.0f;
            bubblingRate = 0.1f;
            bloatRate = 0.02f;
            startScale = objToBubblePres.transform.localScale;
            prevPresence = presAndResist.presenceObj.GetComponent<UIValues>().initialValue;
            prevResist = presAndResist.resistanceObj.GetComponent<UIValues>().initialValue;
            valueChanged = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject == gameObject)
            {
                objToDisappear.SetActive(true);
            }
            else if (!valueChanged)
            {
                objToDisappear.SetActive(false);
            }
        }
        
        //TODO: Get this to work.
        /*
        if (bubblesUpOnAction && timer >= 5.0f)
        {
            if (!valueChanged)
            {
                CheckForBubbling();
            }

            if (valueChanged)
            {
                Bubble(bubblingObj);
            }
            else
            {
                objToBubblePres.transform.localScale = startScale;
                objToBubbleResist.transform.localScale = startScale;
            }
        }
        */

        timer += Time.deltaTime;
    }

    public void CheckForBubbling()
    {
        int presenceValue = presAndResist.presenceObj.GetComponent<UIValues>().GetValue();
        int resistValue = presAndResist.resistanceObj.GetComponent<UIValues>().GetValue();

        if (prevPresence != presenceValue)
        {
            Debug.Log("Previous: " + prevPresence);
            Debug.Log("Now: " + presenceValue);
            objToBubblePres.SetActive(true);
            valueChanged = true;
            bubblingObj = objToBubblePres;
            distBetweenBubbling = 0.0f;
            amountBloat = 0.0f;
        }
        if (prevResist != resistValue)
        {
            objToBubbleResist.SetActive(true);
            valueChanged = true;
            bubblingObj = objToBubbleResist;
            distBetweenBubbling = 0.0f;
            amountBloat = 0.0f;
        }

        prevPresence = presenceValue;
        prevResist = resistValue;
    }

    public void Bubble(GameObject bubblingObj)
    {
        bubblingObj.transform.localScale = new Vector3(startScale.x + bloatRate, startScale.y + bloatRate, startScale.z + bloatRate);
        amountBloat += bloatRate;
        distBetweenBubbling += bubblingRate;

        if (distBetweenBubbling >= 1.0f)
        {
            distBetweenBubbling = 0.0f;
            valueChanged = false;
        }
    }
}
