using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStuff : MonoBehaviour {

    [SerializeField] int theNumber;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.GetComponent<TMPro.TextMeshProUGUI>().text = PlayerControls.moveValues[theNumber].ToString();
    }
}
