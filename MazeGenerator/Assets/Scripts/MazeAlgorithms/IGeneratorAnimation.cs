using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGeneratorAnimation
{
    public List<Vector2Int> GenerateMazeStepsDelta(int width, int depth);
    public Dictionary<Vector2Int, Vector2Int[]> GenerateMazeStepsWalls(int width, int depth);
}
