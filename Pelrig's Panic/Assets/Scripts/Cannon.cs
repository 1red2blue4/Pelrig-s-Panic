using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour {
    
    int charges;
    [SerializeField] public Piece cannon;
    [SerializeField] public int cannonID;
    public bool isCanonUsable;

    private void Start()
    {
        isCanonUsable = true;
        charges = 1;
    }

    private void Update()
    {
        if (PlayerControls.isPlayerTurn && charges > 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (isCanonUsable)
                {
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                    {
                        if (hit.collider.tag == "Enemy")
                        {
                            UseCannon(hit.collider.gameObject.GetComponent<Piece>());
                        }
                        else if (hit.transform.tag == "Cannon")
                        {
                            if (hit.transform.parent.gameObject == gameObject)
                            {
                                return;
                            }
                        }
                    }
                    isCanonUsable = false;
                }
                else
                {
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                    {
                        if (hit.transform.tag == "Cannon")
                        {
                            if (hit.transform.parent.gameObject == gameObject)
                            {
                                isCanonUsable = CheckForPlayersAround();
                            }
                        }
                    }
                }
            }
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