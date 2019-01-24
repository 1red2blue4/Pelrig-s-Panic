using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NarrativeEvent
{
    public List<Dialogue> dialogues;

}
public struct Dialogue
{
    public CharacterType Conversation;
    public string CharacterName;
    public string CharacterImage;
    public string DialogueText;
    public string FontName;
}
public enum CharacterType
{
    Ed, Hally, Kent, Jade, Meda
}
