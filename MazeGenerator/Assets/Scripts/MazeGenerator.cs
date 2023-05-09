using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Unity.VisualScripting;
using UnityEngine;

public class MazeGenerator : MonoBehaviour, IGenerator
{
    [SerializeField]
    private GameObject puzzleUnit;

    private const int width = 10;
    private const int depth = 10;
    private const int unitScale = 3;

    private bool[,] mazeData = new bool[10, 10];
    public void Generate()
    {
        BuildMaze(mazeData);
        //Debug.Log("Generate");
        //throw new System.NotImplementedException();
    }

    private void BuildMaze(bool[,] mazedata) { 
       for(var x = 0; x < mazeData.GetLength(0); x++)
        {
            for (var z = 0; z < mazeData.GetLength(1); z++) {
                Vector3 position = new Vector3(x*unitScale, 0, z*unitScale);
                Vector3 scale = new Vector3(unitScale, unitScale, unitScale);
                GameObject createdPuzzleUnit = Instantiate(puzzleUnit, position, Quaternion.identity);
                createdPuzzleUnit.transform.localScale = scale;
            }
        }
    }
}
