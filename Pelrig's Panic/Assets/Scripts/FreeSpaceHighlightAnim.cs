using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeSpaceHighlightAnim : MonoBehaviour
{
    public bool isVisible;
    // Use this for initialization
    void Start()
    {
        isVisible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isVisible)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
