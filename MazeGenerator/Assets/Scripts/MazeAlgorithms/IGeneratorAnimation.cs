using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGeneratorAnimation
{
    public List<bool[,]> GenerateMazeSteps(int width, int depth);
}
