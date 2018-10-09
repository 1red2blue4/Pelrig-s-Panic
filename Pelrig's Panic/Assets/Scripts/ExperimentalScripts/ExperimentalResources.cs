using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentalResources : MonoBehaviour {


    [SerializeField]static int resources = 1000;
	// Use this for initialization
	void Start () {
		
	}

    public static bool ModifyResource(int amount)
    {
        if (resources - amount > 0)
        {
            resources += amount;
            return true;
        }
        return false;
    }
}
