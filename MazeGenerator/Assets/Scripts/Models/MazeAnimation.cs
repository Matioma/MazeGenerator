using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MazeAnimation : MonoBehaviour
{
    public UnityEvent<int> onFrameChanged;
    public UnityEvent<int> onAnimationFramesCountChange;

    private int _currentFrame = 0;
    public int CurrentFrame
    {
        get { return _currentFrame; }
        set
        {
            if (_currentFrame == value)
            {
                return;
            }

            if (value < 0)
            {
                Debug.LogError("Can not Assign Negative TimeFrame");
                return;
            }

            _currentFrame = value;
            onFrameChanged?.Invoke(_currentFrame);
        }
    }

    private List<AnimationFrame> _frames = new List<AnimationFrame>();
    private List<AnimationFrame> Frames
    {
        get { return _frames; }
        set
        {
            _frames = value;
            onAnimationFramesCountChange?.Invoke(value.Count);
        }
    }

    public void ResetAnimationState()
    {
        CurrentFrame = 0;
    }

    public void LoadAnimation(List<AnimationFrame> animationFrames)
    {
        CurrentFrame = 0;
        Frames = animationFrames;
    }

    public AnimationFrame GetNext()
    {
        if (CurrentFrame < Frames.Count)
        {
            var nextFrame = Frames[CurrentFrame];
            CurrentFrame++;
            return nextFrame;
        }
        return null;
    }
    public AnimationFrame GetPrevious()
    {
        if (CurrentFrame >= 0)
        {
            var nextFrame = Frames[CurrentFrame];
            CurrentFrame--;
            return nextFrame;
        }
        return null;
    }

    public void OnDestroy()
    {
        onFrameChanged.RemoveAllListeners();
        onAnimationFramesCountChange.RemoveAllListeners();
    }
}
