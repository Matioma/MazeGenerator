using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
    internal class PrimsMaze: IGenerator
    {
        private List<Vector2Int> walls = new List<Vector2Int>();
        public bool[,] GenerateMaze(int width, int depth)
        {
            var maze = new bool[width, depth];

            //Pick start Cell
            int x = Random.Range(1, maze.GetLength(0) - 1);
            int z = Random.Range(1, maze.GetLength(1) - 1);
            Vector2Int startCell = new Vector2Int(x, z);

            walls.AddRange(GetWalls(maze, startCell));

            while (walls.Count > 0)
            {
                var randomWallIndex = Random.Range(0, walls.Count);
                var wall = walls[randomWallIndex];

                var wallsOfNewlySelectedCell = GetWalls(maze, wall);
                if (wallsOfNewlySelectedCell.Count >= 3)
                {
                    maze[wall.x, wall.y] = true;
                    walls.AddRange(GetWalls(maze, wall));
                    walls.Remove(wall);
                }
                else
                {
                    walls.Remove(wall);
                }
            }

            maze[startCell.x, startCell.y] = true;
            return maze;
        }

        private List<Vector2Int> GetWalls(bool[,] maze, Vector2Int cellToCheck)
        {
            if (cellToCheck.x <= 0 || cellToCheck.x >= maze.GetLength(0) - 1)
            {
                return new List<Vector2Int>();
            }
            if (cellToCheck.y <= 0 || cellToCheck.y >= maze.GetLength(1) - 1)
            {
                return new List<Vector2Int>();
            }

            var rightWallAddress = new Vector2Int(cellToCheck.x + 1, cellToCheck.y);
            var leftWallAddress = new Vector2Int(cellToCheck.x - 1, cellToCheck.y);

            var topWallAddress = new Vector2Int(cellToCheck.x, cellToCheck.y + 1);
            var downWallAddress = new Vector2Int(cellToCheck.x, cellToCheck.y - 1);

            var newWalls = new List<Vector2Int>();

            if (maze[rightWallAddress.x, rightWallAddress.y] == false)
            {
                newWalls.Add(rightWallAddress);
            }
            if (maze[leftWallAddress.x, leftWallAddress.y] == false)
            {
                newWalls.Add(leftWallAddress);
            }
            if (maze[topWallAddress.x, topWallAddress.y] == false)
            {
                newWalls.Add(topWallAddress);
            }
            if (maze[downWallAddress.x, downWallAddress.y] == false)
            {
                newWalls.Add(downWallAddress);
            }
            return newWalls;
        }

    }
}
