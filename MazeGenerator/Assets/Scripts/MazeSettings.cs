using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public class MazeSettings:MonoBehaviour
    {
        public UnityEvent<int> onWidthChange;
        public UnityEvent<int> onDepthChange;

        [SerializeField]
        private int _width;
        [SerializeField]
        private int _depth;


        public int Width {
            get { return _width; } 
            set { 
                _width = value;
                onWidthChange.Invoke(value);
            }
        }

        public int Depth
        {
            get { return _depth; }
            set
            {
                _depth = value;
                onDepthChange.Invoke(value);
            }
        }

        public void OnDestroy()
        {
            onWidthChange.RemoveAllListeners();
            onDepthChange.RemoveAllListeners();
        }
    }
}
