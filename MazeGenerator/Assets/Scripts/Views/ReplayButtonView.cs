using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ReplayButtonView : MazeViewBase
{
    private bool _isPlaying = true;
    private Button _pauseButton;

    private void Awake()
    {
        base.Awake();
        _pauseButton = GetComponent<Button>();


        _pauseButton.onClick.AddListener(HandleClickButton);
    }

    private void HandleClickButton() {
        _isPlaying = !_isPlaying;
        _mazeAnimation.ResetAnimation();
    }
}
