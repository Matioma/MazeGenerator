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

        _mazeAnimation.onFrameChanged.AddListener(HandleCurrentFrameDisplay);
        _mazeAnimation.onAnimationFramesCountChange.AddListener(HandleAnimationFrameCountChange);
    }
    private void HandleCurrentFrameDisplay(int frame)
    {
        _frameNumber = frame;
        UpdateViews();
    }
    private void HandleAnimationFrameCountChange(int frame)
    {
        _maxFrameNumber = frame;
        _animationSlider.minValue = 1;
        _animationSlider.maxValue = _maxFrameNumber;
        UpdateViews();
    }

    public void UpdateViews()
    {
        _currentFrameTextMesh.text = _frameNumber.ToString();
        _animationSlider.value = _frameNumber;
    }
}
