using Assets.Scripts;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MazeSettings))]
[RequireComponent (typeof(MazeAnimation))]
public class MazeBuilder : MonoBehaviour
{
    [SerializeField]
    private GameObject _mazeUnit;
    private MazeSettings _mazeSettings;
    private MazeAnimation _mazeAnimation;

    public UnityEvent<MazeSettings> mazeCreated;

    private GameObject[,] puzzleUnitCache;

    Vector2Int indexOffset;
    Vector3 mazeStartLocation;

    public void Awake()
    {
        _mazeSettings = GetComponent<MazeSettings>();
        _mazeAnimation = GetComponent<MazeAnimation>();

        _mazeAnimation.onRenderNextFrame.AddListener(HandleNextFrame);
        _mazeAnimation.onResetAnimation.AddListener(HideCache);
        InitializeCache();
    }

    public void Generate()
    {
        StopAllCoroutines();
        IAnimationGeneration generatorAnimation = new PrimsMaze();
        var animationFrames = generatorAnimation.GetMazeAnimation(_mazeSettings);

        _mazeAnimation.LoadAnimation(animationFrames);
        HideCache();
        indexOffset = new Vector2Int((MazeSettings.MAX_DIMENSION - _mazeSettings.Width) / 2, (MazeSettings.MAX_DIMENSION - _mazeSettings.Depth) / 2);
        mazeStartLocation = GetMazeStartLocation();

        _mazeAnimation.StartAnimation();

        mazeCreated.Invoke(_mazeSettings);
    }

    private void HandleNextFrame(AnimationFrame nextFrame) {
        if (nextFrame == null) {
            return;
        }
        RenderFrame(indexOffset, mazeStartLocation, nextFrame, !nextFrame.isReverse);
    }

    private void InitializeCache()
    {
        puzzleUnitCache = new GameObject[MazeSettings.MAX_DIMENSION, MazeSettings.MAX_DIMENSION];
        for (var x = 0; x < puzzleUnitCache.GetLength(0); x++)
        {
            for (var z = 0; z < puzzleUnitCache.GetLength(1); z++)
            {
                var scale = new Vector3(_mazeSettings.BlockScale, _mazeSettings.BlockScale, _mazeSettings.BlockScale);

                GameObject createdPuzzleUnit = Instantiate(_mazeUnit,Vector3.zero, Quaternion.identity, transform);
                createdPuzzleUnit.transform.localScale = scale;
                createdPuzzleUnit.SetActive(false);
                puzzleUnitCache[x, z] = createdPuzzleUnit;
            }
        }
    }

    private IEnumerator PlayAnimation(MazeAnimation mazeAnimationFrames)
    {
        HideCache();
        var indexOffset = new Vector2Int((MazeSettings.MAX_DIMENSION - _mazeSettings.Width) / 2, (MazeSettings.MAX_DIMENSION - _mazeSettings.Depth) / 2);
        var mazeStartLocation = GetMazeStartLocation();

        var frameCounter = 0;
        AnimationFrame nextFrame;
        while ((nextFrame = mazeAnimationFrames.GetNext()) !=null)
        {
            frameCounter++;
            RenderFrame(indexOffset, mazeStartLocation, nextFrame, true);
            if (frameCounter % 100 == 0)
            {
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    private void RenderFrame(Vector2Int indexOffset, Vector3 mazeStartLocation, AnimationFrame frame, bool isAddingWalls)
    {
        foreach (var wall in frame.Value)
        {
            var blockOffset = new Vector3(wall.x * _mazeSettings.BlockScale, 0, wall.y * _mazeSettings.BlockScale);
            puzzleUnitCache[indexOffset.x + wall.x, indexOffset.y + wall.y].SetActive(isAddingWalls);
            puzzleUnitCache[indexOffset.x + wall.x, indexOffset.y + wall.y].transform.position = mazeStartLocation + blockOffset;
        }

        if (frame.Key is Vector2Int cell)
        {
            puzzleUnitCache[indexOffset.x + cell.x, indexOffset.y + cell.y].SetActive(!isAddingWalls);
        }
    }

    private Vector3 GetMazeStartLocation()
    {
        float mazeStartX = -_mazeSettings.Width / 2 * _mazeSettings.BlockScale;
        float mazeStartz = -_mazeSettings.Depth / 2 * _mazeSettings.BlockScale;
        return transform.position + new Vector3(mazeStartX, 0, mazeStartz);
    }

    private void HideCache()
    {
        foreach (var puzzleItem in puzzleUnitCache) {
            puzzleItem.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        mazeCreated.RemoveAllListeners();
    }
}
