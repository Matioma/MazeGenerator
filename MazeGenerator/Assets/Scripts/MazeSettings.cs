using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public class MazeSettings:MonoBehaviour
    {
        public UnityEvent<int> onWidthChange;
        public UnityEvent<int> onDepthChange;

        public const int MAX_DIMENSION = 250;
        public const int MIN_DIMENSION = 10;

        [Range(MIN_DIMENSION, MAX_DIMENSION)]
        [SerializeField]
        private int _width = MIN_DIMENSION;

        [Range(MIN_DIMENSION, MAX_DIMENSION)]
        [SerializeField]
        private int _depth = MIN_DIMENSION;

        [SerializeField]
        private int _blockScale = 3;

        public int Width {
            get { return _width; } 
            set {
                if (value < MIN_DIMENSION || value > MAX_DIMENSION) {
                    return;
                }
                _width = value;
                onWidthChange.Invoke(value);
            }
        }

        public int Depth
        {
            get { return _depth; }
            set
            {
                if (value < MIN_DIMENSION || value > MAX_DIMENSION)
                {
                    return;
                }
                _depth = value;
                onDepthChange.Invoke(value);
            }
        }

        public int BlockScale
        {
            get { return _blockScale; }
            set
            {
                _blockScale = value;
            }
        }

        public void OnDestroy()
        {
            onWidthChange.RemoveAllListeners();
            onDepthChange.RemoveAllListeners();
        }
    }
}
