using UnityEngine;

namespace Src.Signals
{
    public struct ItemChangedSignal
    {
        public int Index;
        public Sprite Sprite;
        public Color Color;
        public int StackCount;
    }
}