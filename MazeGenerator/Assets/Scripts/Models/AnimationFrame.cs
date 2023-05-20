using UnityEngine;

public class AnimationFrame
{
    public Vector2Int Key;
    public Vector2Int[] Value;

    public AnimationFrame(Vector2Int key, Vector2Int[] value)
    {
        Key = key;
        Value = value;
    }
}
