using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigamePlayer : MonoBehaviour
{

    [SerializeField] private float speedScale;
    public bool winningGame;
    [SerializeField] private Material winningColor;
    [SerializeField] private Material notWinningColor;
    public GameObject[] backgroundObjects;
    private int direction;

    public void Start()
    {
        direction = 0;
        speedScale = 20.0f;
        winningGame = false;
    }

    // Update is called once per frame
    public void Update()
    {
        Controls();
        CheckIfWinning();
    }

    public void Controls()
    {
        float vert = Input.GetAxis("Vertical");
        float horiz = Input.GetAxis("Horizontal");

        if (vert > 0)
        {
            direction = 1;
        }
        else if (vert < 0)
        {
            direction = 2;
        }
        if (horiz > 0)
        {
            direction = 3;
        }
        else if (horiz < 0)
        {
            direction = 4;
        }

        if (direction == 1)
        {
            transform.localPosition += new Vector3(0.0f, speedScale * Time.deltaTime, 0.0f);
        }
        else if (direction == 2)
        {
            transform.localPosition += new Vector3(0.0f, -speedScale * Time.deltaTime, 0.0f);
        }
        else if (direction == 3)
        {
            transform.localPosition += new Vector3(speedScale * Time.deltaTime, 0.0f, 0.0f);
        }
        else if (direction == 4)
        {
            transform.localPosition += new Vector3(-speedScale * Time.deltaTime, 0.0f, 0.0f);
        }
    }
    
    public void CheckIfWinning()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
        {
            if (hit.collider.GetComponent<MinigameBonus>() != null)
            {
                winningGame = true;
                for (int i = 0; i < backgroundObjects.Length; i++)
                {
                    backgroundObjects[i].GetComponent<Renderer>().material.color = winningColor.color;
                }
            }
            else
            {
                winningGame = false;
                for (int i = 0; i < backgroundObjects.Length; i++)
                {
                    backgroundObjects[i].GetComponent<Renderer>().material.color = notWinningColor.color;
                }
            }
        }
        else
        {
            winningGame = false;
            for (int i = 0; i < backgroundObjects.Length; i++)
            {
                backgroundObjects[i].GetComponent<Renderer>().material.color = notWinningColor.color;
            }
        }
    }

}
