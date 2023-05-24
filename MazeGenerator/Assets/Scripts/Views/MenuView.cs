using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MenuView : MonoBehaviour
{
    [SerializeField]
    private RectTransform _menuRect;
    private float _defaultRectWidth;

    [SerializeField]
    private float _transitionTime =5.0f;
    [SerializeField]
    private RectTransform _layoutGroup;

    private bool _menuIsOpen = true;

    private void Awake()
    {
        _defaultRectWidth = _menuRect.rect.width;
    }

    public void ToggleMenu()
    {
        if (_menuIsOpen)
        {
            StartCoroutine(MenuLerp(0));
        }
        else {
            StartCoroutine(MenuLerp(_defaultRectWidth));
        }
        _menuIsOpen = !_menuIsOpen;
    }

    private IEnumerator MenuLerp(float targetWidth)
    {
        float timeElapsed = 0;
        float startWidth = _menuRect.rect.width;

        while (timeElapsed < _transitionTime)
        {
            _menuRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Lerp(startWidth, targetWidth, timeElapsed / _transitionTime));
            timeElapsed += Time.deltaTime;
            LayoutRebuilder.MarkLayoutForRebuild(_layoutGroup);
            yield return null;
        }

        _menuRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, targetWidth);
        LayoutRebuilder.MarkLayoutForRebuild(_layoutGroup);
    }
}
