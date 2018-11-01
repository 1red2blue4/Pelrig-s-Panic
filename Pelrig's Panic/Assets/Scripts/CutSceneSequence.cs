using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutSceneSequence : MonoBehaviour
{
    public GameObject camera1;
    public GameObject camera2;
    public GameObject camera3;
	// Use this for initialization
	void Start ()
    {
        StartCoroutine(SceneSequence());
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    IEnumerator SceneSequence()
    {
        yield return new WaitForSeconds(5);
        SceneTransition();
    }

    void SceneTransition()
    { 
        SceneManager.LoadScene("PirateShipWithBoard");
    }
}
