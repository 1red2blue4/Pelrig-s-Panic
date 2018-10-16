using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextDisplay : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (PlayerControls.isPlayerTurn)
        {
            transform.GetComponent<TMPro.TextMeshProUGUI>().text = ExperimentalResources.resources.ToString();
        }
        else
        {
            transform.GetComponent<TMPro.TextMeshProUGUI>().text = null;
        }
	}
}
