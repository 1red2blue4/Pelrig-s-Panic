using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialoguePortrait : MonoBehaviour {

    public CharacterData talkingCharacter;
    public Image uiCharacterImage;
    [SerializeField] private Image dummyUICharImage;
   // [SerializeField] private SceneOfDialogueRealWorld sceneOfDialogue;
    [SerializeField] private Text nameBox;

    public void UpdatePortrait()
    {
        if (uiCharacterImage == null)
        {
            uiCharacterImage = dummyUICharImage;
        }
        Sprite resultingSprite = Sprite.Create(talkingCharacter.characterPortrait, new Rect(0.0f, 0.0f, 284.0f, 284.0f), new Vector2(0.0f, 0.0f));
        uiCharacterImage.sprite = resultingSprite;
        Debug.Log(talkingCharacter.name);
        nameBox.text = talkingCharacter.name;
    }
}
