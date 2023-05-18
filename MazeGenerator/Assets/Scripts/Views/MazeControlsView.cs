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

        DisplayMazeWidth(_mazeSettings.Width);
        DisplayMazeDepth(_mazeSettings.Depth);

        _mazeSettings.onWidthChange.AddListener(DisplayMazeWidth);
        _mazeSettings.onDepthChange.AddListener(DisplayMazeDepth);

        _widthSlider.onValueChanged.AddListener(UpdateWidth);
        _depthSlider.onValueChanged.AddListener(UpdateDepth);
    }

    private void DisplayMazeWidth(int width) { 
        _widthSlider.value = width;
    }
    private void DisplayMazeDepth(int depth)
    {
        _depthSlider.value = depth;
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
