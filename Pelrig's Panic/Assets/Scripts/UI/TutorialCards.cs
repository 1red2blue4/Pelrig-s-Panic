using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialCards : MonoBehaviour {

    public List<Sprite> cards = new List<Sprite>();
    public static bool isTutorialRunning;
    bool changeCards;
    bool card1, card2;
    int iterator;
    bool isEnabled;
    float timer;
    bool lastTutorial;

	// Use this for initialization
	void Start () {
        transform.GetChild(0).GetComponent<Image>().enabled = false;
        transform.GetChild(1).GetComponent<Image>().enabled = false;
        transform.GetChild(2).GetComponent<Button>().enabled = false;
        transform.GetChild(2).GetComponent<Image>().enabled = false;
        transform.GetChild(2).GetChild(0).GetComponent<Text>().enabled = false;
        transform.GetChild(2).GetChild(0).GetComponent<Text>().text = "Next";
        isTutorialRunning = false;
        iterator = 0;
        changeCards = true;
        isEnabled = false;
        timer = 0.0f;
        card1 = true;
        card2 = true;
        lastTutorial = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (!isEnabled && !TextManager.playerControlsLocked)
        {
            isTutorialRunning = true;
            isEnabled = true;
        }
		if (isTutorialRunning)
        {
            if (changeCards)
            {
                timer += Time.deltaTime;
                if (card1)
                {
                    FillSlot(0);
                    iterator += 1;
                    card1 = false;
                }
                if (timer > 1.0f && card2)
                {
                    FillSlot(1);
                    iterator += 1;
                    card2 = false;
                }
                if (iterator >= cards.Count)
                {
                    lastTutorial = true;
                    transform.GetChild(2).GetChild(0).GetComponent<Text>().text = "Lets Play!";
                }

                float timeIncrement = Time.deltaTime * 1.6f;
                transform.GetChild(0).localScale += new Vector3(timeIncrement, timeIncrement, timeIncrement);
                transform.GetChild(1).localScale += new Vector3(timeIncrement, timeIncrement, timeIncrement);

                if (!card1 && transform.GetChild(0).localScale.x >= 2.0f)
                {
                    transform.GetChild(0).localScale = new Vector3(2.0f, 2.0f, 2.0f);
                }
                if (!card2 && transform.GetChild(1).localScale.x >= 2.0f)
                {
                    transform.GetChild(1).localScale = new Vector3(2.0f, 2.0f, 2.0f);
                    transform.GetChild(2).GetComponent<Button>().enabled = true;
                    transform.GetChild(2).GetComponent<Image>().enabled = true;
                    transform.GetChild(2).GetChild(0).GetComponent<Text>().enabled = true;
                    changeCards = false;
                }

            }
        }
	}

    public void NextRound()
    {
        transform.GetChild(2).GetChild(0).GetComponent<Text>().enabled = false;
        transform.GetChild(0).GetComponent<Image>().enabled = false;
        transform.GetChild(1).GetComponent<Image>().enabled = false;
        transform.GetChild(2).GetComponent<Button>().enabled = false;
        transform.GetChild(2).GetComponent<Image>().enabled = false;
        if (lastTutorial)
        {
            isTutorialRunning = false;
        }
        else
        {
            changeCards = true;
            card1 = true;
            card2 = true;
            timer = 0.0f;
        }
    }

    void FillSlot(int childNumber)
    {
        Image tutorialSpace = transform.GetChild(childNumber).GetComponent<Image>();
        tutorialSpace.sprite = cards[iterator];
        tutorialSpace.enabled = true;
        transform.GetChild(childNumber).localScale = new Vector3(0.1f, 0.1f, 0.1f);
    }
}