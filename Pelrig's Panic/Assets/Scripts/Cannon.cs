using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour {

    [SerializeField]
    Texture2D mouseTarget;
    int charges;
    [SerializeField] public Piece cannon;
    [SerializeField] public int cannonID;
    [SerializeField] public GameObject onImage;
    [SerializeField] public GameObject offImage;
    public static bool isCanonUsable;
    public static bool isCanonSelected;
    int theOne;
    private void Start()
    {
        isCanonUsable = false;
        charges = 1;
        isCanonSelected = false;
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
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1))
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
                    Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
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
                                Cursor.SetCursor(mouseTarget, Vector2.zero, CursorMode.Auto);
                                offImage.SetActive(true);
                            }
                        }
                    }
                }
            }
        }
        
        if (isCanonUsable && charges > 0)
        {
            //offImage.SetActive(true);
            CannonPopup.isVisible = true;
        }
        else
        {
            CannonPopup.isVisible = false;
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
            enemy.rowPosition <= cannon.rowPosition + 5 && enemy.rowPosition >= cannon.rowPosition - 5 &&
            enemy.colPosition <= cannon.colPosition + 5 && enemy.colPosition >= cannon.colPosition - 5)
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