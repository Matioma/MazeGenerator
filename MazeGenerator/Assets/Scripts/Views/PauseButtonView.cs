using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class PauseButtonView : MazeViewBase
{
    [Header("ButtonSettings")]
    [SerializeField]
    private Texture2D pauseImage;
    [SerializeField]
    private Texture2D playImage;
    [SerializeField]
    private RawImage image;

    private bool _isPlaying = true;
    private Button _pauseButton;

    private void Awake()
    {
        base.Awake();
        _pauseButton = GetComponent<Button>();

        SetImage(_isPlaying);

        _pauseButton.onClick.AddListener(HandleClickButton);
    }

    private void HandleClickButton() {
        _isPlaying = !_isPlaying;
        SetImage(_isPlaying);
        if (_isPlaying)
        {
            _mazeAnimation.Play();
        }
        else { 
            _mazeAnimation.Pause();
        }
    }

    private void SetImage(bool isPlaying) { 
        if(isPlaying)
        {
            image.texture = pauseImage;
        }
        else
        {
            image.texture = playImage;
        }
    }
}
