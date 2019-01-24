using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaygroundControlManager : MonoBehaviour
{
   // [SerializeField]
    //private GameObject[] buttonHolder;

    private void OnMouseEnter()
    {
        Debug.Log("OnMouseEnter");
    }

    private void OnMouseExit()
    {
        Debug.Log("OnMouseExit");
    }
}
