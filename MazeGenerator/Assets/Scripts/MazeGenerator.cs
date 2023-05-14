using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MazeGenerator : MonoBehaviour, IGenerator
{
    [SerializeField]
    private GameObject puzzleUnit;

    private const int width = 20;
    private const int depth = 20;
    private const int unitScale = 3;

    //private bool[,] mazeData = new bool[10, 10];

    private List<Vector2Int> walls = new List<Vector2Int>();
    public void Generate()
    {
        BuildMaze(new bool[width, depth]);
    }


    private void BuildMaze(bool[,] mazedata) { 
        var newMaze = GenerateMaze(mazedata);

       for(var x = 0; x < newMaze.GetLength(0); x++)
        {
            for (var z = 0; z < newMaze.GetLength(1); z++) {

                if (newMaze[x,z])
                {
                    continue;
                }

                Vector3 position = new Vector3(x * unitScale, 0, z * unitScale);
                Vector3 scale = new Vector3(unitScale, unitScale, unitScale);
                GameObject createdPuzzleUnit = Instantiate(puzzleUnit, position, Quaternion.identity);
                createdPuzzleUnit.transform.localScale = scale;
            }
        }
    }


    private  bool[,] GenerateMaze(bool[,] maze) {

        //Pick start Cell
        int x = Random.Range(1, maze.GetLength(0)-1); 
        int z = Random.Range(1, maze.GetLength(1)-1);
        Vector2Int startCell = new Vector2Int(x, z);

        walls.AddRange(GetWalls(maze, startCell));

        while (walls.Count > 0) {
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


    private List<Vector2Int> GetWalls(bool[,] maze, Vector2Int cellToCheck) {
        if (cellToCheck.x <= 0 || cellToCheck.x >= maze.GetLength(0) - 1) { 
            return new List<Vector2Int>();
        }
        if (cellToCheck.y <= 0 || cellToCheck.y >= maze.GetLength(1) - 1)
        {
            return new List<Vector2Int>();
        }


        var rightWallAddress = new Vector2Int(cellToCheck.x + 1, cellToCheck.y);
        var leftWallAddress = new Vector2Int(cellToCheck.x - 1, cellToCheck.y);

        var topWallAddress = new Vector2Int(cellToCheck.x, cellToCheck.y+1);
        var downWallAddress = new Vector2Int(cellToCheck.x , cellToCheck.y-1);
            
        var topRightWallAddress = new Vector2Int(cellToCheck.x +1, cellToCheck.y + 1);
        var downRightWallAddress = new Vector2Int(cellToCheck.x +1, cellToCheck.y - 1);

        var topLefttWallAddress = new Vector2Int(cellToCheck.x - 1, cellToCheck.y + 1);
        var downLeftWallAddress = new Vector2Int(cellToCheck.x - 1, cellToCheck.y - 1);


       
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
        //if (maze[topRightWallAddress.x, topRightWallAddress.y] == false)
        //{
        //    newWalls.Add(topRightWallAddress);
        //}
        //if (maze[downRightWallAddress.x, downRightWallAddress.y] == false)
        //{
        //    newWalls.Add(downRightWallAddress);
        //}
        //if (maze[topLefttWallAddress.x, topLefttWallAddress.y] == false)
        //{
        //    newWalls.Add(topLefttWallAddress);
        //}
        //if (maze[downLeftWallAddress.x, downLeftWallAddress.y] == false)
        //{
        //    newWalls.Add(downLeftWallAddress);
        //}
        return newWalls;
    }

}
