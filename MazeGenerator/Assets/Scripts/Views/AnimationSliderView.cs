using Assets.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnimationSliderView : MazeViewBase
{
    [Header("Slider")]
    [SerializeField]
    private TMP_InputField _currentFrameTextMesh;
    [SerializeField]
    private Slider _animationSlider;

    private int _frameNumber = 0;
    private int _maxFrameNumber = 0;

    public override void Awake()
    {
        base.Awake();

        _mazeAnimation.onFrameChanged.AddListener(handleCurrentFrameDisplay);
        _mazeAnimation.onAnimationFramesCountChange.AddListener(handleAnimationFrameCountChange);

        _animationSlider.onValueChanged.AddListener(handleAnimationFrameChange);
    }

    private void handleAnimationFrameChange(float value) {
        //_mazeAnimation.PlayFrame((int)value);
    }

    private void handleCurrentFrameDisplay(int frame)
    {
        _frameNumber = frame;
        UpdateViews();
    }
    private void handleAnimationFrameCountChange(int frame)
    {
        _maxFrameNumber = frame;
        _animationSlider.minValue = 0;
        _animationSlider.maxValue = _maxFrameNumber;
        UpdateViews();
    }

    public void UpdateViews()
    {
        _currentFrameTextMesh.text = _frameNumber.ToString();
        //_animationSlider.value = _frameNumber;
    }
}
