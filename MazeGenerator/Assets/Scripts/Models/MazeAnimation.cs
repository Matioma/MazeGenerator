using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Events;

public class MazeAnimation : MonoBehaviour
{
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


    public UnityEvent<int> onFrameChanged;
    public UnityEvent<int> onAnimationFramesCountChange;
    public UnityEvent<int> onPlaySpeedChanged;
    public UnityEvent<AnimationFrame> onRenderNextFrame;
    public UnityEvent onResetAnimation;

    public bool IsPaused { get; private set; }
    private int _currentFrame = 0;

    private int _playSpeed = 1;
    public int playSpeed {
        get { 
            return _playSpeed;
        }
        set { 
            if(_playSpeed == value ) {
                return;
            }
            _playSpeed = value;
            onPlaySpeedChanged.Invoke(value);

            if (value == 0)
            {
                Pause();
            }
            else {
                Play();
            }
        } 
    }

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


    Coroutine animationRoutine = null;
    public void StartAnimation()
    {
        animationRoutine = StartCoroutine(PlayAnimation());
    }

    public void Play()
    {
        IsPaused = false;
    }

    public void Pause()
    {
        IsPaused = true;
    }

    public void ReplayAnimation()
    {
        CurrentFrame = 0;
        onResetAnimation?.Invoke();
    }

    private IEnumerator PlayAnimation()
    {
        AnimationFrame nextFrame = null;
        do
        {
            if (!IsPaused)
            {
                nextFrame = GetNext();
            }

            onRenderNextFrame.Invoke(nextFrame);
            if (CurrentFrame % 100 == 0)
            {
                yield return new WaitForSeconds(0.1f);
            }

        } while (nextFrame != null);
    }

    public void ResetAnimation()
    {
        StopCoroutine(animationRoutine);
        onResetAnimation.Invoke();
        CurrentFrame = 0;
        StartCoroutine(PlayAnimation());
    }

    public void LoadAnimation(List<AnimationFrame> animationFrames)
    {
        CurrentFrame = 0;
        Frames = animationFrames;
    }

    public AnimationFrame GetNext()
    {
        if (isPlayForward() && IsNotLastFrame())
        {
            var nextFrame = Frames[CurrentFrame];
            CurrentFrame++;
            return nextFrame;
        }
        if (!isPlayForward() && IsNotFirstFrame())
        {
            var nextFrame = Frames[CurrentFrame];
            CurrentFrame--;
            return nextFrame;
        }
        return null;
    }

    private bool isPlayForward()
    {
        return playSpeed > 0;
    }

    private bool IsNotLastFrame()
    {
        return CurrentFrame < Frames.Count;
    }

    private bool IsNotFirstFrame()
    {
        return CurrentFrame > 0;
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
        onRenderNextFrame.RemoveAllListeners();
        onResetAnimation.RemoveAllListeners();
        onPlaySpeedChanged.RemoveAllListeners();
    }
}
