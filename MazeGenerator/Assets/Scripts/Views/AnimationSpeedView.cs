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

    [SerializeField]
    private int speedStep = 10;

    private int frameSpeed;

    public override void Awake()
    {
        base.Awake();

        HandleSpeedChange(_mazeAnimation.PlaySpeed);
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
            speedText.text = $"{speed}";
        }
        else if (frameSpeed == 0) {
            speedText.text = $"PAUSED";
        }
        else
        {
            speedText.text = $"{speed}";
        }
    }

    private void IncreaseSpeed() {
        _mazeAnimation.PlaySpeed += speedStep;
    }

    private void DecreaseSpeed() {
        _mazeAnimation.PlaySpeed -= speedStep;
    }
}
