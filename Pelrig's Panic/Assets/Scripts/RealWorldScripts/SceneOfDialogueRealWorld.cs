using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneOfDialogueRealWorld : MonoBehaviour {

    [SerializeField] public bool wonLastBattle;
    private bool ranSecondDialogue;
    [SerializeField] private DialoguePortrait charPortrait;
    public CharacterData talkingCharacter;
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
        Debug.Log("currLine:  " + currLine);
        SetNewVoice(allConversations[currConversationNum].characterData[TextManager.currentTextSet].characterIdNum);

    }

    void Update()
    {
        string prevLine = currLine;
        if (!TextManager.textViewEmptied)
        {
            Debug.Log("currentTextSet:  "+ TextManager.currentTextSet);
            currLine = TextManager.textSets[TextManager.currentTextSet];
           
            if (prevLine != currLine)
            {
                SetNewVoice(allConversations[currConversationNum].characterData[TextManager.currentTextSet].characterIdNum);
            }
            TextManager.RunText();
        }
        else
        {
            dialogueBox.SetActive(false);
        }

        //condition under which the conversation changes
        talkingCharacter = allConversations[currConversationNum].characterData[TextManager.currentTextSet];
        charPortrait.talkingCharacter = talkingCharacter;
        charPortrait.UpdatePortrait();
        if (TextManager.textViewEmptied && Input.GetAxis("RunConversation") > 0 && !ranSecondDialogue)
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
        convo1Words[0] = "...Jade... is there something going on?";
        convo1Words[1] = "Nope! I just wanted to walk over and say hi, Ed. So, hi, Ed! Hally totally couldn't catch me today.";
        Conversation convo1 = new Conversation(convo1Words, lengthConvo1, convo1CharData);

        int lengthConvo2 = 2;
        string[] convo2Words = new string[lengthConvo2];
        CharacterData[] convo2CharData = new CharacterData[lengthConvo2];
        convo2CharData[0] = meda.GetComponent<CharacterData>();
        convo2CharData[1] = kent.GetComponent<CharacterData>();
        convo2Words[0] = "Kent, it appears your bandage is peeling. Is there something I can do to remedy the situation? Perhaps I could find some type of adhesive.";
        convo2Words[1] = "No! I'm fine, Meda. I don't need any help! So just... rgh, just leave me be, alright?";
        Conversation convo2 = new Conversation(convo2Words, lengthConvo2, convo2CharData);

        Conversation[] multiConvo = new Conversation[2];
        multiConvo[0] = convo1;
        multiConvo[1] = convo2;

        return multiConvo;
    }


}
