using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    public GameObject panel;
    Animator animator; 

    public void OpenPanel()
    {        
        if(panel != null)
        {
            animator = panel.GetComponent<Animator>();
            
            if (animator != null)
            {
               bool isOpen = animator.GetBool("Open");
                
                animator.SetBool("Open", !isOpen);
            }
        }        
    }
}
