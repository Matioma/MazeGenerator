using Assets.Scripts;
using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MazeControlsView : MonoBehaviour
{
    [SerializeField]
    private MazeSettings _mazeSettings;

    [SerializeField]
    private Slider _widthSlider;
    [SerializeField]
    private TMP_InputField _widthInput;


    [SerializeField]
    private Slider _depthSlider;
    [SerializeField]
    private TMP_InputField _depthInput;

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
        _widthInput.onValueChanged.AddListener(HandleWidthInput);


        _depthSlider.onValueChanged.AddListener(UpdateDepth);
        _depthInput.onValueChanged.AddListener(HandleDepthInput);
    }


    private void HandleWidthInput(string input) {
        int inputValue;
        bool succesfullyParsed = int.TryParse(input, out inputValue);
        if(succesfullyParsed)
        {
            _mazeSettings.Width = inputValue;
        }
        else
        {
            _widthInput.text = _mazeSettings.Width.ToString();
        }
    }

    private void HandleDepthInput(string input)
    {
        int inputValue;
        bool succesfullyParsed = int.TryParse(input, out inputValue);
        if (succesfullyParsed)
        {
            _mazeSettings.Depth = inputValue;
        }
        else
        {
            _depthInput.text = _mazeSettings.Depth.ToString();
        }
    }

    private void InitializeWidthControls(int width) { 
        _widthSlider.value = width;
        _widthInput.text = width.ToString();
        _widthSlider.minValue = MazeSettings.MIN_DIMENSION;
        _widthSlider.maxValue = MazeSettings.MAX_DIMENSION;
    }
    private void InitializeDepthControls(int depth)
    {
        _depthSlider.value = depth;
        _depthInput.text = depth.ToString();
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
