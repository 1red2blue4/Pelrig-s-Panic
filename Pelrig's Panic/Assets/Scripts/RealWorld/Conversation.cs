using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conversation {

    public string[] lines;
    public int numLines;
    public CharacterData[] characterData;

    public Conversation(string[] lns, int nmLns, CharacterData[] chDta)
    {
        lines = lns;
        numLines = nmLns;
        characterData = chDta;
    }
}
