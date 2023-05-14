using Assets.Scripts;
using UnityEngine;

public class MazeBuilder : MonoBehaviour
{
    [SerializeField]
    private GameObject puzzleUnit;

    private const int width = 20;
    private const int depth = 20;
    private const int unitScale = 3;

    public void Generate()
    {
        IGenerator primsMaze = new PrimsMaze();
        BuildMaze(primsMaze.GenerateMaze(width, depth));
    }

    private void BuildMaze(bool[,] mazedata) {
       DestoryMaze();
       for (var x = 0; x < mazedata.GetLength(0); x++)
        {
            for (var z = 0; z < mazedata.GetLength(1); z++) {

                if (mazedata[x,z])
                {
                    continue;
                }

                Vector3 position = new Vector3(x * unitScale, 0, z * unitScale);
                Vector3 scale = new Vector3(unitScale, unitScale, unitScale);
                GameObject createdPuzzleUnit = Instantiate(puzzleUnit, position, Quaternion.identity, transform);
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
}
