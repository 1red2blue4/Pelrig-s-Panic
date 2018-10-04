using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScreenWipeAnimation : MonoBehaviour {

    public float alpha;
    private Color savedSecondColor;
    private bool colorSwapped;
    private float distBetweenColors;
    private bool choseFirstColor;
    private float timer;
    private bool sceneLoaded;

	// Use this for initialization
	void Start ()
    {
        alpha = 0.0f;
        savedSecondColor = new Color();
        colorSwapped = false;
        distBetweenColors = 0.0f;
        choseFirstColor = false;
        timer = 0.0f;
        sceneLoaded = false;
        Color spriteColor = gameObject.GetComponent<MeshRenderer>().materials[0].color;
        gameObject.GetComponent<MeshRenderer>().materials[0].color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, alpha);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (timer < 5.0f)
        {
            WipeScreen(0.01f);
        }
        else if (timer >= 5.0f && !sceneLoaded)
        {
            sceneLoaded = true;
            LoadScene("PlayGround");
        }
        timer += Time.deltaTime;
	}

    public void WipeScreen(float rate)
    {
        Color secondColor = new Color();
        Color emptyColor = new Color();
        Color spriteColor = gameObject.GetComponent<MeshRenderer>().materials[0].color;
        if (!choseFirstColor)
        {
            spriteColor = new Color(Random.value, Random.value, Random.value, alpha);
            choseFirstColor = true;
        }
        if (alpha < 1.0f)
        {
            gameObject.GetComponent<MeshRenderer>().materials[0].color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, alpha);
            alpha += rate;
        }
        else if (!colorSwapped)
        {
            alpha = 1.0f;
            secondColor = ChooseColor(new Color(spriteColor.r, spriteColor.g, spriteColor.b, 1.0f));
            savedSecondColor = secondColor;
            colorSwapped = true;
        }

        if (colorSwapped)
        {
            gameObject.GetComponent<MeshRenderer>().materials[0].color = Color.Lerp(spriteColor, savedSecondColor, distBetweenColors);
            distBetweenColors += rate;
            if (distBetweenColors >= 1)
            {
                distBetweenColors = 0.0f;
                secondColor = ChooseColor(new Color(spriteColor.r, spriteColor.g, spriteColor.b, 1.0f));
                savedSecondColor = secondColor;
            }
        }
    }

    public Color ChooseColor(Color currentColor)
    {
        float redResult = 0.0f;
        float greenResult = 0.0f;
        float blueResult = 0.0f;
        if (currentColor.r + 0.4 < 1)
        {
            redResult = currentColor.r + Random.value * 0.3f;
        }
        else if (currentColor.r - 0.4 > 0)
        {
            redResult = currentColor.r - Random.value * 0.3f;
        }
        else
        {
            redResult = currentColor.r;
        }

        if (currentColor.g + 0.4 < 1)
        {
            greenResult = currentColor.g + Random.value * 0.3f;
        }
        else if (currentColor.g - 0.4 > 0)
        {
            greenResult = currentColor.g - Random.value * 0.3f;
        }
        else
        {
            greenResult = currentColor.g;
        }

        if (currentColor.b + 0.4 < 1)
        {
            blueResult = currentColor.b + Random.value * 0.3f;
        }
        else if (currentColor.r - 0.4 > 0)
        {
            blueResult = currentColor.b - Random.value * 0.3f;
        }
        else
        {
            blueResult = currentColor.b;
        }

        return new Color(redResult, blueResult, greenResult, 1.0f);
    }

    public void LoadScene(int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadAsynchronously(sceneName));
    }

    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        while (operation.progress < 1.0f)
        {
            Debug.Log(operation.progress);

            WipeScreen(0.006f);

            yield return null;
        }
    }

    IEnumerator LoadAsynchronously(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while (operation.progress < 1.0f)
        {
            Debug.Log(operation.progress);

            WipeScreen(0.01f);

            yield return null;
        }
    }
}
