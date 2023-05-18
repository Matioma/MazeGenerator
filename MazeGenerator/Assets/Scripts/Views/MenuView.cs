using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static System.TimeZoneInfo;

public class MenuView : MonoBehaviour
{
    [SerializeField]
    private RectTransform _menuRect;
    private float _defaultRectWidth;

    [SerializeField]
    private float transitionTime =5.0f;
    [SerializeField]
    private RectTransform layoutGroup;

    private bool _menuIsOpen = true;

    private void Awake()
    {
        _defaultRectWidth = _menuRect.rect.width;
    }

    public void ToggleMenu()
    {
        if (_menuIsOpen)
        {
            StartCoroutine(menuLerp(0));
        }
        else {
            StartCoroutine(menuLerp(_defaultRectWidth));
        }
        _menuIsOpen = !_menuIsOpen;
    }

    private IEnumerator menuLerp(float targetWidth)
    {
        float timeElapsed = 0;
        float startWidth = _menuRect.rect.width;

        while (timeElapsed < transitionTime)
        {
            _menuRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Lerp(startWidth, targetWidth, timeElapsed / transitionTime));
            timeElapsed += Time.deltaTime;
            LayoutRebuilder.MarkLayoutForRebuild(layoutGroup);
            yield return null;
        }
    }
}
