using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextManager : MonoBehaviour {


    public static string[] textSets;
    public static int currentCharacter;
    public static int currentTextSet;
    public static int totalTextSets;
    private float timer;

	// Use this for initialization
	void Start ()
    {
        timer = 0.0f;
        currentCharacter = 0;
        currentTextSet = 0;
        totalTextSets = 5;
        textSets = new string[totalTextSets];

        textSets[0] = "This text continues onward until it reaches the end of the line, at which point I need to determine what happens. Can I make a fourth line? It looks like I can.";
        textSets[1] = "And so now we have a new set of lines making up text on the screen.  So here it is, my new text, which you are reading now. Isn't it nice?";
    }
	
	// Update is called once per frame
	void Update ()
    {
        bool wentToEndOfText = false;
        //move to the next character if not at the end of the dialogue box yet
        if (timer >= 0.01f)
        {
            if (currentCharacter < textSets[currentTextSet].Length)
            {
                currentCharacter++;
            }
            timer = 0.0f;
        }
        //increment the timer
        if (currentCharacter < textSets[currentTextSet].Length)
        {
            if (Input.GetKeyDown("space"))
            {
                currentCharacter = textSets[currentTextSet].Length - 1;
                wentToEndOfText = true;
            }
            timer += Time.deltaTime;
        }
        //look for dismissing the text box
        else
        {
            if (Input.GetKeyDown("space") && totalTextSets > currentTextSet - 1 && textSets[currentTextSet + 1] != null && wentToEndOfText == false)
            {
                currentCharacter = 0;
                currentTextSet++;
            }
        }
	}
}
