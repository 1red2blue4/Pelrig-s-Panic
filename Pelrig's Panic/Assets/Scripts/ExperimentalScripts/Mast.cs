using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mast : MonoBehaviour {

    Color inputMaterial;
    Color invisibleColor;
    private void Start()
    {
        inputMaterial = GetComponent<Renderer>().material.color;
        invisibleColor = Color.white;
        invisibleColor.a = 0;
    }

    private void OnMouseEnter()
    {
        Debug.Log("Enter");
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    private void OnMouseExit()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        GetComponent<Renderer>().material.SetColor("_Color", inputMaterial);
    }
}
