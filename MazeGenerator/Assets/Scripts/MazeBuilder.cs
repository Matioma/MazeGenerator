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

    private GameObject[,] _mazeUnitsPool;
    private Vector2Int _indexOffsetRelativeToThePool;
    private Vector3 _mazePositionEdgeCorner;

    public void Awake()
    {
        _mazeSettings = GetComponent<MazeSettings>();
        _mazeAnimation = GetComponent<MazeAnimation>();

        _mazeAnimation.onRenderNextFrame.AddListener(HandleNextFrame);
        _mazeAnimation.onAminationStart.AddListener(HidePool);
        InitializePool();
    }

    public void Generate()
    {
        StopAllCoroutines();
        IAnimationGeneration generatorAnimation = new PrimsMaze();
        var animationFrames = generatorAnimation.GetMazeAnimation(_mazeSettings);
        _mazeAnimation.LoadAnimation(animationFrames);
        
        HidePool();
        _indexOffsetRelativeToThePool = new Vector2Int((MazeSettings.MAX_DIMENSION - _mazeSettings.Width) / 2, (MazeSettings.MAX_DIMENSION - _mazeSettings.Depth) / 2);
        _mazePositionEdgeCorner = GetMazeStartLocation();
        _mazeAnimation.StartAnimation();

        mazeCreated.Invoke(_mazeSettings);
    }

    private void HandleNextFrame(AnimationFrame nextFrame) {
        if (nextFrame == null) {
            return;
        }
        RenderFrame(_indexOffsetRelativeToThePool, _mazePositionEdgeCorner, nextFrame, !nextFrame.isReverse);
    }

    private void InitializePool()
    {
        _mazeUnitsPool = new GameObject[MazeSettings.MAX_DIMENSION, MazeSettings.MAX_DIMENSION];
        StartCoroutine(FillInThePoolGradually());
    }
    private void RenderFrame(Vector2Int indexOffset, Vector3 mazeStartLocation, AnimationFrame frame, bool isAddingWalls)
    {
        foreach (var wall in frame.Walls)
        {
            if (_mazeUnitsPool[indexOffset.x + wall.x, indexOffset.y + wall.y] == null)
            {
                CreateWall(indexOffset,wall);
            }

            var blockPositionOffset = new Vector3(wall.x * _mazeSettings.BlockScale, 0, wall.y * _mazeSettings.BlockScale);
            _mazeUnitsPool[indexOffset.x + wall.x, indexOffset.y + wall.y].transform.position = mazeStartLocation + blockPositionOffset;
            _mazeUnitsPool[indexOffset.x + wall.x, indexOffset.y + wall.y].SetActive(isAddingWalls);
        }

        if (frame.Path is Vector2Int cell)
        {
            if (_mazeUnitsPool[indexOffset.x + cell.x, indexOffset.y + cell.y] == null)
            {
                CreateWall(indexOffset, cell);
            }

            _mazeUnitsPool[indexOffset.x + cell.x, indexOffset.y + cell.y].SetActive(!isAddingWalls);
        }
    }

    private void CreateWall(Vector2Int indexOffset, Vector2Int wall)
    {
        var scale = new Vector3(_mazeSettings.BlockScale, _mazeSettings.BlockScale, _mazeSettings.BlockScale);

        GameObject createdPuzzleUnit = Instantiate(_mazeUnit, Vector3.zero, Quaternion.identity, transform);
        createdPuzzleUnit.transform.localScale = scale;
        createdPuzzleUnit.SetActive(false);
        _mazeUnitsPool[indexOffset.x + wall.x, indexOffset.y + wall.y] = createdPuzzleUnit;
    }

    private IEnumerator FillInThePoolGradually() {
        for (var x = 0; x < _mazeUnitsPool.GetLength(0); x++)
        {
            for (var z = 0; z < _mazeUnitsPool.GetLength(1); z++)
            {
                if(_mazeUnitsPool[x, z] != null)
                {
                    continue;
                }
                var scale = new Vector3(_mazeSettings.BlockScale, _mazeSettings.BlockScale, _mazeSettings.BlockScale);

                GameObject createdPuzzleUnit = Instantiate(_mazeUnit, Vector3.zero, Quaternion.identity, transform);
                createdPuzzleUnit.transform.localScale = scale;
                createdPuzzleUnit.SetActive(false);
                _mazeUnitsPool[x, z] = createdPuzzleUnit;
            }
            yield return new WaitForSeconds(0.2f);
        }
        
    }

    private Vector3 GetMazeStartLocation()
    {
        float mazeStartX = -_mazeSettings.Width / 2 * _mazeSettings.BlockScale;
        float mazeStartz = -_mazeSettings.Depth / 2 * _mazeSettings.BlockScale;
        return transform.position + new Vector3(mazeStartX, 0, mazeStartz);
    }

    private void HidePool()
    {
        foreach (var puzzleItem in _mazeUnitsPool) {
            puzzleItem?.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        mazeCreated.RemoveAllListeners();
    }
}
