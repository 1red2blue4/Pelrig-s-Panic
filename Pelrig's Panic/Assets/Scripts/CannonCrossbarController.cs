using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonCrossbarController : MonoBehaviour {

    [SerializeField]
    public Texture2D mouseTarget;

    List<Cannon> cannons;
    public static bool isCannonSelected;
	// Use this for initialization
	void Start () {
        cannons = new List<Cannon>();
	}
	
	// Update is called once per frame
	void Update () {
		if (PlayerControls.isPlayerTurn)
        {
            bool cannonSelected = false;
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
                if (!isCannonSelected)
                {
                    isCannonSelected = true;
                    Cursor.SetCursor(mouseTarget, Vector2.zero, CursorMode.Auto);
                }
            }
            else
            {
                if (isCannonSelected)
                {
                    isCannonSelected = false;
                    Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                }
            }
        }
        else if (isCannonSelected)
        {
            isCannonSelected = false;
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }

        
	}
}
