using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayOnHover : MonoBehaviour {

    [SerializeField] private GameObject objToDisappear;
    
    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject == gameObject)
            {
                objToDisappear.SetActive(true);
            }
            else
            {
                objToDisappear.SetActive(false);
            }
        }
    }
}
