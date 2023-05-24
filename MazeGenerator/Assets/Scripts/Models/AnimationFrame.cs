using UnityEngine;

public class AnimationFrame
{
    public Vector2Int Path;
    public Vector2Int[] Walls;

    public bool isReverse = false;

    public AnimationFrame(Vector2Int key, Vector2Int[] value)
    {
        Path = key;
        Walls = value;
    }
}
