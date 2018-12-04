using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour {

    [SerializeField]
    public Texture2D mouseTarget;
    int charges;
    [SerializeField] public Piece cannon;
    [SerializeField] public int cannonID;
    [SerializeField] public GameObject onImage;
    [SerializeField] public GameObject offImage;
    CannonRadius cannonRadius;
    public bool isCanonUsable;
    public bool isCanonSelected;
    //public CannonPopup cannonPopup;
    int theOne;


    private void Start()
    {
        isCanonUsable = false;
        charges = 1;
        isCanonSelected = false;
        cannonRadius = GetComponent<CannonRadius>();
    }

    private void Update()
    {
        isCanonUsable = CheckForPlayersAround();
        if (PlayerControls.isPlayerTurn && charges > 0)
        {
            if (Input.GetMouseButtonDown(0) && isCanonUsable)
            {
                if (isCanonSelected)
                {
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    
                    LayerMask layermask = LayerMask.GetMask("Enemy") | LayerMask.GetMask("Interactable");
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, layermask))
                    {
                        if (hit.collider.tag == "Enemy")
                        {
                            UseCannon(hit.collider.gameObject.GetComponent<Piece>());
                        }
                        else if (hit.transform.tag == "Cannon")
                        {
                            if (hit.collider.gameObject == gameObject)
                            {
                                return;
                            }
                        }
                    }
                    isCanonSelected = false;
                    cannonRadius.RemoveHighlights();
                    //Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                    offImage.SetActive(false);
                }
                else
                {
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1))
                    {
                        if (hit.transform.tag == "Cannon")
                        {
                            if (hit.collider.gameObject == gameObject)
                            {

                                isCanonSelected = true;
                                cannonRadius.HighlightGrids();
                                //Cursor.SetCursor(mouseTarget, Vector2.zero, CursorMode.Auto);
                                offImage.SetActive(true);
                            }
                        }
                    }
                }
            }
        }
        else if (isCanonSelected)
        {
            isCanonSelected = false;
            cannonRadius.RemoveHighlights();
            offImage.SetActive(false);
        }
        
        if (isCanonUsable && charges > 0)
        {
            //offImage.SetActive(true);
            Popup.isVisible = true;
        }
        else
        {
            Popup.isVisible = false;
        }
    }

    //Check players right round canon
    bool CheckForPlayersAround()
    {
        foreach (var playerObjects in Board.possibleMoveableChars)
        {
            if (playerObjects.rowPosition <= cannon.rowPosition + 1 && playerObjects.rowPosition >= cannon.rowPosition - 1 &&
                playerObjects.colPosition <= cannon.colPosition + 1 && playerObjects.colPosition >= cannon.colPosition - 1)
            {
                return true;
            }
        }
        return false;
    }

    //Destroy any enemy 5 spaces away
    void UseCannon(Piece enemy)
    {
        if (charges > 0 &&
            enemy.rowPosition <= cannon.rowPosition + cannonRadius.cannonRadius && enemy.rowPosition >= cannon.rowPosition - cannonRadius.cannonRadius &&
            enemy.colPosition <= cannon.colPosition + cannonRadius.cannonRadius && enemy.colPosition >= cannon.colPosition - cannonRadius.cannonRadius)
        {
            for (int i = 0; i < Board.spawnedEnemies.Count; i++)
            {
                if (enemy == Board.spawnedEnemies[i])
                {
                    Destroy(enemy.gameObject);
                    Board.spawnedEnemies.RemoveAt(i);
                    charges--;
                    return;
                }
            }         
        }
    }
}