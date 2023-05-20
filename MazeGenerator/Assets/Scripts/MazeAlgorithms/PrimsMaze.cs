using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
    internal class PrimsMaze: IGenerator, IAnimationedGeneration
    {
        private List<Vector2Int> _walls = new List<Vector2Int>();
        private List<Vector2Int> _animationStepsDelta = new List<Vector2Int>();
        private List<KeyValuePair<Vector2Int?, Vector2Int[]>> _animzationStepWithWalls = new List<KeyValuePair<Vector2Int?, Vector2Int[]>>();

        public bool[,] GenerateMaze(int width, int depth)
        {
            return GenerateMaze(width, depth, false);
        }

        public List<Vector2Int> GenerateMazeStepsDelta(int width, int depth)
        {
            GenerateMaze(width, depth, true);
            return _animationStepsDelta;
        }

        public List<KeyValuePair<Vector2Int?, Vector2Int[]>> GenerateMazeStepsWalls(int width, int depth)
        {
            GenerateMaze(width, depth, true);
            return _animzationStepWithWalls;
        }

        public List<AnimationFrame> GetMazeAnimation(MazeSettings mazeSettings)
        {
            return GenerateMaze(mazeSettings);
        }

        private List<AnimationFrame> GenerateMaze(MazeSettings mazeSettings)
        {
            var maze = new bool[mazeSettings.Width, mazeSettings.Depth];
            List<AnimationFrame>  animationFrames = new List<AnimationFrame>();


            //Pick start Cell
            int x = Random.Range(1, maze.GetLength(0) - 1);
            int z = Random.Range(1, maze.GetLength(1) - 1);
            Vector2Int startCell = new Vector2Int(x, z);


            maze[startCell.x, startCell.y] = true;

            _walls.AddRange(GetWalls(maze, startCell));

            animationFrames.Add(new AnimationFrame(startCell, _walls.ToArray()));


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

                    animationFrames.Add(new AnimationFrame(new Vector2Int(wall.x, wall.y), _walls.ToArray()));
                }
                else
                {
                    _walls.Remove(wall);
                }
            }

            List<Vector2Int> lastWalls = new List<Vector2Int>();
            for (var xIndex = 0; xIndex < maze.GetLength(0); xIndex++)
            {
                for (var yIndex = 0; yIndex < maze.GetLength(1); yIndex++)
                {
                    if (!maze[xIndex, yIndex])
                    {
                        lastWalls.Add(new Vector2Int(xIndex, yIndex));
                    }
                }
            }
            animationFrames.Add(new AnimationFrame(startCell, lastWalls.ToArray()));

            return animationFrames;
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
            _animzationStepWithWalls.Add(new KeyValuePair<Vector2Int?, Vector2Int[]>(startCell, _walls.ToArray()));

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
                        _animzationStepWithWalls.Add(new KeyValuePair<Vector2Int?, Vector2Int[]>(new Vector2Int(wall.x, wall.y), _walls.ToArray()));
                    }
                }
                else
                {
                    _walls.Remove(wall);
                }
            }

            if (storeSteps)
            {
                List<Vector2Int> lastWalls = new List<Vector2Int>();
                for (var xIndex = 0; xIndex < maze.GetLength(0); xIndex++)
                {
                    for (var yIndex = 0; yIndex < maze.GetLength(1); yIndex++)
                    {
                        if (!maze[xIndex, yIndex])
                        {
                            lastWalls.Add(new Vector2Int(xIndex, yIndex));
                        }
                    }
                }

                _animzationStepWithWalls.Add(new KeyValuePair<Vector2Int?, Vector2Int[]>(startCell, lastWalls.ToArray()));
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
