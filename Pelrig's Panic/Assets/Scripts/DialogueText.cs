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
        for (int i = 0; i < TextManager.currentCharacter; i++)
        {
            visibleText += TextManager.textSets[TextManager.currentTextSet][i];
            if (i == TextManager.currentCharacter - 1)
            {
                PlayLetterSound(TextManager.textSets[TextManager.currentTextSet][i], i % TextManager.allSoundListeners.Length);
            }
        }
        thisText.text = visibleText;
	}

    public void PlayLetterSound(char letter, int audioSourceNum)
    {
        for (int i = 0; i < TextManager.allLetters.Length; i++)
        {
            if (TextManager.allLetters[i].name[0] == letter)
            {
                TextManager.allSoundListeners[audioSourceNum].clip = TextManager.allLetters[i];
                TextManager.allSoundListeners[audioSourceNum].Play();
            }
        }
    }
}
