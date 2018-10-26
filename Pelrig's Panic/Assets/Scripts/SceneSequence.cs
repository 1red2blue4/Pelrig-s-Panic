using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSequence : MonoBehaviour
{
    public GameObject cam1;
    public GameObject cam2;
    public GameObject cam3;
    
	// Use this for initialization
	void Start () {
        StartCoroutine(Sequence());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    IEnumerator Sequence()
    {
        yield return new WaitForSeconds(5);
        cam2.SetActive(true);
        cam1.SetActive(false);

        yield return new WaitForSeconds(5);
        cam3.SetActive(true);
        cam2.SetActive(false);
        

        yield return new WaitForSeconds(8);
        SceneTransition();
    }

    void SceneTransition()
    {
        SceneManager.LoadScene("PirateShipWithBoard");
    }
}
