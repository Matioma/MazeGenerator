using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MazeSettings))]
public class MazeBuilder : MonoBehaviour
{
    [SerializeField]
    private GameObject _puzzleUnit;

    private MazeSettings _mazeSettings;

    public UnityEvent<MazeSettings> mazeCreated;


    public void Awake()
    {
        _mazeSettings = GetComponent<MazeSettings>();
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
            //var mazeSteps = generatorAnimation.GenerateMazeSteps(_mazeSettings.Width, _mazeSettings.Depth);
            var mazeStepsDelta  = generatorAnimation.GenerateMazeStepsDelta(_mazeSettings.Width, _mazeSettings.Depth);
            StartCoroutine(DisplayMazeAnimationUsingDelta(mazeStepsDelta));
            //DisplayMazeAnimation(mazeSteps);
            //StartCoroutine(DisplayMazeAnimation(mazeSteps));
        }

        mazeCreated.Invoke(_mazeSettings);
    }

    private IEnumerator DisplayMazeAnimation(List<bool[,]> mazeAnimationSteps)
    {
        DestoryMaze();
        var mazeStartLocation =GetMazeStartLocation();

        var firstAnimationFrame = mazeAnimationSteps[0];


        //CreateObjectPool to avoid to much deletion/creation
        GameObject[,] objectPool = new GameObject[firstAnimationFrame.GetLength(0), firstAnimationFrame.GetLength(1)];
        for (var x = 0; x < mazeAnimationSteps[0].GetLength(0); x++)
        {
            for (var z = 0; z < mazeAnimationSteps[0].GetLength(1); z++)
            {
                GameObject createdPuzzleUnit = Instantiate(_puzzleUnit, Vector3.zero, Quaternion.identity, transform);
                createdPuzzleUnit.transform.localScale = new Vector3(_mazeSettings.BlockScale, _mazeSettings.BlockScale, _mazeSettings.BlockScale);
                createdPuzzleUnit.SetActive(false);
                objectPool[x, z] = createdPuzzleUnit;
            }
        }

        // Build Animation Frame
        for ( var i=0; i< mazeAnimationSteps.Count; i++)
        {
            for (var x = 0; x < mazeAnimationSteps[i].GetLength(0); x++)
            {
                for (var z = 0; z < mazeAnimationSteps[i].GetLength(1); z++)
                {
                    Vector3 blockOffset = new Vector3(x * _mazeSettings.BlockScale, 0, z * _mazeSettings.BlockScale);
                    objectPool[x, z].transform.position = mazeStartLocation + blockOffset;
                    if (mazeAnimationSteps[i][x, z])
                    {
                        objectPool[x, z].SetActive(false);
                    } else if (i == 0)
                    {
                        objectPool[x, z].SetActive(true);
                    }

                }
            }
            yield return new WaitForSeconds(0.001f);

        }
        foreach(var mazeFrame in mazeAnimationSteps) { 
            
        }
    }


    private IEnumerator DisplayMazeAnimationUsingDelta(List<Vector2Int> mazeAnimationSteps)
    {
        DestoryMaze();
        var mazeStartLocation = GetMazeStartLocation();

        //var firstAnimationFrame = mazeAnimationSteps[0];


        //CreateObjectPool to avoid to much deletion/creation
        GameObject[,] objectPool = new GameObject[_mazeSettings.Width, _mazeSettings.Depth];
        for (var x = 0; x < objectPool.GetLength(0); x++)
        {
            for (var z = 0; z < objectPool.GetLength(1); z++)
            {
                GameObject createdPuzzleUnit = Instantiate(_puzzleUnit, Vector3.zero, Quaternion.identity, transform);
                createdPuzzleUnit.transform.localScale = new Vector3(_mazeSettings.BlockScale, _mazeSettings.BlockScale, _mazeSettings.BlockScale);
                Vector3 blockOffset = new Vector3(x * _mazeSettings.BlockScale, 0, z * _mazeSettings.BlockScale);
                createdPuzzleUnit.transform.position = mazeStartLocation + blockOffset;
                objectPool[x, z] = createdPuzzleUnit;
            }
        }

        Debug.Log(mazeAnimationSteps.Count);
        // Build Animation Frame
        for (var i = 0; i < mazeAnimationSteps.Count; i++)
        {
            var position = mazeAnimationSteps[i];

            objectPool[position.x, position.y].SetActive(false);
            //for (var x = 0; x < mazeAnimationSteps[i].GetLength(0); x++)
            //{
            //    for (var z = 0; z < mazeAnimationSteps[i].GetLength(1); z++)
            //    {
            //        Vector3 blockOffset = new Vector3(x * _mazeSettings.BlockScale, 0, z * _mazeSettings.BlockScale);
            //        objectPool[x, z].transform.position = mazeStartLocation + blockOffset;
            //        if (mazeAnimationSteps[i][x, z])
            //        {
            //            objectPool[x, z].SetActive(false);
            //        }
            //        else if (i == 0)
            //        {
            //            objectPool[x, z].SetActive(true);
            //        }

            //    }
            //}
            yield return new WaitForSeconds(0.001f);

        }
    }


    private Vector3 GetMazeStartLocation()
    {
        float mazeStartX = -_mazeSettings.Width / 2 * _mazeSettings.BlockScale;
        float mazeStartz = -_mazeSettings.Depth / 2 * _mazeSettings.BlockScale;
        return transform.position + new Vector3(mazeStartX, 0, mazeStartz);
    }

    private void BuildMaze(bool[,] mazedata) {
        DestoryMaze();
        float mazeStartX = -_mazeSettings.Width / 2 * _mazeSettings.BlockScale;
        float mazeStartz = -_mazeSettings.Depth / 2 * _mazeSettings.BlockScale;
        Vector3 mazeStartPoint = transform.position + new Vector3(mazeStartX, 0, mazeStartz);



       for (var x = 0; x < mazedata.GetLength(0); x++)
        {
            for (var z = 0; z < mazedata.GetLength(1); z++) {

                if (mazedata[x,z])
                {
                    continue;
                }

                Vector3 blockOffset = new Vector3(x * _mazeSettings.BlockScale, 0, z * _mazeSettings.BlockScale);
                Vector3 scale = new Vector3(_mazeSettings.BlockScale, _mazeSettings.BlockScale, _mazeSettings.BlockScale);
                GameObject createdPuzzleUnit = Instantiate(_puzzleUnit, mazeStartPoint + blockOffset, Quaternion.identity, transform);
                createdPuzzleUnit.transform.localScale = scale;
            }
        }
    }

    private void DestoryMaze() {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void OnDestroy()
    {
        mazeCreated.RemoveAllListeners();
    }
}
