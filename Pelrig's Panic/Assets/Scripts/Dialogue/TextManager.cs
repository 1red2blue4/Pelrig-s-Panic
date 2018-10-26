using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextManager {


    public static string[] textSets;
    public static AudioSource[] allSoundListeners;
    public static AudioClip[] allLetters;
    public static int currentCharacter;
    public static int currentTextSet;
    public static int totalTextSets;
    private static float timer;
    public static bool textViewEmptied;
    public static bool preppingNewVoice;

    public static GameObject audioSourceHolderMeda;
    public static GameObject audioSourceHolderKent;
    public static GameObject audioSourceHolderJade;
    public static GameObject audioSourceHolderEd;
    public static GameObject audioSourceHolderHally;

    // Use this for initialization
    public static void Setup()
    {
        audioSourceHolderMeda = GameObject.FindGameObjectWithTag("LettersHolderMeda");
        audioSourceHolderKent = GameObject.FindGameObjectWithTag("LettersHolderKent");
        audioSourceHolderJade = GameObject.FindGameObjectWithTag("LettersHolderJade");
        audioSourceHolderEd = GameObject.FindGameObjectWithTag("LettersHolderEd");
        audioSourceHolderHally = GameObject.FindGameObjectWithTag("LettersHolderHally");
        allSoundListeners = audioSourceHolderEd.GetComponents<AudioSource>();
        allLetters = audioSourceHolderEd.GetComponent<AudioClipHolder>().audioClips;
        totalTextSets = 5;
        textSets = new string[totalTextSets];

        preppingNewVoice = false;
        textViewEmptied = false;
        timer = 0.0f;
        currentCharacter = 0;
        currentTextSet = 0;
        
    }
	
	// Update is called once per frame
	public static void RunText()
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
            if (Input.GetKeyDown(KeyCode.Space))
            {
                currentCharacter = textSets[currentTextSet].Length - 1;
                wentToEndOfText = true;
            }
            timer += Time.deltaTime;
        }
        //look for dismissing the text box
        else
        {
            if (Input.GetKeyDown(KeyCode.Space) && totalTextSets > currentTextSet - 1 && textSets[currentTextSet + 1] != null && wentToEndOfText == false)
            {
                currentCharacter = 0;
                currentTextSet++;
                preppingNewVoice = true;
            }
            else if (Input.GetKeyDown(KeyCode.Space) && (currentTextSet == textSets.Length - 1 || textSets[currentTextSet + 1] == null) && wentToEndOfText == false)
            {
                currentCharacter = 0;
                textViewEmptied = true;
            }
        }
	}

    public static void CutSceneText()
    {
        
    }
}
