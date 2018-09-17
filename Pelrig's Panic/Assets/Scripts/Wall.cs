using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {

    [SerializeField] public bool dependent;
    [SerializeField] public MovementManager.Direction deadDirection;
    [SerializeField] public Wall[] wallDependencies;

	// Use this for initialization
	void Start ()
    {
		
	}

    public bool CalcIfOutside(Vector3 objLocation)
    {
        if (dependent)
        {
            return false;
        }
        bool outsideOtherWalls = false;
        int count = 0;
        for (int i = 0; i < wallDependencies.Length; i++)
        {
            if (OutsideCertainWall(wallDependencies[i], objLocation))
            {
                count++;
            }
        }
        if (count >= wallDependencies.Length)
        {
            outsideOtherWalls = true;
        }
        if (outsideOtherWalls && OutsideCertainWall(this, objLocation))
        {
            return true;
        }
        return false;
    }

    public bool OutsideCertainWall(Wall wall, Vector3 objLocation)
    {
        if (wall.deadDirection == MovementManager.Direction.Up)
        {
            if (objLocation.y > wall.gameObject.transform.position.y - 0.9f)
            {
                return true;
            }
        }
        else if (wall.deadDirection == MovementManager.Direction.Right)
        {
            if (objLocation.x > wall.gameObject.transform.position.x - 0.9f)
            {
                return true;
            }
        }
        else if (wall.deadDirection == MovementManager.Direction.Down)
        {
            if (objLocation.y < wall.gameObject.transform.position.y + 0.9f)
            {
                return true;
            }
        }
        else if (wall.deadDirection == MovementManager.Direction.Left)
        {
            if (objLocation.x < wall.gameObject.transform.position.x + 0.9f)
            {
                return true;
            }
        }
        return false;
    }
}
