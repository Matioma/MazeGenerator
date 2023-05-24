using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animation))]
public class SpawnEffect : MonoBehaviour
{
    [SerializeField]
    private string showTriggerName;
    [SerializeField]
    private string hideTriggerName;

    [SerializeField]
    private AnimationClip animationClip;

    private Animation _animation;

    public void Awake()
    {
        _animation = GetComponent<Animation>();
    }


    public void Update() { 
        if(!_animation.isPlaying) {
            _animation.enabled = false; 
        }
        
    }

    public void Show() {
        _animation.enabled = true;
        _animation.Play();
        //GetComponent<Animation>().Play();
        //GetComponent<Animation>().is


    }

    public void Hide() {
        //GetComponent<Animator>().SetTrigger(hideTriggerName);
    }

    public void OnEnable()
    {
        Show();
    }

    public void OnDisable()
    {
        Hide(); 
    }
}
