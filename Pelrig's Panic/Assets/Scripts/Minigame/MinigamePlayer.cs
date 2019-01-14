using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigamePlayer : MonoBehaviour
{

    public float speedScale;
    public bool winningGame;
    public Material winningColor;
    public Material notWinningColor;
    public GameObject[] backgroundObjects;

    public void Start()
    {
        speedScale = 4.0f;
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

        gameObject.transform.position += new Vector3(speedScale * horiz * Time.deltaTime, speedScale * vert * Time.deltaTime, 0.0f);
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
