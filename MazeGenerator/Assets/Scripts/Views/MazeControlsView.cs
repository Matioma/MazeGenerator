using Assets.Scripts;
using System;
using UnityEngine;
using UnityEngine.UI;

public class MazeControlsView : MonoBehaviour
{
    [SerializeField]
    private MazeSettings _mazeSettings;

    [SerializeField]
    private Slider _widthSlider;
    [SerializeField]
    private Slider _depthSlider;

    public void Awake()
    {
        if (_mazeSettings == null) {
            _mazeSettings = FindObjectOfType<MazeSettings>();
        }

        InitializeWidthControls(_mazeSettings.Width);
        InitializeDepthControls(_mazeSettings.Depth);

        _mazeSettings.onWidthChange.AddListener(InitializeWidthControls);
        _mazeSettings.onDepthChange.AddListener(InitializeDepthControls);

        _widthSlider.onValueChanged.AddListener(UpdateWidth);
        _depthSlider.onValueChanged.AddListener(UpdateDepth);
    }

    private void InitializeWidthControls(int width) { 
        _widthSlider.value = width;
        _widthSlider.minValue = MazeSettings.MIN_DIMENSION;
        _widthSlider.maxValue = MazeSettings.MAX_DIMENSION;
    }
    private void InitializeDepthControls(int depth)
    {
        _depthSlider.value = depth;
        _depthSlider.minValue = MazeSettings.MIN_DIMENSION;
        _depthSlider.maxValue = MazeSettings.MAX_DIMENSION;
    }

    private void UpdateWidth(float newValue)
    {
        _mazeSettings.Width = (int)newValue;
    }

    private void UpdateDepth(float newValue)
    {
        _mazeSettings.Depth = (int)newValue;
    }

    public void OnDestroy()
    {
        _widthSlider.onValueChanged.RemoveAllListeners();
    }
}
