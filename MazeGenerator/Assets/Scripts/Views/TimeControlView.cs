using Assets.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeControlView : MonoBehaviour
{
    [Header("Models")]
    [SerializeField]
    private MazeSettings _mazeSettings;

    [SerializeField]
    private MazeAnimation _mazeAnimation;

    //[Header("Slider")]
    //[SerializeField]
    //private TMP_InputField _currentFrameTextMesh;

    //[SerializeField]
    //private Slider _animationSlider;

    //private int _frameNumber = 0;
    //private int _maxFrameNumber = 0;

  
    void Awake()
    {
        if (_mazeSettings == null)
        {
            _mazeSettings = FindObjectOfType<MazeSettings>();
        }

        if (_mazeAnimation == null)
        {
            _mazeAnimation = FindObjectOfType<MazeAnimation>();
        }

        //_mazeAnimation.onFrameChanged.AddListener(handleCurrentFrameDisplay);
        //_mazeAnimation.onAnimationFramesCountChange.AddListener(handleAnimationFrameCountChange);
    }

    //private void handleCurrentFrameDisplay(int frame) {
    //    _frameNumber = frame;
    //    UpdateViews();
    //}
    //private void handleAnimationFrameCountChange(int frame)
    //{
    //    _maxFrameNumber = frame;
    //    _animationSlider.minValue = 0;
    //    _animationSlider.maxValue = _maxFrameNumber;
    //    UpdateViews();
    //}

    //public void UpdateViews() {
    //    _currentFrameTextMesh.text = _frameNumber.ToString();
    //    _animationSlider.value = _frameNumber;
    //}
}
