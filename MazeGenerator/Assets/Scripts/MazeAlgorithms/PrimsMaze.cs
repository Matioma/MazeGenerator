using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
    internal class PrimsMaze : IAnimationGeneration
    {
        private List<Vector2Int> _potentialWalls = new List<Vector2Int>();

        public List<AnimationFrame> GetMazeAnimation(MazeSettings mazeSettings)
        {
            return GenerateMaze(mazeSettings);
        }

        private List<AnimationFrame> GenerateMaze(MazeSettings mazeSettings)
        {
            var mazeData = new bool[mazeSettings.Width, mazeSettings.Depth];
            List<AnimationFrame> animationFrames = new List<AnimationFrame>();
            List<Vector2Int> definitelyWalls = new List<Vector2Int>();

            //Pick start Cell
            int x = Random.Range(1, mazeData.GetLength(0) - 1);
            int z = Random.Range(1, mazeData.GetLength(1) - 1);
            Vector2Int startCell = new Vector2Int(x, z);

            // Set the First Cell as Path
            mazeData[startCell.x, startCell.y] = true;
            _potentialWalls.AddRange(GetWalls(mazeData, startCell));
            animationFrames.Add(new AnimationFrame(startCell, _potentialWalls.ToArray()));


            while (AnyWallIsPotentialPath())
            {
                var randomWallIndex = Random.Range(0, _potentialWalls.Count);
                var potentialPath = _potentialWalls[randomWallIndex];

                var wallsAroundPotentialPath = GetWalls(mazeData, potentialPath);
                if (WallsCompletlySurroundPotentialPath(wallsAroundPotentialPath))
                {
                    mazeData[potentialPath.x, potentialPath.y] = true; 
                    _potentialWalls.Remove(potentialPath);

                    var deltaWalls = wallsAroundPotentialPath.Where(x => !_potentialWalls.Contains(x)).ToArray();
                    var deltaWallsWithoutDefinetlyWalls = deltaWalls.Where(x => !definitelyWalls.Contains(x)).ToArray();

                    _potentialWalls.AddRange(GetWalls(mazeData, potentialPath));
                    animationFrames.Add(new AnimationFrame(new Vector2Int(potentialPath.x, potentialPath.y), deltaWallsWithoutDefinetlyWalls));
                }
                else
                {
                    _potentialWalls.Remove(potentialPath);
                    definitelyWalls.Add(potentialPath);
                }
            }

            if (!mazeSettings.Animate)
            {
                animationFrames.Clear();
                definitelyWalls.Clear();
            }

            // Get Last Frame
            List<Vector2Int> lastWalls = new List<Vector2Int>();
            for (var xIndex = 0; xIndex < mazeData.GetLength(0); xIndex++)
            {
                for (var yIndex = 0; yIndex < mazeData.GetLength(1); yIndex++)
                {
                    if (!mazeData[xIndex, yIndex])
                    {
                        lastWalls.Add(new Vector2Int(xIndex, yIndex));
                    }
                }
            }
            var deltaLastWalls = lastWalls.Where(x => !definitelyWalls.Contains(x)).ToArray();
            animationFrames.Add(new AnimationFrame(startCell, deltaLastWalls));

            return animationFrames;
        }

        private static bool WallsCompletlySurroundPotentialPath(List<Vector2Int> wallsAroundPotentialPath)
        {
            return wallsAroundPotentialPath.Count >= 3;
        }

        private bool AnyWallIsPotentialPath()
        {
            return _potentialWalls.Count > 0;
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
