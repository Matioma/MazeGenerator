using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
    internal class PrimsMaze: IGenerator, IGeneratorAnimation
    {
        private List<Vector2Int> _walls = new List<Vector2Int>();
        private List<Vector2Int> _animationStepsDelta = new List<Vector2Int>();
        private Dictionary<Vector2Int, Vector2Int[]> _animzationStepWithWalls = new Dictionary<Vector2Int, Vector2Int[]>();

        public bool[,] GenerateMaze(int width, int depth)
        {
            return GenerateMaze(width, depth, false);
        }

        public List<Vector2Int> GenerateMazeStepsDelta(int width, int depth)
        {
            GenerateMaze(width, depth, true);
            return _animationStepsDelta;
        }

        public Dictionary<Vector2Int, Vector2Int[]> GenerateMazeStepsWalls(int width, int depth)
        {
            GenerateMaze(width, depth, true);
            return _animzationStepWithWalls;
        }


        private bool[,] GenerateMaze(int width, int depth, bool storeSteps)
        {
            var maze = new bool[width, depth];

            //Pick start Cell
            int x = Random.Range(1, maze.GetLength(0) - 1);
            int z = Random.Range(1, maze.GetLength(1) - 1);
            Vector2Int startCell = new Vector2Int(x, z);


            maze[startCell.x, startCell.y] = true;
            _animationStepsDelta.Add(new Vector2Int(startCell.x, startCell.y));

            _walls.AddRange(GetWalls(maze, startCell));
            _animzationStepWithWalls.Add(startCell, _walls.ToArray());

            while (_walls.Count > 0)
            {
                var randomWallIndex = Random.Range(0, _walls.Count);
                var wall = _walls[randomWallIndex];

                var wallsOfNewlySelectedCell = GetWalls(maze, wall);
                if (wallsOfNewlySelectedCell.Count >= 3)
                {
                    maze[wall.x, wall.y] = true;


                    _walls.AddRange(GetWalls(maze, wall));
                    _walls.Remove(wall);

                    if (storeSteps)
                    {
                        _animationStepsDelta.Add(new Vector2Int(wall.x, wall.y));
                        _animzationStepWithWalls.Add(new Vector2Int(wall.x, wall.y), _walls.ToArray());
                    }
                }
                else
                {
                    _walls.Remove(wall);
                }
            }
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
