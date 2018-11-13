using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mast : MonoBehaviour {

    private void OnMouseEnter()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    private void OnMouseExit()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = true;
    }
}
