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

    void Awake()
    {
        base.Awake();

        handleAnimationToggle(_mazeSettings.Animate);
        _mazeSettings.onAnimateChanged.AddListener(handleAnimationToggle);

        _mazeAnimation.onAnimationFinished.AddListener(handleAnimationFinished);
        _mazeAnimation.onAminationStart.AddListener(handleAnimationStarted);
    }

    private void handleAnimationFinished() {
        _replayButton.SetActive(true);
        _playButton.SetActive(false);
    }

    private void handleAnimationStarted()
    {
        _replayButton.SetActive(false);
        _playButton.SetActive(true);
    }


    private void handleAnimationToggle(bool newValue) {
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
