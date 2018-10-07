using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIControllerRealWorld : MonoBehaviour {

    [SerializeField] public CharacterData selectedCharacter;
    [SerializeField] public Image uiCharacterImage;
    [SerializeField] public Image[] uiAbilityImages;
    [SerializeField] public GameObject[] allPossibleUIImageObjs;
    public GameObject[] visibleGameObjs;
    public float arbitraryDifferenceInValueAndIHaveNoIdeaWhyItExists;
    private bool isMovingObject;

	// Use this for initialization
	void Start () {
        isMovingObject = false;
        arbitraryDifferenceInValueAndIHaveNoIdeaWhyItExists = 909;
        uiAbilityImages = new Image[5];
        visibleGameObjs = new GameObject[5];
        float positionDown = 150.0f;
        for (int i = 0; i < uiAbilityImages.Length; i++)
        {
            bool lookingForOff = false;
            uiAbilityImages[i] = selectedCharacter.characterAbilities[i].abilityImage;
            if (i < selectedCharacter.abilityCapNum)
            {
                positionDown += 60.0f;
            }
            else
            {
                positionDown += 100.0f;
                lookingForOff = true;
            }
            GameObject seekedObj = null;
            if (!lookingForOff)
            {
                for (int j = 0; j < allPossibleUIImageObjs.Length; j++)
                {
                    if (selectedCharacter.characterAbilities[i].id == allPossibleUIImageObjs[j].GetComponent<Ability>().id)
                    {
                        seekedObj = allPossibleUIImageObjs[j];
                    }
                }
            }
            else
            {
                for (int j = 0; j < allPossibleUIImageObjs.Length; j++)
                {
                    if (-selectedCharacter.characterAbilities[i].id == allPossibleUIImageObjs[j].GetComponent<Ability>().id)
                    {
                        seekedObj = allPossibleUIImageObjs[j];
                    }
                }
            }
            visibleGameObjs[i] = seekedObj;
            seekedObj.GetComponent<RectTransform>().position = new Vector3(60.0f, -positionDown + arbitraryDifferenceInValueAndIHaveNoIdeaWhyItExists, 0.0f);
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!isMovingObject)
        {
            UpdatePosition();
        }
        CheckForObjectMove();
    }

    public void UpdatePosition()
    {
        float positionDown = 150.0f;
        for (int i = 0; i < uiAbilityImages.Length; i++)
        {
            if (i < selectedCharacter.abilityCapNum)
            {
                positionDown += 60.0f;
            }
            else
            {
                positionDown += 100.0f;
            }

            visibleGameObjs[i].GetComponent<RectTransform>().position = new Vector3(60.0f, -positionDown + arbitraryDifferenceInValueAndIHaveNoIdeaWhyItExists, 0.0f);
        }
    }

    public void CheckForObjectMove()
    {
        RaycastHit2D hit;

        Ray ray = gameObject.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        hit = Physics2D.Raycast(transform.position, ray.direction, Mathf.Infinity);
        if (hit.collider != null)
        {
            Debug.Log(hit.collider.name);
        }

    }
}
