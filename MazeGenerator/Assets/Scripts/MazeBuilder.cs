using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

[RequireComponent(typeof(MazeSettings))]
public class MazeBuilder : MonoBehaviour
{
    [SerializeField]
    private GameObject _puzzleUnit;

    private MazeSettings _mazeSettings;

    public UnityEvent<MazeSettings> mazeCreated;

    private GameObject[,] puzzleUnitCache; 
    public void Awake()
    {
        _mazeSettings = GetComponent<MazeSettings>();

        InitializeCache();
    }

    public void Generate()
    {
        StopAllCoroutines();
        var primsMaze = new PrimsMaze();

        IGenerator mazeGenerator = primsMaze;
        if (!_mazeSettings.animate) {
            BuildMaze(mazeGenerator.GenerateMaze(_mazeSettings.Width, _mazeSettings.Depth));
        }
        else
        {
            IGeneratorAnimation generatorAnimation = primsMaze;
            var mazeWalls = generatorAnimation.GenerateMazeStepsWalls(_mazeSettings.Width, _mazeSettings.Depth);
            StartCoroutine(DisplayMazeAnimationUsingWallsAddition(mazeWalls));
            //var mazeStepsDelta  = generatorAnimation.GenerateMazeStepsDelta(_mazeSettings.Width, _mazeSettings.Depth);
            //StartCoroutine(DisplayMazeAnimationUsingDelta(mazeStepsDelta));
        }

        mazeCreated.Invoke(_mazeSettings);
    }

    private void InitializeCache()
    {
        puzzleUnitCache = new GameObject[MazeSettings.MAX_DIMENSION, MazeSettings.MAX_DIMENSION];
        for (var x = 0; x < puzzleUnitCache.GetLength(0); x++)
        {
            for (var z = 0; z < puzzleUnitCache.GetLength(1); z++)
            {
                var scale = new Vector3(_mazeSettings.BlockScale, _mazeSettings.BlockScale, _mazeSettings.BlockScale);

                GameObject createdPuzzleUnit = Instantiate(_puzzleUnit,Vector3.zero, Quaternion.identity, transform);
                createdPuzzleUnit.transform.localScale = scale;
                createdPuzzleUnit.SetActive(false);
                puzzleUnitCache[x, z] = createdPuzzleUnit;
            }
        }
    }

    private IEnumerator DisplayMazeAnimationUsingDelta(List<Vector2Int> mazeAnimationSteps)
    {
        HideCache();
        var mazeStartLocation = GetMazeStartLocation();

        for (var x = 0; x < _mazeSettings.Width; x++)
        {
            for (var z = 0; z < _mazeSettings.Depth; z++)
            {
                var blockOffset = new Vector3(x * _mazeSettings.BlockScale, 0, z * _mazeSettings.BlockScale);
                puzzleUnitCache[x, z].transform.position = mazeStartLocation+ blockOffset;
                puzzleUnitCache[x, z].SetActive(true);
            }
        }

        //Display Animation Frames
        for (var i = 0; i < mazeAnimationSteps.Count; i++)
        {
            var position = mazeAnimationSteps[i];
            puzzleUnitCache[position.x, position.y].SetActive(false);

            if (i % 300 == 0)
            {
                yield return new WaitForSeconds(0.1f);

            }
        }
    }

    private IEnumerator DisplayMazeAnimationUsingWallsAddition(Dictionary<Vector2Int, Vector2Int[]> mazeAnimationFrames)
    {
        HideCache();
        var indexOffset = new Vector2Int((MazeSettings.MAX_DIMENSION-_mazeSettings.Width)/2, (MazeSettings.MAX_DIMENSION - _mazeSettings.Depth) / 2);
        var mazeStartLocation = GetMazeStartLocation();

        var frameCounter = 0;
        foreach (var frame in mazeAnimationFrames)
        {
            frameCounter++;
            foreach (var wall in frame.Value) {
                var blockOffset = new Vector3(wall.x * _mazeSettings.BlockScale, 0, wall.y * _mazeSettings.BlockScale);
                puzzleUnitCache[indexOffset.x + wall.x, indexOffset.y + wall.y].SetActive(true);
                puzzleUnitCache[indexOffset.x + wall.x, indexOffset.y + wall.y].transform.position = mazeStartLocation + blockOffset;
            }

            puzzleUnitCache[indexOffset.x + frame.Key.x, indexOffset.y + frame.Key.y].SetActive(false);
            if (frameCounter % 50 == 0)
            {
                yield return new WaitForSeconds(0.1f);

            }
        }

        //var mazeStartLocation = GetMazeStartLocation();

        //for (var x = 0; x < _mazeSettings.Width; x++)
        //{
        //    for (var z = 0; z < _mazeSettings.Depth; z++)
        //    {
        //        var blockOffset = new Vector3(x * _mazeSettings.BlockScale, 0, z * _mazeSettings.BlockScale);
        //        puzzleUnitCache[x, z].transform.position = mazeStartLocation + blockOffset;
        //        puzzleUnitCache[x, z].SetActive(true);
        //    }
        //}

        ////Display Animation Frames
        //for (var i = 0; i < mazeAnimationSteps.Count; i++)
        //{
        //    var position = mazeAnimationSteps[i];
        //    puzzleUnitCache[position.x, position.y].SetActive(false);

        //    if (i % 300 == 0)
        //    {

        //    }
        //}
    }

    private Vector3 GetMazeStartLocation()
    {
        float mazeStartX = -_mazeSettings.Width / 2 * _mazeSettings.BlockScale;
        float mazeStartz = -_mazeSettings.Depth / 2 * _mazeSettings.BlockScale;
        return transform.position + new Vector3(mazeStartX, 0, mazeStartz);
    }

    private void BuildMaze(bool[,] mazedata) {
        HideCache();
        Vector3 mazeStartPoint = GetMazeStartLocation();


       for (var x = 0; x < mazedata.GetLength(0); x++)
        {
            for (var z = 0; z < mazedata.GetLength(1); z++) {
                if (mazedata[x,z])
                {
                    continue;
                }
                var blockOffset = new Vector3(x * _mazeSettings.BlockScale, 0, z * _mazeSettings.BlockScale);
                puzzleUnitCache[x, z].transform.position = mazeStartPoint + blockOffset;
                puzzleUnitCache[x, z].SetActive(true);
            }
        }
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
