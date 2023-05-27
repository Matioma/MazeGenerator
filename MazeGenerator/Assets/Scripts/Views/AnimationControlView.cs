using Assets.Scripts;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnimationControlView : MazeViewBase
{
    [SerializeField]
    private GameObject _replayButton;

    [SerializeField]
    private GameObject _playButton;


    [SerializeField]
    private Button _nextFrameButton;
    [SerializeField]
    private Button _previousFrameButton;

    public override void Awake()
    {
        base.Awake();

        HandleAnimationToggle(_mazeSettings.Animate);
        _mazeSettings.onAnimateChanged.AddListener(HandleAnimationToggle);

        _mazeAnimation.onAnimationFinished.AddListener(HandleAnimationFinished);
        _mazeAnimation.onAminationStart.AddListener(HandleAnimationStarted);

        _nextFrameButton.onClick.AddListener(HandleNextFrameClick);
        _previousFrameButton.onClick.AddListener(HandlePreviousFrameClick);
    }

    private void HandleNextFrameClick(){
        _mazeAnimation.RenderNext();
    }

    private void HandlePreviousFrameClick()
    {
        _mazeAnimation.RenderPrevious();
    }

    private void HandleAnimationFinished() {
        _replayButton.SetActive(true);
        _playButton.SetActive(false);
    }

    private void HandleAnimationStarted()
    {
        _replayButton.SetActive(false);
        _playButton.SetActive(true);
    }


    private void HandleAnimationToggle(bool newValue) {
        if(newValue)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    public void Show()
    {
        foreach (var child in transform.GetComponentsInChildren<RectTransform>(true).Where(x=>x.parent == transform))
        {
            child.gameObject.SetActive(true);
        }
    }

    public void Hide()
    {
        foreach (var child in transform.GetComponentsInChildren<RectTransform>(true).Where(x=>x.parent == transform))
        {
            child.gameObject.SetActive(false);
        }
    }

}
