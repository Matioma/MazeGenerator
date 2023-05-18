using Assets.Scripts;
using System.Collections;
using UnityEngine;


[RequireComponent(typeof(Camera))]
public class CameraView : MonoBehaviour
{
    [SerializeField]
    private MazeBuilder _mazeSettings;

    private Camera _camera;


    private Vector3 newCameraPosition;
    [SerializeField]
    private float transitionTime = 1.5f;

    public void Awake()
    {
        if (_mazeSettings == null)
        {
            _mazeSettings = FindObjectOfType<MazeBuilder>();
        }

        _camera = GetComponent<Camera>();
        _mazeSettings.mazeCreated.AddListener(AdjustCamera);
        newCameraPosition = transform.position;
    }

    private void AdjustCamera(MazeSettings settings)
    {
        StopAllCoroutines();

        var HalfFOV = _camera.fieldOfView / 2;
        var sizeToSeeInHalfFOV = Mathf.Max(settings.Width, settings.Depth) / 2 * settings.BlockScale;

        var distance = sizeToSeeInHalfFOV / Mathf.Abs(Mathf.Sin(Mathf.Deg2Rad * HalfFOV));
        newCameraPosition = new Vector3(0, distance, 0);
        
        StartCoroutine(TransitionCamera());
    }

    private IEnumerator TransitionCamera()
    {
        float timeElapsed = 0;
        Vector3 startLerpPosition = transform.position;
        while (timeElapsed < transitionTime)
        {
            transform.position = Vector3.Lerp(startLerpPosition, newCameraPosition, timeElapsed / transitionTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = newCameraPosition;
    }
}
