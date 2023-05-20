using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAnimationedGeneration
{
    public List<Vector2Int> GenerateMazeStepsDelta(int width, int depth);
    public List<KeyValuePair<Vector2Int?, Vector2Int[]>> GenerateMazeStepsWalls(int width, int depth);

    public List<AnimationFrame> GetMazeAnimation(MazeSettings mazeSettings);
}
