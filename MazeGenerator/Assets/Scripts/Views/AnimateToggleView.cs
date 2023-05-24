using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class AnimateToggleView : MonoBehaviour
{
    [SerializeField]
    private MazeSettings _mazeSettings;

    [SerializeField]
    private Toggle _toggle;

    void Awake()
    {
        if (_mazeSettings == null)
        {
            _mazeSettings = FindObjectOfType<MazeSettings>();
        }
        HandleAnimateStateChange(_mazeSettings.Animate);

        _mazeSettings.onAnimateChanged.AddListener(HandleAnimateStateChange);
        _toggle.onValueChanged.AddListener(HandleViewToggleChange);
    }

    private void HandleAnimateStateChange(bool value) { 
        _toggle.isOn = value;
    }

    private void HandleViewToggleChange(bool value)
    {
        _mazeSettings.Animate = value;
    }
}
