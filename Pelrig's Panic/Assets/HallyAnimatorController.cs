using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallyAnimatorController : MonoBehaviour
{
    Animator aniamtor;
	// Use this for initialization
	void Start ()
    {
        aniamtor = GetComponent<Animator>();	
	}

    // Update is called once per frame
    void Update() {

        Debug.Log("PlayerControls.isWalk:       " + PlayerControls.isWalk);
        if (PlayerControls.isWalk)
        {
            aniamtor.SetBool("isWalking", true);
            //PlayerControls.isWalk = false;
        }
        
    
        else
        {
            aniamtor.SetBool("isWalking", false);
        }
            

	}
}
