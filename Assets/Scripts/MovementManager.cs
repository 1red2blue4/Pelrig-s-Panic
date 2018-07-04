using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MovementManager {
    
    public enum Direction: int { Up = 0, Right = 1, Down = 2, Left = 3 };

    public static Direction[] directionLineup;
    public static Direction nextDirection;

    public static void Move(Piece character)
    {
        if (directionLineup[0] == Direction.Up)
        {
            character.transform.position += new Vector3(0.0f, 0.0f, 0.0f);
        }
    }

    public static void SetStartDirectionLineup()
    {
        //set the lineup of directions
        for (int i = 0; i < directionLineup.Length; i++)
        {
            //get one of the directions
            int numDirections = 4;
            int randNum = (int)Mathf.Floor(Random.value * (float)numDirections);
            //on the off chance it rolls exactly 1, pick the largest value instead of overflowing
            if (randNum == numDirections)
            {
                randNum = numDirections - 1;
            }
            //set the direction
            Direction setDirection = Direction.Up;
            switch (randNum)
            {
                case 0:
                    setDirection = Direction.Up;
                    break;
                case 1:
                    setDirection = Direction.Right;
                    break;
                case 2:
                    setDirection = Direction.Down;
                    break;
                case 3:
                    setDirection = Direction.Left;
                    break;
                default:
                    setDirection = Direction.Up;
                    break;

            }
            directionLineup[i] = setDirection;
        }
    }

    public static void FillOneSpotInLineup()
    {
        //set the lineup of directions
        for (int i = 0; i < directionLineup.Length; i++)
        {

            if (i != directionLineup.Length - 1)
            {
                directionLineup[i] = directionLineup[i + 1];
            }
            else
            {
                //get one of the directions
                int numDirections = 4;
                int randNum = (int)Mathf.Floor(Random.value * (float)numDirections);
                //on the off chance it rolls exactly 1, pick the largest value instead of overflowing
                if (randNum == numDirections)
                {
                    randNum = numDirections - 1;
                }
                //set the direction
                Direction setDirection = Direction.Up;
                switch (randNum)
                {
                    case 0:
                        setDirection = Direction.Up;
                        break;
                    case 1:
                        setDirection = Direction.Right;
                        break;
                    case 2:
                        setDirection = Direction.Down;
                        break;
                    case 3:
                        setDirection = Direction.Left;
                        break;
                    default:
                        setDirection = Direction.Up;
                        break;

                }
                directionLineup[i] = setDirection;
            }
        }
    }
}
