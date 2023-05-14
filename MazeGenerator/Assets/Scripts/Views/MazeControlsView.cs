using Assets.Scripts;
using System;
using UnityEngine;
using UnityEngine.UI;

public class MazeControlsView : MonoBehaviour
{
    [SerializeField]
    private MazeSettings mazeSettings;


    [SerializeField]
    private Slider widthSlider;
    [SerializeField]
    private Slider depthSlider;

    public void Awake()
    {
        if (mazeSettings == null) {
            mazeSettings = FindObjectOfType<MazeSettings>();
        }

        widthSlider.onValueChanged.AddListener(UpdateWidth);
        depthSlider.onValueChanged.AddListener(UpdateDepth);
    }

    private void UpdateWidth(float newValue)
    {
        mazeSettings.Width = (int)newValue;
    }

    private void UpdateDepth(float newValue)
    {
        mazeSettings.Depth = (int)newValue;
    }

    public void OnDestroy()
    {
        widthSlider.onValueChanged.RemoveAllListeners();
    }
}
