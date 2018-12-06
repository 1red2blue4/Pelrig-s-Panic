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
        if (bubblesUpOnAction)
        {
            amountBloat = 0.0f;
            bubblingRate = 2.25f;
            bloatRate = 0.5f;
            startScale = objToBubblePres.transform.localScale;
            for (int i = 0; i < Board.possibleMoveableChars.Length; i++)
            {
                if (Board.possibleMoveableChars[i].thePiece == gameObject)
                {
                    prevPresence = Board.possibleMoveableChars[i].thePiece.GetComponent<Stats>().health;
                    prevResist = Board.possibleMoveableChars[i].thePiece.GetComponent<Stats>().damage; 
                }
            }
            /*
            for (int i = 0; i < Board.spawnedEnemies.Count; i++)
            {
                if (Board.spawnedEnemies[i].thePiece == gameObject)
                {
                    prevPresence = Board.spawnedEnemies[i].presenceValue;
                    prevResist = Board.spawnedEnemies[i].thePiece.GetComponent<Stats>().damage;
                }
            }
            */
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
                objToBubblePres.SetActive(true);
                objToBubbleResist.SetActive(true);
            }
            else if (!valueChanged)
            {
                objToDisappear.SetActive(false);
            }
        }
        
        if (bubblesUpOnAction && timer >= 0.5f)
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

        timer += Time.deltaTime;
    }

    public void CheckForBubbling()
    {
        int presenceValue = 0;
        int resistValue = 0;

        for (int i = 0; i < Board.possibleMoveableChars.Length; i++)
        {
            if (Board.possibleMoveableChars[i].thePiece == gameObject)
            {
              //  Debug.Log(" Board.possibleMoveableChars[i].thePiece.GetComponent<Stats>().health:       " + Board.possibleMoveableChars[i].thePiece.GetComponent<Stats>().health);
              //  Debug.Log(" Board.possibleMoveableChars[i].thePiece.GetComponent<Stats>().damage:       " + Board.possibleMoveableChars[i].thePiece.GetComponent<Stats>().damage);

                presenceValue = Board.possibleMoveableChars[i].thePiece.GetComponent<Stats>().health;
                resistValue = Board.possibleMoveableChars[i].thePiece.GetComponent<Stats>().damage;
            }
        }
        for (int i = 0; i < Board.spawnedEnemies.Count; i++)
        {
            if (Board.spawnedEnemies[i].thePiece == gameObject)
            {
                presenceValue = Board.spawnedEnemies[i].thePiece.GetComponent<Stats>().health;
                resistValue = Board.spawnedEnemies[i].thePiece.GetComponent<Stats>().damage;
            }
        }

        if (prevPresence != presenceValue)
        {
            objToDisappear.SetActive(true);
            objToBubblePres.SetActive(true);
            objToBubbleResist.SetActive(false);
            valueChanged = true;
            bubblingObj = objToBubblePres;
            distBetweenBubbling = 0.0f;
            amountBloat = 0.0f;
        }
        if (prevResist != resistValue)
        {
            objToDisappear.SetActive(true);
            objToBubbleResist.SetActive(true);
            objToBubblePres.SetActive(false);
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
        bubblingObj.transform.localScale = new Vector3(startScale.x + amountBloat, startScale.y + amountBloat, startScale.z + amountBloat);
        amountBloat += bloatRate * Time.deltaTime;
        distBetweenBubbling += bubblingRate * Time.deltaTime;

        if (distBetweenBubbling >= 1.0f)
        {
            distBetweenBubbling = 0.0f;
            valueChanged = false;
        }
    }
}
