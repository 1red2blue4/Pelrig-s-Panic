using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueText : MonoBehaviour {

    string visibleText;
    Text thisText;

	// Use this for initialization
	void Start ()
    {
        thisText = gameObject.GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        visibleText = "";
        for (int i = 0; i <= TextManager.currentCharacter; i++)
        {
            visibleText += TextManager.textSets[TextManager.currentTextSet][i];
        }
        thisText.text = visibleText;
	}
}
