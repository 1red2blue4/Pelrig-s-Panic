using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTint : MonoBehaviour {

    Texture2D aTexture;
    Color fillColor;

    private void Start()
    {
        aTexture = new Texture2D(1, 1);
        fillColor = Color.red;
        fillColor.a = 0.06f;
        aTexture.SetPixel(0, 0, fillColor);
    }

    void OnGUI()
    {
        if (!PlayerControls.isPlayerTurn)
        {
            GUI.color = fillColor;
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), aTexture, ScaleMode.StretchToFill, true);
        }
    }
}
