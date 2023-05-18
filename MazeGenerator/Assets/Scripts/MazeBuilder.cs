using Assets.Scripts;
using Newtonsoft.Json.Linq;
using Unity.VisualScripting;
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
        IGenerator primsMaze = new PrimsMaze();
        BuildMaze(primsMaze.GenerateMaze(_mazeSettings.Width, _mazeSettings.Depth));
        mazeCreated.Invoke(_mazeSettings);
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
