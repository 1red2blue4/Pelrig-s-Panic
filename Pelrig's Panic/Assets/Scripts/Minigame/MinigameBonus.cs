using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameBonus : MonoBehaviour
{
    [SerializeField] private string typeOfMinigame;

    private void Start()
    {
        SetLocation();
    }

    private void Update()
    {
        
    }

    private void SetLocation()
    {
        if (typeOfMinigame == "Hally")
        {
            int rand = (int)Mathf.Floor(Random.value * 3.0f);
            if (rand == 0)
            {
                gameObject.transform.localPosition = new Vector3(16.0f, 9.5f, 1.0f);
            }
            else if (rand == 1)
            {
                gameObject.transform.localPosition = new Vector3(16.0f, -7.0f, 1.0f);
            }
            else
            {
                gameObject.transform.localPosition = new Vector3(-16.0f, 0.0f, 1.0f);
            }
        }
    }
}
