using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeViewBase : MonoBehaviour
{
    [Header("Models")]
    [SerializeField]
    protected MazeSettings _mazeSettings;
    [SerializeField]
    protected MazeAnimation _mazeAnimation;

    public virtual void Awake()
    {
        if (_mazeSettings == null)
        {
            _mazeSettings = FindObjectOfType<MazeSettings>();
        }

        if (_mazeAnimation == null)
        {
            _mazeAnimation = FindObjectOfType<MazeAnimation>();
        }
    }
}
