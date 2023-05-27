using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.TimeZoneInfo;
using UnityEngine.UI;

public class SpawnEffect : MonoBehaviour
{
    [SerializeField]
    private float _startY;
    [SerializeField]
    private float _TargetY;
    [SerializeField]
    private float _animationLength;

    public void Show() {
       StopAllCoroutines();
        StartCoroutine(PlayAnimation(_startY, _TargetY, _animationLength));
    }
    private IEnumerator PlayAnimation(float StartHeight, float TargetHeight, float transitionTime)
    {
        float timeElapsed = 0;
        while (timeElapsed < transitionTime)
        {
            var newYPosition = Mathf.Lerp(StartHeight, TargetHeight, timeElapsed / transitionTime);
            transform.position = new Vector3(transform.position.x, newYPosition, transform.position.z); ;

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = new Vector3(transform.position.x, TargetHeight, transform.position.z); ;
    }

    public void OnEnable()
    {
        Show();
    }
}
