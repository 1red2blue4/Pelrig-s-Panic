using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonCrossbarController : MonoBehaviour {

    [SerializeField]
    public Texture2D mouseTarget;

    List<Cannon> cannons;
    bool cannonSelected;
	// Use this for initialization
	void Start () {
        cannons = new List<Cannon>();
	}
	
	// Update is called once per frame
	void Update () {
		if (PlayerControls.isPlayerTurn)
        {
            cannonSelected = false;
            for (int i = 0; i < Board.allCannons.Length; i++)
            {
                if (Board.allCannons[i].GetComponent<Cannon>().isCanonSelected)
                {
                    cannonSelected = true;
                    break;
                }
            }

            if (cannonSelected)
            {
                Cursor.SetCursor(mouseTarget, Vector2.zero, CursorMode.Auto);
            }
            else
            {
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            }
        }

        
	}
}
