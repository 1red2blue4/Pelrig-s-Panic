using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneOfDialogueRealWorld : MonoBehaviour {

    [SerializeField] public bool wonLastBattle;
    private bool ranSecondDialogue;
    public Conversation[] allConversations;
    public int currConversationNum;
    public string currLine;
    private float timer;
    [SerializeField] public GameObject dialogueBox;
    [SerializeField] public GameObject meda;
    [SerializeField] public GameObject kent;
    [SerializeField] public GameObject jade;
    [SerializeField] public GameObject ed;
    [SerializeField] public GameObject hally;

    void Start()
    {
        timer = 0.0f;
        ranSecondDialogue = false;
        currLine = "";
        currConversationNum = 0;
        TextManager.Setup();

        allConversations = ProduceConversations();

        for (int i = 0; i < allConversations[currConversationNum].lines.Length; i++)
        {
            TextManager.textSets[i] = allConversations[currConversationNum].lines[i];
        }
        currLine = TextManager.textSets[currConversationNum];

        SetNewVoice(allConversations[currConversationNum].characterData[TextManager.currentTextSet].characterIdNum);

    }

    void Update()
    {
        string prevLine = currLine;
        if (!TextManager.textViewEmptied)
        {
            TextManager.RunText();
            currLine = TextManager.textSets[TextManager.currentTextSet];
            if (prevLine != currLine)
            {
                SetNewVoice(allConversations[currConversationNum].characterData[TextManager.currentTextSet].characterIdNum);
            }
        }
        else
        {
            dialogueBox.SetActive(false);
        }

        //condition under which the conversation changes
        if (timer >= 10.0f && !ranSecondDialogue)
        {
            currConversationNum = 1;
            TextManager.textViewEmptied = false;
            for (int i = 0; i < allConversations[currConversationNum].lines.Length; i++)
            {
                TextManager.textSets[i] = allConversations[currConversationNum].lines[i];
            }
            TextManager.currentTextSet = 0;

            SetNewVoice(allConversations[currConversationNum].characterData[TextManager.currentTextSet].characterIdNum);
            dialogueBox.SetActive(true);
            timer = 0.0f;
            ranSecondDialogue = true;
        }
        else
        {
            timer += Time.deltaTime;
        }
    }

    public void SetNewVoice(int charIdNum)
    {
        CharacterData[] allCharactersWithVoices = GameObject.FindObjectsOfType<CharacterData>();
        for (int i = 0; i < allCharactersWithVoices.Length; i++)
        {
            if (allCharactersWithVoices[i].characterIdNum == charIdNum)
            {
                if (charIdNum == 1)
                {
                    TextManager.allSoundListeners = TextManager.audioSourceHolderKent.GetComponents<AudioSource>();
                    TextManager.allLetters = TextManager.audioSourceHolderKent.GetComponent<AudioClipHolder>().audioClips;
                }
                else if (charIdNum == 2)
                {
                    TextManager.allSoundListeners = TextManager.audioSourceHolderHally.GetComponents<AudioSource>();
                    TextManager.allLetters = TextManager.audioSourceHolderHally.GetComponent<AudioClipHolder>().audioClips;
                }
                else if (charIdNum == 3)
                {
                    TextManager.allSoundListeners = TextManager.audioSourceHolderEd.GetComponents<AudioSource>();
                    TextManager.allLetters = TextManager.audioSourceHolderEd.GetComponent<AudioClipHolder>().audioClips;
                }
                else if (charIdNum == 4)
                {
                    TextManager.allSoundListeners = TextManager.audioSourceHolderMeda.GetComponents<AudioSource>();
                    TextManager.allLetters = TextManager.audioSourceHolderMeda.GetComponent<AudioClipHolder>().audioClips;
                }
                else if (charIdNum == 5)
                {
                    TextManager.allSoundListeners = TextManager.audioSourceHolderJade.GetComponents<AudioSource>();
                    TextManager.allLetters = TextManager.audioSourceHolderJade.GetComponent<AudioClipHolder>().audioClips;
                }
            }
        }
    }

    public Conversation[] ProduceConversations()
    {
        int lengthConvo1 = 2;
        string[] convo1Words = new string[lengthConvo1];
        CharacterData[] convo1CharData = new CharacterData[lengthConvo1];
        convo1CharData[0] = ed.GetComponent<CharacterData>();
        convo1CharData[1] = jade.GetComponent<CharacterData>();
        convo1Words[0] = "This text continues onward until it reaches the end of the line, at which point I need to determine what happens. Can I make a fourth line? It looks like I can.";
        convo1Words[1] = "And so now we have a new set of lines making up text on the screen.  So here it is, my new text, which you are reading now. Isn't it nice?";
        Conversation convo1 = new Conversation(convo1Words, lengthConvo1, convo1CharData);

        int lengthConvo2 = 2;
        string[] convo2Words = new string[lengthConvo2];
        CharacterData[] convo2CharData = new CharacterData[lengthConvo2];
        convo2CharData[0] = meda.GetComponent<CharacterData>();
        convo2CharData[1] = kent.GetComponent<CharacterData>();
        convo2Words[0] = "Here's some new text which is brand new and fresh. I hope that our viewers enjoy it.";
        convo2Words[1] = "Yes, indeed, Meda. I would completely agree with that sentiment.";
        Conversation convo2 = new Conversation(convo2Words, lengthConvo2, convo2CharData);

        Conversation[] multiConvo = new Conversation[2];
        multiConvo[0] = convo1;
        multiConvo[1] = convo2;

        return multiConvo;
    }
}
