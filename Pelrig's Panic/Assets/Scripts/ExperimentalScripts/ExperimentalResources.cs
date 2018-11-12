using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentalResources : MonoBehaviour {
    [SerializeField]public static int resources = 40;
    public static int mainResources = 40;
    public static int generatorsActive = 0;

	// Use this for initialization
	void Start () {
		
	}

    public static bool ModifyResource(int amount)
    {
        if (resources - amount >= 0)
        {
            resources -= amount;
            //Debug.Log("PlayerControls.moveValues[theNumber].ToString()):        " + resources);
            return true;
        }
        
        return false;
    }

    public static void ReInitializeResources()
    {
        ExperimentalResources.resources = ExperimentalResources.mainResources + (10* ExperimentalResources.generatorsActive);
    }
}
