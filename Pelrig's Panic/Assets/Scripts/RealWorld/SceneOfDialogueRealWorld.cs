using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneOfDialogueRealWorld : MonoBehaviour
{
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
    public Conversation[] startSceneConverstion;
    public bool isStartConvo;
    public static string[] convoWords;
    void Start()
    {
        timer = 0.0f;
        ranSecondDialogue = false;
        currLine = "";
        currConversationNum = 0;
        TextManager.Setup();        

        if(!isStartConvo)
        {
            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                startSceneConverstion = PirateShipCusceneConversation();
            }

            if (SceneManager.GetActiveScene().buildIndex == 2)
            {
                startSceneConverstion = PirateShipBeforeStartingConversation();
            }

            if (SceneManager.GetActiveScene().buildIndex != 1 && SceneManager.GetActiveScene().buildIndex != 2)
            {
                startSceneConverstion = TutorialSceneConversation();
            }

            for (int i = 0; i < startSceneConverstion[currConversationNum].lines.Length; i++)
            {
                TextManager.textSets[i] = startSceneConverstion[currConversationNum].lines[i];
            }
            currLine = TextManager.textSets[currConversationNum];

            SetNewVoice(startSceneConverstion[currConversationNum].characterData[TextManager.currentTextSet].characterIdNum);
        }
        if(isStartConvo)
        {
            allConversations = ProduceConversations();

            for (int i = 0; i < allConversations[currConversationNum].lines.Length; i++)
            {
                TextManager.textSets[i] = allConversations[currConversationNum].lines[i];
            }
            currLine = TextManager.textSets[currConversationNum];

            SetNewVoice(allConversations[currConversationNum].characterData[TextManager.currentTextSet].characterIdNum);
        }
    }

    void Update()
    {
        string prevLine = currLine;
        if (!TextManager.textViewEmptied)
        {
            currLine = TextManager.textSets[TextManager.currentTextSet];
            if (!isStartConvo)
            {
                if (prevLine != currLine)
                {
                    SetNewVoice(startSceneConverstion[currConversationNum].characterData[TextManager.currentTextSet].characterIdNum);
                }
            }
            if (isStartConvo)
            {
                if (prevLine != currLine)
                {
                    SetNewVoice(allConversations[currConversationNum].characterData[TextManager.currentTextSet].characterIdNum);
                }
            }
            TextManager.RunText();
        }
        else
        {
            dialogueBox.SetActive(false);
        }

        //condition under which the conversation changes
        if (!isStartConvo)
        {
            talkingCharacter = startSceneConverstion[currConversationNum].characterData[TextManager.currentTextSet];
        }
        if (isStartConvo)
        {
            talkingCharacter = allConversations[currConversationNum].characterData[TextManager.currentTextSet];
        }

        charPortrait.talkingCharacter = talkingCharacter;
        charPortrait.UpdatePortrait();
        if (TextManager.textViewEmptied && Input.GetAxis("RunConversation") > 0 && !ranSecondDialogue)
        {
            currConversationNum = 1;
            TextManager.textViewEmptied = false;
            TextManager.currentTextSet = 0;
            if (!isStartConvo)
            {
                for (int i = 0; i < startSceneConverstion[currConversationNum].lines.Length; i++)
                {
                    TextManager.textSets[i] = startSceneConverstion[currConversationNum].lines[i];
                }
                SetNewVoice(startSceneConverstion[currConversationNum].characterData[TextManager.currentTextSet].characterIdNum);
            }
            if (isStartConvo)
            {
                for (int i = 0; i < allConversations[currConversationNum].lines.Length; i++)
                {
                    TextManager.textSets[i] = allConversations[currConversationNum].lines[i];
                    SetNewVoice(allConversations[currConversationNum].characterData[TextManager.currentTextSet].characterIdNum);
                }
            }
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

    public Conversation[] PirateShipCusceneConversation()
    {
        int startConvoLength = 19;
        convoWords = new string[startConvoLength];
        CharacterData[] convoCharData = new CharacterData[startConvoLength];
        convoCharData[0] = hally.GetComponent<CharacterData>();
        convoCharData[1] = meda.GetComponent<CharacterData>();
        convoCharData[2] = kent.GetComponent<CharacterData>();
        convoCharData[3] = kent.GetComponent<CharacterData>();
        convoCharData[4] = kent.GetComponent<CharacterData>();
        convoCharData[5] = meda.GetComponent<CharacterData>();
        convoCharData[6] = hally.GetComponent<CharacterData>();
        convoCharData[7] = meda.GetComponent<CharacterData>();
        convoCharData[8] = kent.GetComponent<CharacterData>();
        convoCharData[9] = jade.GetComponent<CharacterData>();
        convoCharData[10] = meda.GetComponent<CharacterData>();
        convoCharData[11] = kent.GetComponent<CharacterData>();
        convoCharData[12] = meda.GetComponent<CharacterData>();
        convoCharData[13] = kent.GetComponent<CharacterData>();
        convoCharData[14] = ed.GetComponent<CharacterData>();
        convoCharData[15] = kent.GetComponent<CharacterData>();
        convoCharData[16] = jade.GetComponent<CharacterData>();
        convoCharData[17] = ed.GetComponent<CharacterData>();
        convoCharData[18] = ed.GetComponent<CharacterData>();
        convoWords[0] = "Where....Are we?";
        convoWords[1] = "Aghh! Pirates!";
        convoWords[2] = "What? You’re crazy Meda. There aren’t any pir--";
        convoWords[3] = "...";
        convoWords[4] = "Meda, there are pirates! Why didn’t you tell us before?";
        convoWords[5] = "We’re on a pirate ship!! A plank on the port side, cannons on the starboard. There’s no doubt about it!";
        convoWords[6] = "Star....Board?";
        convoWords[7] = "*sigh* The right side of the ship. What are we going to do?";
        convoWords[8] = "Awww! Looks like the babies are playing their baby games again. Heh… I suppose I’ll play along with your \"Pirate Ship.\"";
        convoWords[9] = "Well, I’m excited to fight some pirates!";
        convoWords[10] = "All I know is that this means we’re somewhere in the Caribbean sea.";
        convoWords[11] = "And how would you know that?";
        convoWords[12] = "Because when you are busy being a jerk to Ed, I read. And one of my books was about the seven seas.";
        convoWords[13] = "...";
        convoWords[14] = "Meda is right. We are on a pirate ship.";
        convoWords[15] = "Well well well! Look who decided to speak up. So, how about that lunch money?";
        convoWords[16] = "Hush Kent. Now’s not the time. Ed, how do you know all this is real?";
        convoWords[17] = "See for yourselves.";
        convoWords[18] = "Hmmm...";


        Conversation convo = new Conversation(convoWords, startConvoLength, convoCharData);

        Conversation[] startConvo = new Conversation[1];
        startConvo[0] = convo; 
        return startConvo;
    }

    public Conversation[] PirateShipBeforeStartingConversation()
    {
        int startConvoLength = 11;
        convoWords = new string[startConvoLength];
        CharacterData[] convoCharData = new CharacterData[startConvoLength];
        convoCharData[0] = hally.GetComponent<CharacterData>();
        convoCharData[1] = meda.GetComponent<CharacterData>();
        convoCharData[2] = hally.GetComponent<CharacterData>();
        convoCharData[3] = jade.GetComponent<CharacterData>();
        convoCharData[4] = meda.GetComponent<CharacterData>();
        convoCharData[5] = hally.GetComponent<CharacterData>();
        convoCharData[6] = kent.GetComponent<CharacterData>();
        convoCharData[7] = meda.GetComponent<CharacterData>();
        convoCharData[8] = meda.GetComponent<CharacterData>();
        convoCharData[9] = jade.GetComponent<CharacterData>();
        convoCharData[10] = jade.GetComponent<CharacterData>();
        convoWords[0] = "Ohhhh, I love pirates! I’m the captain.";
        convoWords[1] = "Looks like the ship already has a captain, Hally.";
        convoWords[2] = "Daw...";
        convoWords[3] = "Oh look! That pirate is looking right at us! Hello there Mr. Pirate!!";
        convoWords[4] = "It looks like if we defeat the pirate captain or hold the wheel, we’ll take control of the ship.";
        convoWords[5] = "Huh... that’s oddly kind of the captain to give us the wheel.";
        convoWords[6] = "I don’t think he’s giving it to us.";
        convoWords[7] = "Does everyone remember when we surrounded Kent? Let’s surround the pirates as well!";
        convoWords[8] = "And maybe if we surround those generators on the sides of the ship, we’ll be able to move faster!";
        convoWords[9] = "You had me on board at “move faster.” Let’s surround those generators! And next, the pirates!";


        Conversation convo = new Conversation(convoWords, startConvoLength, convoCharData);

        Conversation[] startConvo = new Conversation[1];
        startConvo[0] = convo;
        return startConvo;
    }

    public Conversation[] TutorialSceneConversation()
    {
        int startConvoLength = 2;
        convoWords = new string[startConvoLength];
        CharacterData[] convoCharData = new CharacterData[startConvoLength];
        convoCharData[0] = meda.GetComponent<CharacterData>();
        convoCharData[1] = meda.GetComponent<CharacterData>();
        convoWords[0] = "I just want to say onew thing.";
        convoWords[1] = "That's it.";


        Conversation convo = new Conversation(convoWords, startConvoLength, convoCharData);

        Conversation[] startConvo = new Conversation[1];
        startConvo[0] = convo;
        return startConvo;
    }

    // This is for automatic dialogue box appears in the interval 0f 10secs.
    IEnumerator DialogueConversation()
    {
        yield return new WaitForSeconds(5);
        TextManager.endConversation = true;

        yield return new WaitForSeconds(5);
        TextManager.endConversation = true;
    }


    

    
}
