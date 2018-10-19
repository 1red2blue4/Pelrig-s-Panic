using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    [SerializeField] private GameObject downArrow;
    [SerializeField] private GameObject leftArrow;
    [SerializeField] private GameObject rightArrow;
    [SerializeField] private GameObject upArrow;
    private int numVisibleInSequence;
    private GameObject[,] nextInSequence;

    // Use this for initialization
    void Start ()
    {
        MovementManager.Setup();
        numVisibleInSequence = 4;
        nextInSequence = new GameObject[numVisibleInSequence, MovementManager.numDirectionLineups];
        GameObject[] tempObjects = GameObject.FindGameObjectsWithTag("arrows");

        //make sure the objects are in the correct order
        for (int k = 0; k < MovementManager.numDirectionLineups; k++)
        {
            for (int i = 0; i < numVisibleInSequence; i++)
            {
                for (int j = 0; j < tempObjects.Length; j++)
                {
                    if (tempObjects[j].GetComponent<UIData>().arrowID == i + numVisibleInSequence*k)
                    {
                        nextInSequence[i, k] = tempObjects[j];
                        break;
                    }
                }
            }
        }
	}
	
	// Update is called once per frame
	void Update ()
    {

	}
}
