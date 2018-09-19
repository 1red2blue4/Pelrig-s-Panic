using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenWipeAnimation : MonoBehaviour {

    private float alpha;
    private Color savedSecondColor;
    private bool colorSwapped;
    private float distBetweenColors;

	// Use this for initialization
	void Start ()
    {
        alpha = 0.0001f;
        savedSecondColor = new Color();
        colorSwapped = false;
        distBetweenColors = 0.0f;
	}
	
	// Update is called once per frame
	void Update ()
    {
        WipeScreen(0.006f);
	}

    public void WipeScreen(float rate)
    {
        Color secondColor = new Color();
        Color emptyColor = new Color();
        Color spriteColor = gameObject.GetComponent<MeshRenderer>().materials[0].color;
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
}
