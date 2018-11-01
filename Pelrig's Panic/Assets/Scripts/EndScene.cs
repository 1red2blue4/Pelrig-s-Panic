using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScene : MonoBehaviour
{
    public GameObject camera1;
    public GameObject youLose;

	// Use this for initialization
	void Start ()
    {
        StartCoroutine(EndSceneSequence());
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    IEnumerator EndSceneSequence()
    {
        yield return new WaitForSeconds(3);
        youLose.SetActive(true);
    }
}
