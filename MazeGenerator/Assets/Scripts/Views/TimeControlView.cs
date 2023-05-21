using Assets.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeControlView : MonoBehaviour
{
    [Header("Models")]
    [SerializeField]
    private MazeSettings _mazeSettings;

    [SerializeField]
    private MazeAnimation _mazeAnimation;

    void Awake()
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
