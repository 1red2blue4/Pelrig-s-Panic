using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame : MonoBehaviour
{

    private float timer;
    [SerializeField] private float timeAvailable;
    [SerializeField] private MinigamePlayer miniGamePlayer;
    public bool gameWon;
    public bool gameConditionSet;
    [SerializeField] private TextMesh timerText;

    // Start is called before the first frame update
    void Start()
    {
        gameConditionSet = false;
        OpenGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameConditionSet)
        {
            RunTimer();
        }
        AnnounceCondition();
    }

    public void OpenGame()
    {
        timer = timeAvailable;
    }

    public void RunTimer()
    {
        timer -= Time.deltaTime;

        if (timer <= 0.0f)
        {
            if (miniGamePlayer.winningGame)
            {
                gameWon = true;
            }
            else
            {
                gameWon = false;
            }
            gameConditionSet = true;
        }

        int timeLeftResult = (int)Mathf.Ceil(timer);
        timerText.text = timeLeftResult.ToString();
    }

    public void AnnounceCondition()
    {
        if (gameConditionSet)
        {
            if (gameWon)
            {
                Debug.Log("You won!");
            }
            else
            {
                Debug.Log("You lost!");
            }
        }
    }
}
