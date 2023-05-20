using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAnimationedGeneration
{
    public List<AnimationFrame> GetMazeAnimation(MazeSettings mazeSettings);
}
