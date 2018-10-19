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
    private GameObject heldObject;

    GraphicRaycaster gRaycaster;
    PointerEventData pointerEventData;
    EventSystem eventSystem;

	// Use this for initialization
	void Start () {
        GameObject canvas = GameObject.FindGameObjectWithTag("Canvas");
        gRaycaster = canvas.GetComponent<GraphicRaycaster>();
        eventSystem = canvas.GetComponent<EventSystem>();

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
            if (i >= selectedCharacter.numOwnedAbilities)
            {
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
        if (heldObject == null)
        {
            CheckForObjectGrab();
        }
        if (heldObject != null)
        {
            MoveGrabbedObject();
        }
        UpdatePortrait();
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

    public void CheckForObjectGrab()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            pointerEventData = new PointerEventData(eventSystem);
            pointerEventData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();

            gRaycaster.Raycast(pointerEventData, results);

            if (results.Count > 0)
            {
                float closestDistanceSqrd = 100000.0f;
                int objNum = -10;
                Vector3 point = pointerEventData.position;

                for (int i = 0; i < results.Count; i++)
                {
                    if (results[i].gameObject.GetComponent<Ability>() == null)
                    {
                        continue;
                    }
                    Vector3 objectLoc = results[i].gameObject.transform.position;
                    float currDistance = (objectLoc.x - point.x) * (objectLoc.x - point.x) + (objectLoc.y - point.y) * (objectLoc.y - point.y);
                    if (currDistance < closestDistanceSqrd)
                    {
                        closestDistanceSqrd = currDistance;
                        objNum = i;
                    }
                }

                if (objNum != -10)
                {
                    heldObject = results[objNum].gameObject;
                }
            }
        }
    }

    public void MoveGrabbedObject()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            isMovingObject = true;
            pointerEventData = new PointerEventData(eventSystem);
            pointerEventData.position = Input.mousePosition;

            heldObject.transform.position = new Vector3(pointerEventData.position.x, pointerEventData.position.y, heldObject.transform.position.z);
        }
        else
        {
            if (heldObject.transform.position.x > visibleGameObjs[0].transform.position.x - 40.0f && heldObject.transform.position.x < visibleGameObjs[0].transform.position.x + 40.0f)
            {
                int numItemsBelow = 0;
                int heldItemPosition = -1;
                for (int i = 0; i < uiAbilityImages.Length; i++)
                {
                    if (heldObject == visibleGameObjs[i])
                    {
                        heldItemPosition = i;
                        continue;
                    }
                    //check how many objects it's below and if it's reasonably within distance
                    if (heldObject.transform.position.y < visibleGameObjs[i].transform.position.y &&
                        heldObject.transform.position.x > visibleGameObjs[i].transform.position.x - 40.0f && heldObject.transform.position.x < visibleGameObjs[i].transform.position.x + 40.0f)
                    {
                        numItemsBelow++;
                    }
                }

                GameObject tempObject = visibleGameObjs[numItemsBelow];

                visibleGameObjs[numItemsBelow] = heldObject;

                //1 for increased, 0 for no change, -1 for decreased
                int howItChanged;

                if (numItemsBelow < heldItemPosition)
                {
                    howItChanged = -1;
                }
                else if (numItemsBelow == heldItemPosition)
                {
                    howItChanged = 0;
                }
                else
                {
                    howItChanged = 1;
                }

                int diffChange = numItemsBelow - heldItemPosition;

                if (howItChanged == 0)
                {
                    //do nothing
                }
                else if (howItChanged == -1)
                {
                    for (int i = heldItemPosition; i > numItemsBelow; i--)
                    {
                        visibleGameObjs[i] = visibleGameObjs[i - 1];
                        if (i == numItemsBelow + 1)
                        {
                            visibleGameObjs[i] = tempObject;
                        }
                    }
                }
                else
                {
                    for (int i = heldItemPosition; i < numItemsBelow; i++)
                    {
                        visibleGameObjs[i] = visibleGameObjs[i + 1];
                        if (i == numItemsBelow - 1)
                        {
                            visibleGameObjs[i] = tempObject;
                        }
                    }
                }

                for (int i = 0; i < uiAbilityImages.Length; i++)
                {
                    uiAbilityImages[i] = visibleGameObjs[i].GetComponent<Image>();
                }

                
            }

            GameObject seekedObj = null;
            GameObject altObj = null;

            for (int i = 0; i < visibleGameObjs.Length; i++)
            {
                if (i >= selectedCharacter.numOwnedAbilities)
                {
                    for (int j = 0; j < allPossibleUIImageObjs.Length; j++)
                    {
                        if (-Mathf.Abs(visibleGameObjs[i].GetComponent<Ability>().id) == allPossibleUIImageObjs[j].GetComponent<Ability>().id)
                        {
                            seekedObj = allPossibleUIImageObjs[j];
                        }
                        if (Mathf.Abs(visibleGameObjs[i].GetComponent<Ability>().id) == allPossibleUIImageObjs[j].GetComponent<Ability>().id)
                        {
                            altObj = allPossibleUIImageObjs[j];
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < allPossibleUIImageObjs.Length; j++)
                    {
                        if (Mathf.Abs(visibleGameObjs[i].GetComponent<Ability>().id) == allPossibleUIImageObjs[j].GetComponent<Ability>().id)
                        {
                            seekedObj = allPossibleUIImageObjs[j];
                        }
                        if (-Mathf.Abs(visibleGameObjs[i].GetComponent<Ability>().id) == allPossibleUIImageObjs[j].GetComponent<Ability>().id)
                        {
                            altObj = allPossibleUIImageObjs[j];
                        }
                    }
                }

                visibleGameObjs[i] = seekedObj;
                uiAbilityImages[i] = seekedObj.GetComponent<Image>();
                altObj.transform.position = new Vector3(50000.0f, 50000.0f, 0.0f);
            }

            

            heldObject = null;
            isMovingObject = false;
        }
    }

    public void UpdatePortrait()
    {
        selectedCharacter = gameObject.GetComponent<RealWorldCamera>().selectedUnit.GetComponent<CharacterData>();
        Sprite resultingSprite = Sprite.Create(selectedCharacter.characterPortrait, new Rect(0.0f, 0.0f, 284.0f, 284.0f), new Vector2(0.0f, 0.0f));
        uiCharacterImage.sprite = resultingSprite;
    }
}
