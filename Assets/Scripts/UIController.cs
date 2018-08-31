using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    [SerializeField] private GameObject downArrow;
    [SerializeField] private GameObject leftArrow;
    [SerializeField] private GameObject rightArrow;
    [SerializeField] private GameObject upArrow;
    private GameObject[] nextInSequence;

    // Use this for initialization
    void Start ()
    {
        nextInSequence = new GameObject[4];
        GameObject[] tempObjects = GameObject.FindGameObjectsWithTag("arrows");

        //make sure the objects are in the correct order
        for (int i = 0; i < nextInSequence.Length; i++)
        {
            for (int j = 0; j < tempObjects.Length; j++)
            {
                if (tempObjects[j].GetComponent<UIData>().arrowID == i)
                {
                    nextInSequence[i] = tempObjects[j];
                    Debug.Log(nextInSequence[i]);
                    Debug.Log(tempObjects[j]);
                    break;
                }
            }
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
		for (int i = 0; i < nextInSequence.Length; i++)
        {
            switch(MovementManager.directionLineup[i])
            {
                case MovementManager.Direction.Down:
                    nextInSequence[i].GetComponent<Image>().sprite = downArrow.GetComponent<Image>().sprite;
                    break;

                case MovementManager.Direction.Left:
                    nextInSequence[i].GetComponent<Image>().sprite = leftArrow.GetComponent<Image>().sprite;
                    break;

                case MovementManager.Direction.Right:
                    nextInSequence[i].GetComponent<Image>().sprite = rightArrow.GetComponent<Image>().sprite;
                    break;

                case MovementManager.Direction.Up:
                    nextInSequence[i].GetComponent<Image>().sprite = upArrow.GetComponent<Image>().sprite;
                    break;

                default:
                    nextInSequence[i].GetComponent<Image>().sprite = upArrow.GetComponent<Image>().sprite;
                    break;
            }
        }
        Debug.Log(nextInSequence[0].GetComponent<Image>().sprite);


	}
}
