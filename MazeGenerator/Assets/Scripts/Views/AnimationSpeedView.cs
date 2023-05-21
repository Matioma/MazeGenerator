using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnimationSpeedView : MazeViewBase
{
    [SerializeField]
    private Button _fasterButton;

    [SerializeField]
    private Button _slowerButton;

    [SerializeField]
    private TextMeshProUGUI speedText;

    private int frameSpeed;

    private void Awake()
    {
        base.Awake();

        HandleSpeedChange(_mazeAnimation.playSpeed);
        _mazeAnimation.onPlaySpeedChanged.AddListener(HandleSpeedChange);
        _fasterButton.onClick.AddListener(IncreaseSpeed);
        _slowerButton.onClick.AddListener(DecreaseSpeed);
    }

    private void HandleSpeedChange(int speed) {
        frameSpeed = speed;
        DisplaySpeed(speed);
    }

    private void DisplaySpeed(int speed) {
        if (frameSpeed > 0)
        {
            speedText.text = $"X{speed}";
        }
        else if (frameSpeed == 0) {
            speedText.text = $"PAUSED";
        }
        else
        {
            speedText.text = $"X ({speed})";
        }
    }

    private void IncreaseSpeed() {
        _mazeAnimation.playSpeed += 1;
    }

    private void DecreaseSpeed() {
        _mazeAnimation.playSpeed -= 1;
    }
}
