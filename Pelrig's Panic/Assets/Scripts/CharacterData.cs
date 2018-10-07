using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData : MonoBehaviour {

    [SerializeField] public Ability[] characterAbilities;
    public int numCharacterAbilities = 5;
    public int abilityCapNum = 3;
}
