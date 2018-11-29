using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public static bool playerControlsLocked;

    public static GameObject audioSourceHolderMeda;
    public static GameObject audioSourceHolderKent;
    public static GameObject audioSourceHolderJade;
    public static GameObject audioSourceHolderEd;
    public static GameObject audioSourceHolderHally;

    public static bool endConversation;
    public static int countDialogueLength;
    public static bool isSpaceKeyPressed;

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
        totalTextSets = 50;
        textSets = new string[totalTextSets];

        preppingNewVoice = false;
        textViewEmptied = false;
        timer = 0.0f;
        currentCharacter = 0;
        currentTextSet = 0;

        playerControlsLocked = false;
        
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
            //currentCharacter = textSets[currentTextSet].Length - 1;
            // This is for automatic dialogue box appears in the interval 0f 10secs.
            //if (endConversation)
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                currentCharacter = textSets[currentTextSet].Length - 1;
                wentToEndOfText = true;
                //endConversation = false;               
            }
            timer += Time.deltaTime;            
        }
        //look for dismissing the text box
        else
        {
            if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && totalTextSets > currentTextSet - 1 && textSets[currentTextSet + 1] != null && wentToEndOfText == false && isSpaceKeyPressed == false)
            {
                SceneSequence.isSkipKeyPressed = true;
                currentCharacter = 0;
                currentTextSet++;
                preppingNewVoice = true;

                //This is for transition into the PirateShip scene.
                countDialogueLength++;                 
            }
            else if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && (currentTextSet == textSets.Length - 1 || textSets[currentTextSet + 1] == null) && wentToEndOfText == false)
            {
                currentCharacter = 0;
                textViewEmptied = true;
            }
            //else if (Input.GetKeyDown(KeyCode.S))
            //{
            //    SceneManager.LoadScene("PirateShipWithBoard");
            //}
            // This is for automatic dialogue box appears in the interval 0f 10secs.
            /*if (endConversation && totalTextSets > currentTextSet - 1 && textSets[currentTextSet + 1] != null && wentToEndOfText == false)
            {
                currentCharacter = 0;
                currentTextSet++;
                preppingNewVoice = true;
            }
            else if (endConversation && (currentTextSet == textSets.Length - 1 || textSets[currentTextSet + 1] == null) && wentToEndOfText == false)
            {
                currentCharacter = 0;
                textViewEmptied = true;
            }*/
        }
        //TODO: organize buildIndex documentation
        if (SceneManager.GetActiveScene().buildIndex == 3 && (Input.GetKey(KeyCode.P) || countDialogueLength >= 18))
        {
            countDialogueLength = 0;
            playerControlsLocked = true;
            SceneManager.LoadScene("PirateShipUI");
        }
        Debug.Log(playerControlsLocked);
        Debug.Log(SceneManager.GetActiveScene().buildIndex);
        if (SceneManager.GetActiveScene().buildIndex == 4 && countDialogueLength < 9)
        {
            Debug.Log("Controls should be locked!");
            playerControlsLocked = true;
        }
        else
        {
            playerControlsLocked = false;
        }
    }
}
