using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAnimationGeneration
{
    public List<AnimationFrame> GetMazeAnimation(MazeSettings mazeSettings);
}
