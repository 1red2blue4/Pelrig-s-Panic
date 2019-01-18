using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameBonus : MonoBehaviour
{
    [SerializeField] private string typeOfMinigame;
    [SerializeField] private GameObject flipper;
    [SerializeField] private GameObject player;

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
                gameObject.transform.localPosition = new Vector3(17.0f, 9.5f, 1.0f);
            }
            else if (rand == 1)
            {
                gameObject.transform.localPosition = new Vector3(17.0f, -7.0f, 1.0f);
            }
            else
            {
                gameObject.transform.localPosition = new Vector3(-17.0f, 1.0f, 1.0f);
            }
        }


        if (typeOfMinigame == "Jade")
        {
            int rand = (int)Mathf.Floor(Random.value * 4.0f);
            if (rand == 0)
            {
                //keep the board as is
            }
            else if (rand == 1)
            {
                flipper.transform.Rotate(0.0f, 0.0f, 90.0f);
            }
            else if (rand == 2)
            {
                flipper.transform.Rotate(0.0f, 0.0f, 180.0f);
            }
            else
            {
                flipper.transform.Rotate(0.0f, 0.0f, 270.0f);
            }

            //need to unparent the player so that it can move freely about the cabin
            player.transform.parent = player.transform.parent.parent;
        }

        if (typeOfMinigame == "Meda")
        {
            int rand = (int)Mathf.Floor(Random.value * 8.0f);
            if (rand >= 0 && rand <= 3)
            {
                gameObject.transform.localPosition = new Vector3(0.0f, 0.0f, 1.0f);
            }
            else if (rand == 4)
            {
                gameObject.transform.localPosition = new Vector3(10.0f, 0.0f, 1.0f);
            }
            else if (rand == 5)
            {
                gameObject.transform.localPosition = new Vector3(-10.0f, 0.0f, 1.0f);
            }
            else if (rand == 6)
            {
                gameObject.transform.localPosition = new Vector3(0.0f, 10.0f, 1.0f);
            }
            else
            {
                gameObject.transform.localPosition = new Vector3(0.0f, -10.0f, 1.0f);
            }
        }
    }

}
