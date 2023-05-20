using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeAnimation:MonoBehaviour
{
    public int currentFrame = 0;
    private List<AnimationFrame> frames = new List<AnimationFrame>();

    //public MazeAnimation(List<AnimationFrame> animationFrames)
    //{
    //    frames = animationFrames;
    //}

    public void LoadAnimation(List<AnimationFrame> animationFrames)
    {
        currentFrame = 0;
        frames = animationFrames;
    }


    public AnimationFrame GetNext()
    {
        if (currentFrame < frames.Count)
        {
            var nextFrame = frames[currentFrame];
            currentFrame++;
            return nextFrame;
        }
        return null;
    }
    public AnimationFrame GetPrevious()
    {
        if (currentFrame >= 0)
        {
            var nextFrame = frames[currentFrame];
            currentFrame--;
            return nextFrame;
        }
        return null;
    }
}
