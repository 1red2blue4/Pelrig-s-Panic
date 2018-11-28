using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AtlasManager : MonoBehaviour, DialogueStateManager
{
    public static Sprite[] sprites;
    public ManagerState currentState
    {
        get;
        private set;
    }

    public void BootSequence()
    { 
        sprites = Resources.LoadAll<Sprite>("EventAtlas");
        currentState = ManagerState.completed;
         
    }

    public Sprite loadSprite(string spriteName)
    {
        foreach (Sprite S in sprites)
        {
            if(S.name == spriteName)
            {
                return S;
            }
        }
        return null;
    }
	 
}
