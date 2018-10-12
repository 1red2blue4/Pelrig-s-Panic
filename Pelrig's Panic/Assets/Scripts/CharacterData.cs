using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData : MonoBehaviour {

    [SerializeField] public Ability[] characterAbilities;
    public int numCharacterAbilities = 5;
    public int abilityCapNum = 3;
    //2 for demo
    public int numOwnedAbilities = 2;
    [SerializeField] public Texture2D characterPortrait;
    //1:Kent; 2:Hally; 3:Ed; 4:Meda; 5:Jade
    [SerializeField] public int characterIdNum;
}
