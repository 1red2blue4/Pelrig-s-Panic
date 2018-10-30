using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UniversalControls : MonoBehaviour {

    private float escapeTimer;
    private float restartTimer;
    [SerializeField] private bool canSkip;
    private bool escaping;
    private bool restarting;
    [SerializeField] private GameObject prompt;
    [SerializeField] private bool restartable;

    void Start()
    {
        escapeTimer = 0.0f;
        restartTimer = 0.0f;
    }

    // Update is called once per frame
    void Update ()
    {
        if (!restarting)
        {
            CheckExitGame();
        }
        if (restartable && !escaping)
        {
            CheckRestartLevel();
        }

        if (Input.GetKeyDown(KeyCode.S) && canSkip)
        {
            SceneManager.LoadScene("PirateShipWithBoard");
        }
    }

    public void CheckExitGame()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            escaping = true;
        }
        else
        {
            escaping = false;
        }

        if (escaping)
        {
            prompt.SetActive(true);
            escapeTimer += Time.deltaTime;

            if (escapeTimer >= 0.0f)
            {
                prompt.GetComponent<Text>().text = "Exiting.";
                if (escapeTimer >= 1.0f)
                {
                    prompt.GetComponent<Text>().text = "Exiting..";
                    if (escapeTimer >= 2.0f)
                    {
                        prompt.GetComponent<Text>().text = "Exiting...";
                    }
                }
            }

            if (escapeTimer >= 3.0f)
            {
                Application.Quit();
            }
        }
        else
        {
            prompt.SetActive(false);
            escapeTimer = 0.0f;
        }
    }

    public void CheckRestartLevel()
    {
        if (Input.GetKey(KeyCode.R))
        {
            restarting = true;
        }
        else
        {
            restarting = false;
        }

        if (restarting)
        {
            prompt.SetActive(true);
            restartTimer += Time.deltaTime;

            if (restartTimer >= 0.0f)
            {
                prompt.GetComponent<Text>().text = "Restarting.";
                if (restartTimer >= 1.0f)
                {
                    prompt.GetComponent<Text>().text = "Restarting..";
                    if (restartTimer >= 2.0f)
                    {
                        prompt.GetComponent<Text>().text = "Restarting...";
                    }
                }
            }

            if (restartTimer >= 3.0f)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
        else
        {
            prompt.SetActive(false);
            restartTimer = 0.0f;
        }
    }
}
