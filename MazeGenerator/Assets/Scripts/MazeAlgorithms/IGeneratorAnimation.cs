using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGeneratorAnimation
{
    public List<Vector2Int> GenerateMazeStepsDelta(int width, int depth);
}
