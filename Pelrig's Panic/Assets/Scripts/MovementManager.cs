using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MovementManager {
    
    public enum Direction: int { Up = 0, Right = 1, Down = 2, Left = 3 };

    public static Direction[,] directionLineups;
    public static int numDirectionsInLineup;
    public static int numDirectionLineups;
    public static int currentDirectionLineup;
    public static Direction nextDirection;

    public static bool Move(Piece character, int direction, int cost)
    {
        if (direction == 1)
        {
            bool inWay = false;
            //check for other characters in the way
            for (int i = 0; i < Board.possibleMoveableChars.Length; i++)
            {
                if (Board.possibleMoveableChars[i] == character)
                {
                    continue;
                }
                if (Board.possibleMoveableChars[i].rowPosition == character.rowPosition - 1 &&
                    Board.possibleMoveableChars[i].colPosition == character.colPosition)
                {
                    inWay = true;
                }
            }
            for (int i = 0; i < Board.spawnedEnemies.Count; i++)
            {
                if (Board.spawnedEnemies[i].rowPosition == character.rowPosition - 1 &&
                    Board.spawnedEnemies[i].colPosition == character.colPosition)
                {
                    inWay = true;
                }
            }
            //check for dead spaces in the way
            for (int i = 0; i < Board.numDeadSpaces; i++)
            {
                if (Board.deadPoints[i].y == character.rowPosition - 1 &&
                    Board.deadPoints[i].x == character.colPosition)
                {
                    inWay = true;
                }
            }
            //check for cannons in the way
            for (int i = 0; i < Board.numCannons; i++)
            {
                if (Board.allCannons[i].cannon.rowPosition == character.rowPosition - 1 && Board.allCannons[i].cannon.colPosition == character.colPosition)
                {
                    inWay = true;
                    //Board.allCannons[i].UseCannon(Board.spawnedEnemies, 5);
                }
            }

            if (Board.pirateBoss.colPosition == character.colPosition && Board.pirateBoss.rowPosition == character.rowPosition - 1)
            {
                inWay = true;
            }
            for (int i = 0; i < Board.numGenerators; i++)
            {
                if (Board.generators[i].generator.colPosition == character.colPosition  && Board.generators[i].generator.rowPosition == character.rowPosition - 1)
                {
                    inWay = true;
                    //Board.allCannons[i].UseCannon(Board.spawnedEnemies, 5);
                }
            }
            if (!inWay && ExperimentalResources.ModifyResource(cost))
            {
                character.SetRowAndCol(character.rowPosition - 1, character.colPosition);
                character.GetPiece().transform.position = GameObject.Find("gridRow" + (character.rowPosition) + "Column" + character.colPosition).transform.position;
                return true;
            }
        }
        else if (direction == 2)
        {
            bool inWay = false;
            //check for other characters in the way
            for (int i = 0; i < Board.possibleMoveableChars.Length; i++)
            {
                if (Board.possibleMoveableChars[i] == character)
                {
                    continue;
                }
                if (Board.possibleMoveableChars[i].colPosition == character.colPosition + 1 && Board.possibleMoveableChars[i].rowPosition == character.rowPosition)
                {
                    inWay = true;
                }
            }
            for (int i = 0; i < Board.spawnedEnemies.Count; i++)
            {
                if (Board.spawnedEnemies[i].colPosition == character.colPosition + 1 && Board.spawnedEnemies[i].rowPosition == character.rowPosition)
                {
                    inWay = true;
                }
            }
            //check for dead spaces in the way
            for (int i = 0; i < Board.numDeadSpaces; i++)
            {
                if (Board.deadPoints[i].x == character.colPosition + 1 && Board.deadPoints[i].y == character.rowPosition)
                {
                    inWay = true;
                }
            }
            //check for cannons in the way
            for (int i = 0; i < Board.numCannons; i++)
            {
                if (Board.allCannons[i].cannon.colPosition == character.colPosition + 1 && Board.allCannons[i].cannon.rowPosition == character.rowPosition)
                {
                    inWay = true;
                    //Board.allCannons[i].UseCannon(Board.spawnedEnemies, 5);
                }
            }
            if (Board.pirateBoss.colPosition == character.colPosition + 1 && Board.pirateBoss.rowPosition == character.rowPosition)
            {
                inWay = true;
            }
            for (int i = 0; i < Board.numGenerators; i++)
            {
                if (Board.generators[i].generator.colPosition == character.colPosition + 1 && Board.generators[i].generator.rowPosition == character.rowPosition)
                {
                    inWay = true;
                    //Board.allCannons[i].UseCannon(Board.spawnedEnemies, 5);
                }
            }
            if (!inWay && ExperimentalResources.ModifyResource(cost))
            {
                character.SetRowAndCol(character.rowPosition, character.colPosition + 1);
                character.GetPiece().transform.position = GameObject.Find("gridRow" + (character.rowPosition) + "Column" + (character.colPosition)).transform.position;
                return true;
            }
        }
        else if (direction == 3)
        {
            bool inWay = false;
            //check for other characters in the way
            for (int i = 0; i < Board.possibleMoveableChars.Length; i++)
            {
                if (Board.possibleMoveableChars[i] == character)
                {
                    continue;
                }
                if (Board.possibleMoveableChars[i].rowPosition == character.rowPosition + 1 && Board.possibleMoveableChars[i].colPosition == character.colPosition)
                {
                    inWay = true;
                }
            }
            for (int i = 0; i < Board.spawnedEnemies.Count; i++)
            {
                if (Board.spawnedEnemies[i].rowPosition == character.rowPosition + 1 && Board.spawnedEnemies[i].colPosition == character.colPosition)
                {
                    inWay = true;
                }
            }
            //check for dead spaces in the way
            for (int i = 0; i < Board.numDeadSpaces; i++)
            {
                if (Board.deadPoints[i].y == character.rowPosition + 1 && Board.deadPoints[i].x == character.colPosition)
                {
                    inWay = true;
                }
            }
            //check for cannons in the way
            for (int i = 0; i < Board.numCannons; i++)
            {
                if (Board.allCannons[i].cannon.rowPosition == character.rowPosition + 1 && Board.allCannons[i].cannon.colPosition == character.colPosition)
                {
                    inWay = true;
                    //Board.allCannons[i].UseCannon(Board.spawnedEnemies, 5);
                }
            }
            if (Board.pirateBoss.colPosition == character.colPosition && Board.pirateBoss.rowPosition == character.rowPosition + 1)
            {
                inWay = true;
            }
            for (int i = 0; i < Board.numGenerators; i++)
            {
                if (Board.generators[i].generator.colPosition == character.colPosition && Board.generators[i].generator.rowPosition == character.rowPosition + 1)
                {
                    inWay = true;
                    //Board.allCannons[i].UseCannon(Board.spawnedEnemies, 5);
                }
            }
            if (!inWay && ExperimentalResources.ModifyResource(cost))
            {
                character.SetRowAndCol(character.rowPosition + 1, character.colPosition);
                character.GetPiece().transform.position = GameObject.Find("gridRow" + (character.rowPosition) + "Column" + (character.colPosition)).transform.position;
                return true;
            }
        }
        else
        {
            bool inWay = false;
            //check for other characters in the way
            for (int i = 0; i < Board.possibleMoveableChars.Length; i++)
            {
                if (Board.possibleMoveableChars[i] == character)
                {
                    continue;
                }
                if (Board.possibleMoveableChars[i].colPosition == character.colPosition - 1 && Board.possibleMoveableChars[i].rowPosition == character.rowPosition)
                {
                    inWay = true;
                }
            }
            for (int i = 0; i < Board.spawnedEnemies.Count; i++)
            {
                if (Board.spawnedEnemies[i].colPosition == character.colPosition - 1 && Board.spawnedEnemies[i].rowPosition == character.rowPosition)
                {
                    inWay = true;
                }
            }
            //check for dead spaces in the way
            for (int i = 0; i < Board.numDeadSpaces; i++)
            {
                if (Board.deadPoints[i].x == character.colPosition - 1 && Board.deadPoints[i].y == character.rowPosition)
                {
                    inWay = true;
                }
            }
            //check for cannons in the way
            for (int i = 0; i < Board.numCannons; i++)
            {
                if (Board.allCannons[i].cannon.colPosition == character.colPosition - 1 && Board.allCannons[i].cannon.rowPosition == character.rowPosition)
                {
                    inWay = true;
                    //Board.allCannons[i].UseCannon(Board.spawnedEnemies, 5);
                }
            }
            for (int i = 0; i < Board.numGenerators; i++)
            {
                if (Board.generators[i].generator.colPosition == character.colPosition - 1 && Board.generators[i].generator.rowPosition == character.rowPosition)
                {
                    inWay = true;
                    //Board.allCannons[i].UseCannon(Board.spawnedEnemies, 5);
                }
            }
            if (Board.pirateBoss.colPosition == character.colPosition - 1 && Board.pirateBoss.rowPosition == character.rowPosition)
            {
                inWay = true;
            }
            if (!inWay && ExperimentalResources.ModifyResource(cost))
            {
                character.SetRowAndCol(character.rowPosition, character.colPosition - 1);
                character.GetPiece().transform.position = GameObject.Find("gridRow" + (character.rowPosition) + "Column" + (character.colPosition)).transform.position;
                return true;
            }
        }

        return false;
    }

    public static void SetStartDirectionLineup()
    {
        for (int j = 0; j < numDirectionLineups; j++)
        {
            //set the lineup of directions
            for (int i = 0; i < numDirectionsInLineup; i++)
            {
                //set the direction
                Direction setDirection = Direction.Up;
                switch (j)
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
                directionLineups[i, j] = setDirection;
            }
        }
    }

    public static void FillOneSpotInLineup()
    {
        //set the lineup of directions
        for (int i = 0; i < numDirectionsInLineup; i++)
        {

            if (i != numDirectionsInLineup - 1)
            {
                directionLineups[i, currentDirectionLineup] = directionLineups[i + 1, currentDirectionLineup];
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
                directionLineups[i, currentDirectionLineup] = setDirection;
            }
        }
    }

    public static void SwitchDirectionLineup(Direction wayToMove, GameObject columnHighlight)
    {
        if (wayToMove == Direction.Right)
        {
            currentDirectionLineup++;

            Vector3 rectPos = columnHighlight.GetComponent<RectTransform>().position;
            columnHighlight.GetComponent<RectTransform>().position = new Vector3(rectPos.x + 75, rectPos.y, rectPos.z);
            rectPos = columnHighlight.GetComponent<RectTransform>().position;

            if (currentDirectionLineup >= numDirectionLineups)
            {
                currentDirectionLineup = 0;
                columnHighlight.GetComponent<RectTransform>().position = new Vector3(rectPos.x - 75*numDirectionLineups, rectPos.y, rectPos.z);
            }
        }
        else if (wayToMove == Direction.Left)
        {
            currentDirectionLineup--;

            Vector3 rectPos = columnHighlight.GetComponent<RectTransform>().position;
            columnHighlight.GetComponent<RectTransform>().position = new Vector3(rectPos.x - 75, rectPos.y, rectPos.z);
            rectPos = columnHighlight.GetComponent<RectTransform>().position;

            if (currentDirectionLineup < 0)
            {
                currentDirectionLineup = numDirectionLineups - 1;
                columnHighlight.GetComponent<RectTransform>().position = new Vector3(rectPos.x + 75 * numDirectionLineups, rectPos.y, rectPos.z);
            }
        }
    }

    public static void Setup()
    {
        MovementManager.currentDirectionLineup = 0;
        MovementManager.numDirectionsInLineup = 25;
        MovementManager.numDirectionLineups = 2;
        MovementManager.directionLineups = new MovementManager.Direction[MovementManager.numDirectionsInLineup, MovementManager.numDirectionLineups];
        MovementManager.SetStartDirectionLineup();
    }
}
