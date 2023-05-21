using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
    internal class PrimsMaze : IGenerator, IAnimationGeneration
    {
        private List<Vector2Int> _walls = new List<Vector2Int>();

        public List<AnimationFrame> GetMazeAnimation(MazeSettings mazeSettings)
        {
            return GenerateMaze(mazeSettings);
        }

        private List<AnimationFrame> GenerateMaze(MazeSettings mazeSettings)
        {
            var maze = new bool[mazeSettings.Width, mazeSettings.Depth];
            List<AnimationFrame> animationFrames = new List<AnimationFrame>();
            List<Vector2Int> definitelyWalls= new List<Vector2Int>();


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

                    _walls.Remove(wall);

                    var newWalls = wallsOfNewlySelectedCell.Where(x => !_walls.Contains(x)).ToArray();
                    var newWallsWithoutDefinetlyWalls = newWalls.Where(x => !definitelyWalls.Contains(x)).ToArray();


                    _walls.AddRange(GetWalls(maze, wall));
                    animationFrames.Add(new AnimationFrame(new Vector2Int(wall.x, wall.y), newWallsWithoutDefinetlyWalls));
                }
                else
                {
                    _walls.Remove(wall);
                    definitelyWalls.Add(wall);
                }
            }

            if (mazeSettings.Animate == false)
            {
                animationFrames.Clear();
                definitelyWalls.Clear();
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

            var deltaLastWalls = lastWalls.Where(x => !definitelyWalls.Contains(x)).ToArray();

            animationFrames.Add(new AnimationFrame(startCell, deltaLastWalls));
            return animationFrames;
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
