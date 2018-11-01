using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelUnderCharacter : MonoBehaviour {

    public bool visible;

    private void Start()
    {
        visible = false;
    }

    void Update()
    {
        if (visible)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
