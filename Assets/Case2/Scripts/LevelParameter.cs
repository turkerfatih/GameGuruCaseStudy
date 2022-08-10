using UnityEngine;

namespace Case2
{
    [SerializeField]
    public struct LevelParameter
    {
        public float Width;
        public float Height;
        public float Length;
        public float ToleranceWidth;
        public float Speed;
        public int PieceCount;
        public float FinalPosition;
    }
}