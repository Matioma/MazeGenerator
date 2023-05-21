using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
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

    [HideInInspector]
    public UnityEvent<int> onFrameChanged;
    [HideInInspector]
    public UnityEvent<int> onAnimationFramesCountChange;
    [HideInInspector]
    public UnityEvent<int> onPlaySpeedChanged;
    [HideInInspector]
    public UnityEvent<AnimationFrame> onRenderNextFrame;
    [HideInInspector]
    public UnityEvent onAminationStart;
    [HideInInspector]
    public UnityEvent onAnimationFinished;

    public bool IsPaused { get; private set; }
    private int _currentFrame = 0;

    private int _playSpeed = 100;
    private int _oldSpeed = 100;
    public int PlaySpeed
    {
        get
        {
            return _playSpeed;
        }
        set
        {
            if (_playSpeed == value)
            {
                return;
            }
            _playSpeed = value;
            onPlaySpeedChanged.Invoke(value);
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
        if (animationRoutine != null)
        {
            StopCoroutine(animationRoutine);
        }
        onAminationStart?.Invoke();
        CurrentFrame = 0;
        animationRoutine = StartCoroutine(PlayAnimation());
    }

    public void Play()
    {
        PlaySpeed = _oldSpeed;
    }

    public void Pause()
    {
        _oldSpeed = PlaySpeed;
        PlaySpeed = 0;
    }

    private IEnumerator PlayAnimation()
    {
        AnimationFrame nextFrame = null;
        do
        {
            if (PlaySpeed!=0)
            {
                nextFrame = GetNext();
            }

            if (nextFrame != null)
            {
                nextFrame.isReverse = _playSpeed < 0;
            }

            onRenderNextFrame.Invoke(nextFrame);

            if (PlaySpeed == 0)
            {
                yield return new WaitForSeconds(0.1f);
            }
            else if (CurrentFrame % Mathf.Abs(_playSpeed) == 0)
            {
                yield return new WaitForSeconds(0.1f);
            }

        } while (nextFrame != null);

        onAnimationFinished.Invoke();
    }

    public void ResetAnimation()
    {
        StopCoroutine(animationRoutine);
        onAminationStart?.Invoke();
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
            CurrentFrame++;
            return Frames[CurrentFrame];
        }
        if (!isPlayForward() && IsNotFirstFrame())
        {
            CurrentFrame--;
            return Frames[CurrentFrame];
        }
        return null;
    }

    private bool isPlayForward()
    {
        return PlaySpeed > 0;
    }

    private bool IsNotLastFrame()
    {
        return CurrentFrame < Frames.Count;
    }

    private bool IsNotFirstFrame()
    {
        return CurrentFrame > 0;
    }

    public void OnDestroy()
    {
        onFrameChanged.RemoveAllListeners();
        onAnimationFramesCountChange.RemoveAllListeners();
        onRenderNextFrame.RemoveAllListeners();
        onAminationStart.RemoveAllListeners();
        onPlaySpeedChanged.RemoveAllListeners();
    }
}
